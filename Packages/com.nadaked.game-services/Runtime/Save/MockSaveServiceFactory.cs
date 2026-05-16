using GameServices.GameServices.Runtime.Core;
using UnityEngine;

namespace GameServices.GameServices.Runtime.Save
{
    [CreateAssetMenu(menuName = "Game Services/Save/Mock Save", fileName = "MockSaveServiceFactory")]
    public sealed class MockSaveServiceFactory : GameServiceFactory
    {
        [SerializeField] private bool logCalls = true;

        public override string ServiceId => "save.mock";

        public override IGameService Create(GameServiceContext context)
        {
            return new MockSaveService(logCalls && context.VerboseLogging);
        }
    }
}


