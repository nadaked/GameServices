using System;
using System.Threading.Tasks;
using GameServices.GameServices.Runtime.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameServices.GameServices.Runtime.SceneLoading
{
    public sealed class UnitySceneLoaderService : ISceneLoaderService
    {
        private readonly LoadSceneMode _loadSceneMode;
        private readonly bool _setLoadedSceneActive;
        private readonly bool _logWarnings;
        private GameServiceStatus _status = GameServiceStatus.NotInitialized;

        public UnitySceneLoaderService(
            LoadSceneMode loadSceneMode,
            bool setLoadedSceneActive,
            bool logWarnings)
        {
            _loadSceneMode = loadSceneMode;
            _setLoadedSceneActive = setLoadedSceneActive;
            _logWarnings = logWarnings;
        }

        public string ServiceId => "scene-loader.unity";
        public GameServiceStatus Status => _status;
        public bool IsReady => _status == GameServiceStatus.Ready;
        public string ActiveSceneName => SceneManager.GetActiveScene().name;

        public Task InitializeAsync(GameServiceContext context)
        {
            _status = GameServiceStatus.Ready;
            return Task.CompletedTask;
        }

        public async Task LoadSceneAsync(string sceneName)
        {
            if (string.IsNullOrWhiteSpace(sceneName))
            {
                LogWarning("Scene name is empty.");
                return;
            }

            var operation = SceneManager.LoadSceneAsync(sceneName, _loadSceneMode);
            if (operation == null)
            {
                LogWarning($"Could not start loading scene '{sceneName}'. Make sure it is added to Build Settings.");
                return;
            }

            await WaitForOperationAsync(operation);

            if (_setLoadedSceneActive)
            {
                var loadedScene = SceneManager.GetSceneByName(sceneName);
                if (loadedScene.IsValid() && loadedScene.isLoaded)
                {
                    SceneManager.SetActiveScene(loadedScene);
                }
            }
        }

        public Task ReloadActiveSceneAsync()
        {
            return LoadSceneAsync(ActiveSceneName);
        }

        private static Task WaitForOperationAsync(AsyncOperation operation)
        {
            var completionSource = new TaskCompletionSource<bool>();

            operation.completed += _ => completionSource.TrySetResult(true);

            if (operation.isDone)
            {
                completionSource.TrySetResult(true);
            }

            return completionSource.Task;
        }

        private void LogWarning(string message)
        {
            if (_logWarnings)
            {
                Debug.LogWarning(message);
            }
        }
    }
}


