using GameServices.GameServices.Runtime.Core;
using UnityEngine;

namespace GameServices.GameServices.Runtime.SceneLoading
{
    [CreateAssetMenu(menuName = "Game Services/Scene Loading/Mock Scene Loader", fileName = "MockSceneLoaderServiceFactory")]
    public sealed class MockSceneLoaderServiceFactory : GameServiceFactory
    {
        [SerializeField] private string initialSceneName = "Demo";
        [SerializeField] private bool logCalls = true;

        public override string ServiceId => "scene-loader.mock";

        public override IGameService Create(GameServiceContext context)
        {
            return new MockSceneLoaderService(initialSceneName, logCalls && context.VerboseLogging);
        }
    }
}


