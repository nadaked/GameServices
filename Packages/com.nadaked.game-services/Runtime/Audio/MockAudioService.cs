using System.Threading.Tasks;
using GameServices.GameServices.Runtime.Core;
using UnityEngine;

namespace GameServices.GameServices.Runtime.Audio
{
    public sealed class MockAudioService : IAudioService
    {
        private readonly bool _logCalls;
        private GameServiceStatus _status = GameServiceStatus.NotInitialized;
        private float _masterVolume;
        private float _musicVolume;
        private float _sfxVolume;

        public MockAudioService(float masterVolume, float musicVolume, float sfxVolume, bool logCalls)
        {
            _masterVolume = Mathf.Clamp01(masterVolume);
            _musicVolume = Mathf.Clamp01(musicVolume);
            _sfxVolume = Mathf.Clamp01(sfxVolume);
            _logCalls = logCalls;
        }

        public string ServiceId => "audio.mock";
        public GameServiceStatus Status => _status;
        public bool IsReady => _status == GameServiceStatus.Ready;
        public float MasterVolume => _masterVolume;
        public float MusicVolume => _musicVolume;
        public float SfxVolume => _sfxVolume;

        public Task InitializeAsync(GameServiceContext context)
        {
            _status = GameServiceStatus.Ready;
            Log("Mock audio service initialized.");
            return Task.CompletedTask;
        }

        public Task PlayMusicAsync(string musicId)
        {
            Log($"Mock music started: {musicId}");
            return Task.CompletedTask;
        }

        public Task StopMusicAsync()
        {
            Log("Mock music stopped.");
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
            Log($"Mock sfx played: {sfxId}, pitch: {options.Pitch}, volume scale: {options.VolumeScale}");
            return Task.CompletedTask;
        }

        public void SetMasterVolume(float volume)
        {
            _masterVolume = Mathf.Clamp01(volume);
            Log($"Mock master volume changed: {_masterVolume}");
        }

        public void SetMusicVolume(float volume)
        {
            _musicVolume = Mathf.Clamp01(volume);
            Log($"Mock music volume changed: {_musicVolume}");
        }

        public void SetSfxVolume(float volume)
        {
            _sfxVolume = Mathf.Clamp01(volume);
            Log($"Mock sfx volume changed: {_sfxVolume}");
        }

        private void Log(string message)
        {
            if (_logCalls)
            {
                Debug.Log(message);
            }
        }
    }
}


