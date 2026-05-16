# GameServices

GameServices is a Unity service architecture sample built around ScriptableObject factories, a ScriptableObject provider, and small typed service contracts.

The goal is to keep game code away from concrete SDKs such as AdMob, Firebase, IAP, audio implementations, save backends, or scene loading details. Gameplay code asks for a capability like `IAdsService`, `IAudioService`, `ISaveService`, or `ISceneLoaderService`; the configured factories decide which runtime implementation is created.

## Why This Exists

Unity projects can quickly collect SDK calls inside scene scripts, managers, UI controllers, and gameplay code. That makes platform differences, mock data, disabled services, and SDK swaps harder than they need to be.

This project explores a cleaner pattern:

- ScriptableObject assets configure which services are active.
- Factories create runtime service instances.
- A provider ScriptableObject exposes services to game code.
- Bootstrapper exists only to initialize the provider from the main scene.
- Mock and null services make editor testing possible without real SDKs.

## Current Services

- Ads
  - `IAdsService`
  - `MockAdsService`
  - `NullAdsService`
- Audio
  - `IAudioService`
  - `MockAudioService`
  - `UnityAudioService`
  - `NullAudioService`
- Supports master, music, and SFX volume channels
- Supports optional AudioMixerGroup routing for master, music, and SFX outputs
- Scene Loading
  - `ISceneLoaderService`
  - `MockSceneLoaderService`
  - `UnitySceneLoaderService`
  - `NullSceneLoaderService`
- Save
  - `ISaveService`
  - `MockSaveService`
  - `PlayerPrefsSaveService`
  - `NullSaveService`
  - Supports primitive values, JSON data, and common Unity values such as `Vector2`, `Vector3`, `Quaternion`, and `Color`

The same pattern can be extended for AdMob, Firebase, Unity IAP, analytics, remote config, save systems, localization, or any other game-level service.

## Architecture

```text
Main Scene
  GameServicesBootstrapper
    -> GameServicesConfig
    -> GameServicesProvider
      -> GameServicesManager
        -> IGameService implementations
```

### Core Roles

`GameServicesBootstrapper`

Runs in the main scene and initializes the configured provider. It is not a singleton and does not use `DontDestroyOnLoad`; after initialization, the ScriptableObject provider is the access point.

`GameServicesConfig`

ScriptableObject asset that stores the list of enabled service factories and shared startup options.

`GameServiceFactory`

Base ScriptableObject factory for creating runtime service instances.

`GameServicesProvider`

ScriptableObject runtime access point. Game code can reference this asset and request typed services.

`GameServicesManager`

Plain C# registry that initializes services and resolves them by interface or service id.

`IGameService`

Base contract for all services.

## Project Structure

This repository is structured so it can be used as a base Unity project:

```text
Assets/
  _Project/
    Audio/
      Music/
      Sfx/
    Editor/
      Toolbar/
    Prefabs/
    Scenes/
      GameServicesDemo.unity
    Scripts/
    Services/
      GameServicesProvider.asset
      GameServicesConfig.asset
      Factories/

  GameServices/
    Runtime/
      Ads/
      Audio/
      Core/
      Save/
      SceneLoading/
    Samples/
      Demo/
        Scripts/
```

`Assets/GameServices/Runtime` contains the reusable framework code. Keep this folder clean from project-specific assets.

`Assets/_Project` contains the current project's scenes, service configuration assets, audio clips, prefabs, and game scripts. When starting a new project from this repository, this is the main area to rename or customize.

`Assets/_Project/Editor/Toolbar` contains project-level Unity Editor toolbar helpers for opening Project Settings and changing `Time.timeScale` from the main toolbar.

`Assets/GameServices/Samples` contains sample-only scripts that demonstrate how to call the services.

## Demo Setup

Recommended scene hierarchy:

```text
GameServicesDemo
  GameServices Bootstrapper
    - GameServicesBootstrapper

  Demo Controller
    - GameServicesDemoController
```

Create or assign these ScriptableObject assets:

```text
Assets/_Project/Services/GameServicesProvider.asset
Assets/_Project/Services/GameServicesConfig.asset
Assets/_Project/Services/Factories/MockAdsServiceFactory.asset
Assets/_Project/Services/Factories/MockAudioServiceFactory.asset
Assets/_Project/Services/Factories/MockSceneLoaderServiceFactory.asset
Assets/_Project/Services/Factories/PlayerPrefsSaveServiceFactory.asset
```

Then add the factories to `GameServicesConfig`:

```text
MockAdsServiceFactory
MockAudioServiceFactory
MockSceneLoaderServiceFactory
PlayerPrefsSaveServiceFactory
```

Assign the same `GameServicesProvider` asset to both `GameServicesBootstrapper` and `GameServicesDemoController`.

The demo controller exposes context menu actions:

- Show Rewarded
- Show Interstitial
- Play Music
- Play Sfx
- Load Scene
- Save Demo Value
- Load Demo Value
- Save Demo Position
- Load Demo Position
- Save Demo Json
- Load Demo Json

To use the real Unity audio adapter, create:

```text
Assets/Create/Game Services/Audio/Unity Audio
```

Then assign music and SFX clips with stable ids. Replace `MockAudioServiceFactory` with `UnityAudioServiceFactory` in `GameServicesConfig`.

To use the real Unity scene loader adapter, create:

```text
Assets/Create/Game Services/Scene Loading/Unity Scene Loader
```

Then replace `MockSceneLoaderServiceFactory` with `UnitySceneLoaderServiceFactory` in `GameServicesConfig`. Scenes loaded by name must be added to Unity Build Settings.

## Example Usage

```csharp
using GameServices.GameServices.Runtime.Ads;
using GameServices.GameServices.Runtime.Core;
using UnityEngine;

public sealed class RewardButton : MonoBehaviour
{
    [SerializeField] private GameServicesProvider provider;

    public async void ShowRewarded()
    {
        var ads = provider.Get<IAdsService>();
        if (ads == null || !ads.IsRewardedReady)
        {
            return;
        }

        var result = await ads.ShowRewardedAsync("rewarded_default");
        Debug.Log($"Reward result: {result}");
    }
}
```

Saving JSON data:

```csharp
[System.Serializable]
public sealed class PlayerProgress
{
    public int level;
    public int coins;
    public string selectedTheme;
}

var save = provider.Get<ISaveService>();
save.SetJson("player.progress", new PlayerProgress
{
    level = 12,
    coins = 450,
    selectedTheme = "forest"
});

await save.SaveAsync();

var progress = save.GetJson("player.progress", new PlayerProgress());
```

## Coding Style

- Serialized private fields do not use an underscore prefix.
- Non-serialized private fields use `_camelCase`.
- `private readonly` fields use `_camelCase`.
- Public properties and methods use `PascalCase`.
- Runtime service classes should stay as plain C# as much as possible.
- Unity lifecycle code should stay near bootstrapper or integration boundaries.

## Roadmap Ideas

- AdMob adapter service
- Firebase adapter service
- Unity IAP adapter service
- JSON file save service
- Parallel and ordered initialization modes
- Service dependency declarations
- Runtime status UI sample
- Editor tooling for validating service configs

## Third-Party Credits

This project uses and references a few helpful community tools:

- [adammyhre/Unity-Utils](https://github.com/adammyhre/Unity-Utils) by Adam Myhre, added through Unity Package Manager as `com.gitamend.unityutils`. The package is MIT licensed.
- [Unity 6.3 Custom Main Toolbar gist](https://gist.github.com/NicolasChicunque/c2512380b1732d50e75fac4574a44b26) by Nicolas Chicunque, forked from Adam Myhre's toolbar gist. The scripts are placed under `Assets/_Project/Editor/Toolbar`.

## Unity Version

Created with Unity `6000.3.12f1`.

## License

This project is open source under the MIT License. See [LICENSE](LICENSE) for details.


