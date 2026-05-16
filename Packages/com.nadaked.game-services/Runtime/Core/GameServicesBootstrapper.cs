using System;
using System.Threading.Tasks;
using UnityEngine;

namespace GameServices.GameServices.Runtime.Core
{
    [DefaultExecutionOrder(-1000)]
    public sealed class GameServicesBootstrapper : MonoBehaviour
    {
        [SerializeField] private GameServicesConfig config;
        [SerializeField] private GameServicesProvider provider;
        [SerializeField] private bool initializeOnAwake = true;

        public GameServicesProvider Provider => provider;

        private async void Awake()
        {
            if (initializeOnAwake)
            {
                await InitializeAsync();
            }
        }

        public async Task InitializeAsync()
        {
            if (config == null)
            {
                Debug.LogError("Game services config is not assigned.", this);
                return;
            }

            if (provider == null)
            {
                Debug.LogError("Game services provider is not assigned.", this);
                return;
            }

            try
            {
                var useMockServices = config.UseMockServicesInEditor && Application.isEditor;
                var context = new GameServiceContext(useMockServices, config.VerboseLogging);

                await provider.InitializeAsync(config, context);

                if (config.VerboseLogging)
                {
                    Debug.Log("Game services initialized.", this);
                }
            }
            catch (Exception exception)
            {
                Debug.LogException(exception, this);
                provider.ResetRuntimeState();
            }
        }
    }
}


