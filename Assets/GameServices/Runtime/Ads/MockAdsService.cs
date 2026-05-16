using System;
using System.Threading.Tasks;
using GameServices.Runtime.Core;
using UnityEngine;

namespace GameServices.Runtime.Ads
{
    public sealed class MockAdsService : IAdsService
    {
        private readonly bool _rewardedReady;
        private readonly bool _interstitialReady;
        private readonly AdRewardResult _rewardedResult;
        private readonly float _showDelaySeconds;
        private GameServiceStatus _status = GameServiceStatus.NotInitialized;

        public MockAdsService(
            bool rewardedReady,
            bool interstitialReady,
            AdRewardResult rewardedResult,
            float showDelaySeconds)
        {
            _rewardedReady = rewardedReady;
            _interstitialReady = interstitialReady;
            _rewardedResult = rewardedResult;
            _showDelaySeconds = Mathf.Max(0f, showDelaySeconds);
        }

        public string ServiceId => "ads.mock";
        public GameServiceStatus Status => _status;
        public bool IsReady => _status == GameServiceStatus.Ready;
        public bool IsRewardedReady => IsReady && _rewardedReady;
        public bool IsInterstitialReady => IsReady && _interstitialReady;

        public Task InitializeAsync(GameServiceContext context)
        {
            _status = GameServiceStatus.Ready;

            if (context.VerboseLogging)
            {
                Debug.Log("Mock ads service initialized.");
            }

            return Task.CompletedTask;
        }

        public async Task<AdRewardResult> ShowRewardedAsync(string placementId)
        {
            if (!IsRewardedReady)
            {
                return AdRewardResult.NotAvailable;
            }

            await DelayIfNeeded();
            Debug.Log($"Mock rewarded ad completed for placement '{placementId}'. Result: {_rewardedResult}");
            return _rewardedResult;
        }

        public async Task ShowInterstitialAsync(string placementId)
        {
            if (!IsInterstitialReady)
            {
                return;
            }

            await DelayIfNeeded();
            Debug.Log($"Mock interstitial ad shown for placement '{placementId}'.");
        }

        private Task DelayIfNeeded()
        {
            return _showDelaySeconds <= 0f
                ? Task.CompletedTask
                : Task.Delay(TimeSpan.FromSeconds(_showDelaySeconds));
        }
    }
}
