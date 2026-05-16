using System.Threading.Tasks;
using GameServices.GameServices.Runtime.Core;
using UnityEngine;

namespace GameServices.GameServices.Runtime.SceneLoading
{
    public sealed class MockSceneLoaderService : ISceneLoaderService
    {
        private readonly string _initialSceneName;
        private readonly bool _logCalls;
        private GameServiceStatus _status = GameServiceStatus.NotInitialized;
        private string _activeSceneName;

        public MockSceneLoaderService(string initialSceneName, bool logCalls)
        {
            _initialSceneName = initialSceneName;
            _logCalls = logCalls;
        }

        public string ServiceId => "scene-loader.mock";
        public GameServiceStatus Status => _status;
        public bool IsReady => _status == GameServiceStatus.Ready;
        public string ActiveSceneName => _activeSceneName;

        public Task InitializeAsync(GameServiceContext context)
        {
            _activeSceneName = string.IsNullOrWhiteSpace(_initialSceneName)
                ? "MockScene"
                : _initialSceneName;

            _status = GameServiceStatus.Ready;
            Log($"Mock scene loader initialized. Active scene: {_activeSceneName}");
            return Task.CompletedTask;
        }

        public Task LoadSceneAsync(string sceneName)
        {
            _activeSceneName = sceneName;
            Log($"Mock scene loaded: {_activeSceneName}");
            return Task.CompletedTask;
        }

        public Task ReloadActiveSceneAsync()
        {
            Log($"Mock scene reloaded: {_activeSceneName}");
            return Task.CompletedTask;
        }

        private void Log(string message)
        {
            if (_logCalls)
            {
                Debug.Log(message);
            }
        }
    }
}


