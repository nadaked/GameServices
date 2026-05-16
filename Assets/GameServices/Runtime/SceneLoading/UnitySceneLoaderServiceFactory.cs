using GameServices.GameServices.Runtime.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameServices.GameServices.Runtime.SceneLoading
{
    [CreateAssetMenu(menuName = "Game Services/Scene Loading/Unity Scene Loader", fileName = "UnitySceneLoaderServiceFactory")]
    public sealed class UnitySceneLoaderServiceFactory : GameServiceFactory
    {
        [SerializeField] private LoadSceneMode loadSceneMode = LoadSceneMode.Single;
        [SerializeField] private bool setLoadedSceneActive = true;
        [SerializeField] private bool logWarnings = true;

        public override string ServiceId => "scene-loader.unity";

        public override IGameService Create(GameServiceContext context)
        {
            return new UnitySceneLoaderService(
                loadSceneMode,
                setLoadedSceneActive,
                logWarnings && context.VerboseLogging);
        }
    }
}


