using System.Collections.Generic;
using System.Threading.Tasks;
using GameServices.GameServices.Runtime.Core;
using GameServices.GameServices.Runtime.Audio;
using UnityEngine;
using UnityEngine.Audio;

namespace GameServices.GameServices.Runtime.Audio
{
    public sealed class UnityAudioService : IAudioService
    {
        private readonly Dictionary<string, AudioClip> _musicClips;
        private readonly Dictionary<string, AudioClip> _sfxClips;
        private readonly bool _loopMusic;
        private readonly bool _persistAcrossScenes;
        private readonly bool _logWarnings;
        private readonly AudioMixerGroup _masterOutput;
        private readonly AudioMixerGroup _musicOutput;
        private readonly AudioMixerGroup _sfxOutput;

        private GameObject _root;
        private AudioSource _musicSource;
        private AudioSource _sfxSource;
        private GameServiceStatus _status = GameServiceStatus.NotInitialized;
        private float _masterVolume;
        private float _musicVolume;
        private float _sfxVolume;

        public UnityAudioService(
            IReadOnlyList<AudioClipDefinition> musicClips,
            IReadOnlyList<AudioClipDefinition> sfxClips,
            float masterVolume,
            float musicVolume,
            float sfxVolume,
            bool loopMusic,
            bool persistAcrossScenes,
            bool logWarnings,
            AudioMixerGroup masterOutput,
            AudioMixerGroup musicOutput,
            AudioMixerGroup sfxOutput)
        {
            _musicClips = BuildClipMap(musicClips);
            _sfxClips = BuildClipMap(sfxClips);
            _masterVolume = Mathf.Clamp01(masterVolume);
            _musicVolume = Mathf.Clamp01(musicVolume);
            _sfxVolume = Mathf.Clamp01(sfxVolume);
            _loopMusic = loopMusic;
            _persistAcrossScenes = persistAcrossScenes;
            _logWarnings = logWarnings;
            _masterOutput = masterOutput;
            _musicOutput = musicOutput;
            _sfxOutput = sfxOutput;
        }

        public string ServiceId => "audio.unity";
        public GameServiceStatus Status => _status;
        public bool IsReady => _status == GameServiceStatus.Ready;
        public float MasterVolume => _masterVolume;
        public float MusicVolume => _musicVolume;
        public float SfxVolume => _sfxVolume;

        public Task InitializeAsync(GameServiceContext context)
        {
            if (_status == GameServiceStatus.Ready)
            {
                return Task.CompletedTask;
            }

            _root = new GameObject("GameServices Audio");
            _musicSource = _root.AddComponent<AudioSource>();
            _sfxSource = _root.AddComponent<AudioSource>();

            ConfigureAudioSource(_musicSource, GetEffectiveMusicVolume(), _loopMusic, _musicOutput ?? _masterOutput);
            ConfigureAudioSource(_sfxSource, GetEffectiveSfxVolume(), false, _sfxOutput ?? _masterOutput);

            if (_persistAcrossScenes)
            {
                Object.DontDestroyOnLoad(_root);
            }

            _status = GameServiceStatus.Ready;
            return Task.CompletedTask;
        }

        public Task PlayMusicAsync(string musicId)
        {
            if (!IsReady || !_musicClips.TryGetValue(musicId, out var clip))
            {
                LogMissingClip("music", musicId);
                return Task.CompletedTask;
            }

            if (_musicSource.clip == clip && _musicSource.isPlaying)
            {
                return Task.CompletedTask;
            }

            _musicSource.clip = clip;
            _musicSource.volume = GetEffectiveMusicVolume();
            _musicSource.loop = _loopMusic;
            _musicSource.Play();

            return Task.CompletedTask;
        }

        public Task StopMusicAsync()
        {
            if (IsReady)
            {
                _musicSource.Stop();
            }

            return Task.CompletedTask;
        }

        public Task PlaySfxAsync(string sfxId)
        {
            if (!IsReady || !_sfxClips.TryGetValue(sfxId, out var clip))
            {
                LogMissingClip("sfx", sfxId);
                return Task.CompletedTask;
            }

            _sfxSource.PlayOneShot(clip, GetEffectiveSfxVolume());
            return Task.CompletedTask;
        }

        public void SetMasterVolume(float volume)
        {
            _masterVolume = Mathf.Clamp01(volume);
            RefreshSourceVolumes();
        }

        public void SetMusicVolume(float volume)
        {
            _musicVolume = Mathf.Clamp01(volume);
            RefreshSourceVolumes();
        }

        public void SetSfxVolume(float volume)
        {
            _sfxVolume = Mathf.Clamp01(volume);
            RefreshSourceVolumes();
        }

        private void RefreshSourceVolumes()
        {
            if (IsReady)
            {
                _musicSource.volume = GetEffectiveMusicVolume();
                _sfxSource.volume = GetEffectiveSfxVolume();
            }
        }

        private float GetEffectiveMusicVolume()
        {
            return _masterVolume * _musicVolume;
        }

        private float GetEffectiveSfxVolume()
        {
            return _masterVolume * _sfxVolume;
        }

        private static Dictionary<string, AudioClip> BuildClipMap(IReadOnlyList<AudioClipDefinition> clips)
        {
            var map = new Dictionary<string, AudioClip>();
            if (clips == null)
            {
                return map;
            }

            for (var i = 0; i < clips.Count; i++)
            {
                var definition = clips[i];
                if (definition == null || string.IsNullOrWhiteSpace(definition.Id) || definition.Clip == null)
                {
                    continue;
                }

                map[definition.Id] = definition.Clip;
            }

            return map;
        }

        private static void ConfigureAudioSource(
            AudioSource source,
            float volume,
            bool loop,
            AudioMixerGroup output)
        {
            source.playOnAwake = false;
            source.loop = loop;
            source.volume = volume;
            source.outputAudioMixerGroup = output;
        }

        private void LogMissingClip(string type, string id)
        {
            if (_logWarnings)
            {
                Debug.LogWarning($"Unity audio service could not find {type} clip with id '{id}'.");
            }
        }
    }
}


