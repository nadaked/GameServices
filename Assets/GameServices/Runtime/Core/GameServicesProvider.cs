using System.Threading.Tasks;
using UnityEngine;

namespace GameServices.Runtime.Core
{
    [CreateAssetMenu(menuName = "Game Services/Provider", fileName = "GameServicesProvider")]
    public sealed class GameServicesProvider : ScriptableObject
    {
        private GameServicesManager _services;
        private Task _initializationTask;

        public GameServicesManager Services => _services ??= new GameServicesManager();
        public bool IsInitialized { get; private set; }
        public bool IsInitializing => _initializationTask is { IsCompleted: false };

        public async Task InitializeAsync(GameServicesConfig config, GameServiceContext context)
        {
            if (IsInitializing)
            {
                await _initializationTask;
                return;
            }

            ResetRuntimeState();
            _initializationTask = InitializeInternalAsync(config, context);
            await _initializationTask;
        }

        public TService Get<TService>() where TService : class, IGameService
        {
            return Services.Get<TService>();
        }

        public bool TryGet<TService>(out TService service) where TService : class, IGameService
        {
            return Services.TryGet(out service);
        }

        public void ResetRuntimeState()
        {
            _services = new GameServicesManager();
            _initializationTask = null;
            IsInitialized = false;
        }

        private async Task InitializeInternalAsync(GameServicesConfig config, GameServiceContext context)
        {
            await Services.InitializeAsync(config, context);
            IsInitialized = true;
        }
    }
}
