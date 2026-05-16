using GameServices.Runtime.Ads;
using GameServices.Runtime.Audio;
using GameServices.Runtime.Core;
using GameServices.Runtime.SceneLoading;
using UnityEngine;

namespace GameServices.Samples.Demo
{
    public sealed class GameServicesDemoController : MonoBehaviour
    {
        [SerializeField] private GameServicesProvider provider;
        [SerializeField] private string rewardedPlacementId = "demo_rewarded";
        [SerializeField] private string interstitialPlacementId = "demo_interstitial";
        [SerializeField] private string musicId = "demo_music";
        [SerializeField] private string sfxId = "demo_click";
        [SerializeField] private string sceneName = "Demo";

        [ContextMenu("Show Rewarded")]
        public async void ShowRewarded()
        {
            if (!HasProvider())
            {
                return;
            }

            var ads = provider.Get<IAdsService>();
            if (ads == null)
            {
                Debug.LogWarning("Ads service is not registered.", this);
                return;
            }

            var result = await ads.ShowRewardedAsync(rewardedPlacementId);
            Debug.Log($"Rewarded result: {result}", this);
        }

        [ContextMenu("Show Interstitial")]
        public async void ShowInterstitial()
        {
            if (!HasProvider())
            {
                return;
            }

            var ads = provider.Get<IAdsService>();
            if (ads == null)
            {
                Debug.LogWarning("Ads service is not registered.", this);
                return;
            }

            await ads.ShowInterstitialAsync(interstitialPlacementId);
        }

        [ContextMenu("Play Music")]
        public async void PlayMusic()
        {
            if (!HasProvider())
            {
                return;
            }

            var audio = provider.Get<IAudioService>();
            if (audio == null)
            {
                Debug.LogWarning("Audio service is not registered.", this);
                return;
            }

            await audio.PlayMusicAsync(musicId);
        }

        [ContextMenu("Play Sfx")]
        public async void PlaySfx()
        {
            if (!HasProvider())
            {
                return;
            }

            var audio = provider.Get<IAudioService>();
            if (audio == null)
            {
                Debug.LogWarning("Audio service is not registered.", this);
                return;
            }

            await audio.PlaySfxAsync(sfxId);
        }

        [ContextMenu("Load Scene")]
        public async void LoadScene()
        {
            if (!HasProvider())
            {
                return;
            }

            var sceneLoader = provider.Get<ISceneLoaderService>();
            if (sceneLoader == null)
            {
                Debug.LogWarning("Scene loader service is not registered.", this);
                return;
            }

            await sceneLoader.LoadSceneAsync(sceneName);
        }

        private bool HasProvider()
        {
            if (provider != null)
            {
                return true;
            }

            Debug.LogWarning("Game services provider is not assigned.", this);
            return false;
        }
    }
}
