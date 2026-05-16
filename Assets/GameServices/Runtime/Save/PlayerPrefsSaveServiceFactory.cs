using GameServices.Runtime.Core;
using UnityEngine;

namespace GameServices.Runtime.Save
{
    [CreateAssetMenu(menuName = "Game Services/Save/PlayerPrefs Save", fileName = "PlayerPrefsSaveServiceFactory")]
    public sealed class PlayerPrefsSaveServiceFactory : GameServiceFactory
    {
        [SerializeField] private string keyPrefix = "game_services.";

        public override string ServiceId => "save.player-prefs";

        public override IGameService Create(GameServiceContext context)
        {
            return new PlayerPrefsSaveService(keyPrefix);
        }
    }
}
