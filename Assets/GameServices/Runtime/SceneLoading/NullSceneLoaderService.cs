using System.Threading.Tasks;
using GameServices.Runtime.Core;

namespace GameServices.Runtime.SceneLoading
{
    public sealed class NullSceneLoaderService : ISceneLoaderService
    {
        public string ServiceId => "scene-loader.null";
        public GameServiceStatus Status => GameServiceStatus.Disabled;
        public bool IsReady => true;
        public string ActiveSceneName => string.Empty;

        public Task InitializeAsync(GameServiceContext context)
        {
            return Task.CompletedTask;
        }

        public Task LoadSceneAsync(string sceneName)
        {
            return Task.CompletedTask;
        }

        public Task ReloadActiveSceneAsync()
        {
            return Task.CompletedTask;
        }
    }
}
