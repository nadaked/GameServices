using GameServices.Runtime.Ads;
using GameServices.Runtime.Audio;
using GameServices.Runtime.Core;
using GameServices.Runtime.Save;
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
        [SerializeField] private string saveKey = "demo.coins";
        [SerializeField] private int saveValue = 100;
        [SerializeField] private string positionSaveKey = "demo.player_position";
        [SerializeField] private Vector3 positionSaveValue = new(1f, 2f, 3f);
        [SerializeField] private string jsonSaveKey = "demo.player_profile";

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

        [ContextMenu("Save Demo Value")]
        public async void SaveDemoValue()
        {
            if (!HasProvider())
            {
                return;
            }

            var save = provider.Get<ISaveService>();
            if (save == null)
            {
                Debug.LogWarning("Save service is not registered.", this);
                return;
            }

            save.SetInt(saveKey, saveValue);
            await save.SaveAsync();
            Debug.Log($"Saved demo value: {saveKey} = {saveValue}", this);
        }

        [ContextMenu("Load Demo Value")]
        public void LoadDemoValue()
        {
            if (!HasProvider())
            {
                return;
            }

            var save = provider.Get<ISaveService>();
            if (save == null)
            {
                Debug.LogWarning("Save service is not registered.", this);
                return;
            }

            var value = save.GetInt(saveKey, -1);
            Debug.Log($"Loaded demo value: {saveKey} = {value}", this);
        }

        [ContextMenu("Save Demo Position")]
        public async void SaveDemoPosition()
        {
            if (!HasProvider())
            {
                return;
            }

            var save = provider.Get<ISaveService>();
            if (save == null)
            {
                Debug.LogWarning("Save service is not registered.", this);
                return;
            }

            save.SetVector3(positionSaveKey, positionSaveValue);
            await save.SaveAsync();
            Debug.Log($"Saved demo position: {positionSaveKey} = {positionSaveValue}", this);
        }

        [ContextMenu("Load Demo Position")]
        public void LoadDemoPosition()
        {
            if (!HasProvider())
            {
                return;
            }

            var save = provider.Get<ISaveService>();
            if (save == null)
            {
                Debug.LogWarning("Save service is not registered.", this);
                return;
            }

            var value = save.GetVector3(positionSaveKey, Vector3.zero);
            Debug.Log($"Loaded demo position: {positionSaveKey} = {value}", this);
        }

        [ContextMenu("Save Demo Json")]
        public async void SaveDemoJson()
        {
            if (!HasProvider())
            {
                return;
            }

            var save = provider.Get<ISaveService>();
            if (save == null)
            {
                Debug.LogWarning("Save service is not registered.", this);
                return;
            }

            var profile = new DemoPlayerProfile
            {
                playerName = "Demo Player",
                level = 3,
                coins = saveValue,
                lastPosition = positionSaveValue
            };

            save.SetJson(jsonSaveKey, profile);
            await save.SaveAsync();
            Debug.Log($"Saved demo json: {jsonSaveKey}", this);
        }

        [ContextMenu("Load Demo Json")]
        public void LoadDemoJson()
        {
            if (!HasProvider())
            {
                return;
            }

            var save = provider.Get<ISaveService>();
            if (save == null)
            {
                Debug.LogWarning("Save service is not registered.", this);
                return;
            }

            var profile = save.GetJson(jsonSaveKey, new DemoPlayerProfile());
            Debug.Log($"Loaded demo json: {profile.playerName}, level {profile.level}, coins {profile.coins}, position {profile.lastPosition}", this);
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

        [System.Serializable]
        private sealed class DemoPlayerProfile
        {
            public string playerName;
            public int level;
            public int coins;
            public Vector3 lastPosition;
        }
    }
}
