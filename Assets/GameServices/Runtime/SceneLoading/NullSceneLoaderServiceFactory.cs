using GameServices.Runtime.Core;
using UnityEngine;

namespace GameServices.Runtime.SceneLoading
{
    [CreateAssetMenu(menuName = "Game Services/Scene Loading/Null Scene Loader", fileName = "NullSceneLoaderServiceFactory")]
    public sealed class NullSceneLoaderServiceFactory : GameServiceFactory
    {
        public override string ServiceId => "scene-loader.null";

        public override IGameService Create(GameServiceContext context)
        {
            return new NullSceneLoaderService();
        }
    }
}
