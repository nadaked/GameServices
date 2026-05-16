using System.Threading.Tasks;
using GameServices.Runtime.Core;

namespace GameServices.Runtime.SceneLoading
{
    public interface ISceneLoaderService : IGameService
    {
        string ActiveSceneName { get; }

        Task LoadSceneAsync(string sceneName);
        Task ReloadActiveSceneAsync();
    }
}
