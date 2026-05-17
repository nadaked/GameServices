using System.Threading.Tasks;
using GameServices.GameServices.Runtime.Core;

namespace GameServices.GameServices.Runtime.Audio
{
    public sealed class NullAudioService : IAudioService
    {
        public string ServiceId => "audio.null";
        public GameServiceStatus Status => GameServiceStatus.Disabled;
        public bool IsReady => true;
        public float MasterVolume => 0f;
        public float MusicVolume => 0f;
        public float SfxVolume => 0f;

        public Task InitializeAsync(GameServiceContext context)
        {
            return Task.CompletedTask;
        }

        public Task PlayMusicAsync(string musicId)
        {
            return Task.CompletedTask;
        }

        public Task StopMusicAsync()
        {
            return Task.CompletedTask;
        }

        public Task PlaySfxAsync(string sfxId)
        {
            return Task.CompletedTask;
        }

        public Task PlaySfxAsync(string sfxId, float pitch, float volumeScale = 1f)
        {
            return Task.CompletedTask;
        }

        public Task PlaySfxAsync(string sfxId, SfxPlayOptions options)
        {
            return Task.CompletedTask;
        }

        public void SetMasterVolume(float volume)
        {
        }

        public void SetMusicVolume(float volume)
        {
        }

        public void SetSfxVolume(float volume)
        {
        }
    }
}


