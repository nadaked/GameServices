using System.Threading.Tasks;
using GameServices.Runtime.Core;

namespace GameServices.Runtime.Ads
{
    public sealed class NullAdsService : IAdsService
    {
        public string ServiceId => "ads.null";
        public GameServiceStatus Status => GameServiceStatus.Disabled;
        public bool IsReady => true;
        public bool IsRewardedReady => false;
        public bool IsInterstitialReady => false;

        public Task InitializeAsync(GameServiceContext context)
        {
            return Task.CompletedTask;
        }

        public Task<AdRewardResult> ShowRewardedAsync(string placementId)
        {
            return Task.FromResult(AdRewardResult.NotAvailable);
        }

        public Task ShowInterstitialAsync(string placementId)
        {
            return Task.CompletedTask;
        }
    }
}
