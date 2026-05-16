using GameServices.Runtime.Core;
using UnityEngine;

namespace GameServices.Runtime.Ads
{
    [CreateAssetMenu(menuName = "Game Services/Ads/Null Ads", fileName = "NullAdsServiceFactory")]
    public sealed class NullAdsServiceFactory : GameServiceFactory
    {
        public override string ServiceId => "ads.null";

        public override IGameService Create(GameServiceContext context)
        {
            return new NullAdsService();
        }
    }
}
