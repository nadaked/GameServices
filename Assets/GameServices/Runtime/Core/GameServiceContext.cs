using UnityEngine;

namespace GameServices.Runtime.Core
{
    public sealed class GameServiceContext
    {
        public GameServiceContext(bool useMockServices = false, bool verboseLogging = true)
        {
            UseMockServices = useMockServices;
            VerboseLogging = verboseLogging;
            IsEditor = Application.isEditor;
            Platform = Application.platform;
        }

        public bool UseMockServices { get; }
        public bool VerboseLogging { get; }
        public bool IsEditor { get; }
        public RuntimePlatform Platform { get; }
    }
}
