using System.Collections.Generic;
using UnityEngine;

namespace GameServices.Runtime.Core
{
    [CreateAssetMenu(menuName = "Game Services/Config", fileName = "GameServicesConfig")]
    public sealed class GameServicesConfig : ScriptableObject
    {
        [SerializeField] private List<GameServiceFactory> factories = new();
        [SerializeField] private bool useMockServicesInEditor = true;
        [SerializeField] private bool verboseLogging = true;

        public IReadOnlyList<GameServiceFactory> Factories => factories;
        public bool UseMockServicesInEditor => useMockServicesInEditor;
        public bool VerboseLogging => verboseLogging;
    }
}
