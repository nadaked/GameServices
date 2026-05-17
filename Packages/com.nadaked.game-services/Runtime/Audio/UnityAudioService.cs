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
        private readonly int _sfxSourcePoolSize;
        private readonly float _minSfxPitch;
        private readonly float _maxSfxPitch;

        private GameObject _root;
        private AudioSource _musicSource;
        private AudioSource[] _sfxSources;
        private float[] _sfxSourceVolumeScales;
        private int _nextSfxSourceIndex;
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
            int sfxSourcePoolSize,
            float minSfxPitch,
            float maxSfxPitch,
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
            _sfxSourcePoolSize = Mathf.Max(1, sfxSourcePoolSize);
            _minSfxPitch = Mathf.Max(0.01f, Mathf.Min(minSfxPitch, maxSfxPitch));
            _maxSfxPitch = Mathf.Max(_minSfxPitch, Mathf.Max(minSfxPitch, maxSfxPitch));
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
            _sfxSources = new AudioSource[_sfxSourcePoolSize];
            _sfxSourceVolumeScales = new float[_sfxSourcePoolSize];

            ConfigureAudioSource(_musicSource, GetEffectiveMusicVolume(), _loopMusic, _musicOutput ?? _masterOutput);

            for (var i = 0; i < _sfxSources.Length; i++)
            {
                _sfxSourceVolumeScales[i] = 1f;
                _sfxSources[i] = _root.AddComponent<AudioSource>();
                ConfigureAudioSource(_sfxSources[i], GetEffectiveSfxVolume(), false, _sfxOutput ?? _masterOutput);
            }

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
            return PlaySfxAsync(sfxId, SfxPlayOptions.Default);
        }

        public Task PlaySfxAsync(string sfxId, float pitch, float volumeScale = 1f)
        {
            return PlaySfxAsync(sfxId, new SfxPlayOptions(pitch, volumeScale));
        }

        public Task PlaySfxAsync(string sfxId, SfxPlayOptions options)
        {
            if (!IsReady || !_sfxClips.TryGetValue(sfxId, out var clip))
            {
                LogMissingClip("sfx", sfxId);
                return Task.CompletedTask;
            }

            var sourceIndex = GetNextSfxSourceIndex();
            var source = _sfxSources[sourceIndex];
            var volumeScale = Mathf.Max(0f, options.VolumeScale);
            var pitch = Mathf.Clamp(GetPlayablePitch(options.Pitch), _minSfxPitch, _maxSfxPitch);

            _sfxSourceVolumeScales[sourceIndex] = volumeScale;
            source.Stop();
            source.clip = clip;
            source.pitch = pitch;
            source.volume = GetEffectiveSfxVolume() * volumeScale;
            source.loop = false;
            source.Play();

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

                for (var i = 0; i < _sfxSources.Length; i++)
                {
                    _sfxSources[i].volume = GetEffectiveSfxVolume() * _sfxSourceVolumeScales[i];
                }
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

        private int GetNextSfxSourceIndex()
        {
            for (var i = 0; i < _sfxSources.Length; i++)
            {
                var index = (_nextSfxSourceIndex + i) % _sfxSources.Length;
                if (!_sfxSources[index].isPlaying)
                {
                    _nextSfxSourceIndex = (index + 1) % _sfxSources.Length;
                    return index;
                }
            }

            var fallbackIndex = _nextSfxSourceIndex;
            _nextSfxSourceIndex = (_nextSfxSourceIndex + 1) % _sfxSources.Length;
            return fallbackIndex;
        }

        private static float GetPlayablePitch(float pitch)
        {
            return Mathf.Approximately(pitch, 0f) ? 1f : pitch;
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


