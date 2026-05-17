using System.Threading.Tasks;
using GameServices.GameServices.Runtime.Core;

namespace GameServices.GameServices.Runtime.Audio
{
    public interface IAudioService : IGameService
    {
        float MasterVolume { get; }
        float MusicVolume { get; }
        float SfxVolume { get; }

        Task PlayMusicAsync(string musicId);
        Task StopMusicAsync();
        Task PlaySfxAsync(string sfxId);
        Task PlaySfxAsync(string sfxId, float pitch, float volumeScale = 1f);
        Task PlaySfxAsync(string sfxId, SfxPlayOptions options);
        void SetMasterVolume(float volume);
        void SetMusicVolume(float volume);
        void SetSfxVolume(float volume);
    }
}


