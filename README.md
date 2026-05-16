# GameServices

GameServices is a Unity service architecture sample built around ScriptableObject factories, a ScriptableObject provider, and small typed service contracts.

The goal is to keep game code away from concrete SDKs such as AdMob, Firebase, IAP, audio implementations, or scene loading details. Gameplay code asks for a capability like `IAdsService`, `IAudioService`, or `ISceneLoaderService`; the configured factories decide which runtime implementation is created.

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
  - `NullAudioService`
- Scene Loading
  - `ISceneLoaderService`
  - `MockSceneLoaderService`
  - `NullSceneLoaderService`

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

## Demo Setup

The demo assets live under:

```text
Assets/GameServices/Samples/Demo
```

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
GameServicesProvider.asset
GameServicesConfig.asset
MockAdsServiceFactory.asset
MockAudioServiceFactory.asset
MockSceneLoaderServiceFactory.asset
```

Then add the factories to `GameServicesConfig`:

```text
MockAdsServiceFactory
MockAudioServiceFactory
MockSceneLoaderServiceFactory
```

Assign the same `GameServicesProvider` asset to both `GameServicesBootstrapper` and `GameServicesDemoController`.

The demo controller exposes context menu actions:

- Show Rewarded
- Show Interstitial
- Play Music
- Play Sfx
- Load Scene

## Example Usage

```csharp
using GameServices.Runtime.Ads;
using GameServices.Runtime.Core;
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
- Parallel and ordered initialization modes
- Service dependency declarations
- Runtime status UI sample
- Editor tooling for validating service configs

## Unity Version

Created with Unity `6000.3.12f1`.

