using System.Threading.Tasks;
using GameServices.GameServices.Runtime.Core;

namespace GameServices.GameServices.Runtime.SceneLoading
{
    public interface ISceneLoaderService : IGameService
    {
        string ActiveSceneName { get; }

        Task LoadSceneAsync(string sceneName);
        Task ReloadActiveSceneAsync();
    }
}


