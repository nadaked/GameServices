using GameServices.Runtime.Core;
using UnityEngine;

namespace GameServices.Runtime.Audio
{
    [CreateAssetMenu(menuName = "Game Services/Audio/Mock Audio", fileName = "MockAudioServiceFactory")]
    public sealed class MockAudioServiceFactory : GameServiceFactory
    {
        [SerializeField, Range(0f, 1f)] private float musicVolume = 1f;
        [SerializeField, Range(0f, 1f)] private float sfxVolume = 1f;
        [SerializeField] private bool logCalls = true;

        public override string ServiceId => "audio.mock";

        public override IGameService Create(GameServiceContext context)
        {
            return new MockAudioService(musicVolume, sfxVolume, logCalls && context.VerboseLogging);
        }
    }
}
