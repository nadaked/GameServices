using GameServices.GameServices.Runtime.Core;
using UnityEngine;

namespace GameServices.GameServices.Runtime.Save
{
    [CreateAssetMenu(menuName = "Game Services/Save/Null Save", fileName = "NullSaveServiceFactory")]
    public sealed class NullSaveServiceFactory : GameServiceFactory
    {
        public override string ServiceId => "save.null";

        public override IGameService Create(GameServiceContext context)
        {
            return new NullSaveService();
        }
    }
}


