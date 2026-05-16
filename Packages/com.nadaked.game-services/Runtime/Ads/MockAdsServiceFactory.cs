using GameServices.GameServices.Runtime.Core;
using UnityEngine;

namespace GameServices.GameServices.Runtime.Ads
{
    [CreateAssetMenu(menuName = "Game Services/Ads/Mock Ads", fileName = "MockAdsServiceFactory")]
    public sealed class MockAdsServiceFactory : GameServiceFactory
    {
        [SerializeField] private bool rewardedReady = true;
        [SerializeField] private bool interstitialReady = true;
        [SerializeField] private AdRewardResult rewardedResult = AdRewardResult.Completed;
        [SerializeField, Min(0f)] private float showDelaySeconds = 0.5f;

        public override string ServiceId => "ads.mock";

        public override IGameService Create(GameServiceContext context)
        {
            return new MockAdsService(
                rewardedReady,
                interstitialReady,
                rewardedResult,
                showDelaySeconds);
        }
    }
}


