using System.Threading.Tasks;
using GameServices.Runtime.Core;

namespace GameServices.Runtime.Audio
{
    public interface IAudioService : IGameService
    {
        float MusicVolume { get; }
        float SfxVolume { get; }

        Task PlayMusicAsync(string musicId);
        Task StopMusicAsync();
        Task PlaySfxAsync(string sfxId);
        void SetMusicVolume(float volume);
        void SetSfxVolume(float volume);
    }
}
