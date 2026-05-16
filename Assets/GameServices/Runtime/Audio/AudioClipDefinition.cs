using System;
using UnityEngine;

namespace GameServices.GameServices.Runtime.Audio
{
    [Serializable]
    public sealed class AudioClipDefinition
    {
        [SerializeField] private string id;
        [SerializeField] private AudioClip clip;

        public string Id => id;
        public AudioClip Clip => clip;
    }
}

