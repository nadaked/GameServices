using GameServices.Runtime.Core;
using UnityEngine;

namespace GameServices.Runtime.Audio
{
    [CreateAssetMenu(menuName = "Game Services/Audio/Null Audio", fileName = "NullAudioServiceFactory")]
    public sealed class NullAudioServiceFactory : GameServiceFactory
    {
        public override string ServiceId => "audio.null";

        public override IGameService Create(GameServiceContext context)
        {
            return new NullAudioService();
        }
    }
}
