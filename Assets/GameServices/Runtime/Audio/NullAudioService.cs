using System.Threading.Tasks;
using GameServices.Runtime.Core;

namespace GameServices.Runtime.Audio
{
    public sealed class NullAudioService : IAudioService
    {
        public string ServiceId => "audio.null";
        public GameServiceStatus Status => GameServiceStatus.Disabled;
        public bool IsReady => true;
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

        public void SetMusicVolume(float volume)
        {
        }

        public void SetSfxVolume(float volume)
        {
        }
    }
}
