using UnityEngine;

namespace GameServices.GameServices.Runtime.Core
{
    public abstract class GameServiceFactory : ScriptableObject
    {
        [SerializeField] private bool enabledService = true;

        public bool EnabledService => enabledService;
        public abstract string ServiceId { get; }

        public abstract IGameService Create(GameServiceContext context);
    }
}


