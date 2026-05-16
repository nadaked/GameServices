using GameServices.Runtime.Core;
using UnityEngine;
using UnityEngine.Audio;

namespace GameServices.Runtime.Audio
{
    [CreateAssetMenu(menuName = "Game Services/Audio/Unity Audio", fileName = "UnityAudioServiceFactory")]
    public sealed class UnityAudioServiceFactory : GameServiceFactory
    {
        [SerializeField] private AudioClipDefinition[] musicClips;
        [SerializeField] private AudioClipDefinition[] sfxClips;
        [SerializeField, Range(0f, 1f)] private float masterVolume = 1f;
        [SerializeField, Range(0f, 1f)] private float musicVolume = 1f;
        [SerializeField, Range(0f, 1f)] private float sfxVolume = 1f;
        [SerializeField] private bool loopMusic = true;
        [SerializeField] private bool persistAcrossScenes = true;
        [SerializeField] private bool logWarnings = true;
        [SerializeField] private AudioMixerGroup masterOutput;
        [SerializeField] private AudioMixerGroup musicOutput;
        [SerializeField] private AudioMixerGroup sfxOutput;

        public override string ServiceId => "audio.unity";

        public override IGameService Create(GameServiceContext context)
        {
            return new UnityAudioService(
                musicClips,
                sfxClips,
                masterVolume,
                musicVolume,
                sfxVolume,
                loopMusic,
                persistAcrossScenes,
                logWarnings && context.VerboseLogging,
                masterOutput,
                musicOutput,
                sfxOutput);
        }
    }
}
