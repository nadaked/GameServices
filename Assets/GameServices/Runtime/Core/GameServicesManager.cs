using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace GameServices.Runtime.Core
{
    public sealed class GameServicesManager
    {
        private readonly Dictionary<Type, IGameService> _servicesByContract = new();
        private readonly Dictionary<string, IGameService> _servicesById = new(StringComparer.OrdinalIgnoreCase);

        public IReadOnlyCollection<IGameService> Services => _servicesById.Values;

        public async Task InitializeAsync(GameServicesConfig config, GameServiceContext context)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            foreach (var factory in config.Factories)
            {
                if (factory == null || !factory.EnabledService)
                {
                    continue;
                }

                var service = factory.Create(context);
                if (service == null)
                {
                    Debug.LogWarning($"Game service factory '{factory.name}' returned null.");
                    continue;
                }

                Register(service);
                await service.InitializeAsync(context);
            }
        }

        public bool TryGet<TService>(out TService service) where TService : class, IGameService
        {
            if (_servicesByContract.TryGetValue(typeof(TService), out var found))
            {
                service = found as TService;
                return service != null;
            }

            service = null;
            return false;
        }

        public TService Get<TService>() where TService : class, IGameService
        {
            return TryGet<TService>(out var service) ? service : null;
        }

        public IGameService GetById(string serviceId)
        {
            return serviceId != null && _servicesById.TryGetValue(serviceId, out var service)
                ? service
                : null;
        }

        private void Register(IGameService service)
        {
            _servicesById[service.ServiceId] = service;
            _servicesByContract[service.GetType()] = service;

            foreach (var contract in service.GetType().GetInterfaces())
            {
                if (typeof(IGameService).IsAssignableFrom(contract))
                {
                    _servicesByContract[contract] = service;
                }
            }
        }
    }
}
