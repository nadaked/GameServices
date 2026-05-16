using System.Threading.Tasks;
using GameServices.Runtime.Core;

namespace GameServices.Runtime.Ads
{
    public interface IAdsService : IGameService
    {
        bool IsRewardedReady { get; }
        bool IsInterstitialReady { get; }

        Task<AdRewardResult> ShowRewardedAsync(string placementId);
        Task ShowInterstitialAsync(string placementId);
    }
}
