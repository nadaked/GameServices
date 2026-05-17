# Game Services

ScriptableObject factory based service architecture for Unity projects.

## Install From Git

Use Unity Package Manager:

```text
https://github.com/nadaked/GameServices.git?path=Packages/com.nadaked.game-services
```

For a tagged version:

```text
https://github.com/nadaked/GameServices.git?path=Packages/com.nadaked.game-services#v0.1.1
```

## Included Services

- Ads contracts with mock/null implementations
- Audio contracts with mock/null/Unity implementations, including pitch-controlled pooled SFX playback
- Save service with PlayerPrefs, JSON, Unity value types, mock, and null implementations
- Scene loading service with mock/null/Unity implementations
- ScriptableObject config, provider, factory, and bootstrapper core

## Audio Example

`UnityAudioServiceFactory` exposes `sfxSourcePoolSize`, `minSfxPitch`, and `maxSfxPitch` so overlapping SFX can play with independent pitch values.

```csharp
var audio = provider.Get<IAudioService>();
await audio.PlaySfxAsync("combo_note", pitch: 1.122f, volumeScale: 0.9f);
```

## Samples

Import `Game Services Demo` from the Unity Package Manager Samples panel. The sample includes:

- Demo scene
- Demo controller
- GameServicesProvider asset
- GameServicesConfig asset
- Mock Ads, Mock Audio, Mock Scene Loader, and PlayerPrefs Save factories

## Namespaces

Runtime namespaces follow the package path:

```text
GameServices.GameServices.Runtime.Core
GameServices.GameServices.Runtime.Audio
GameServices.GameServices.Runtime.Save
GameServices.GameServices.Runtime.SceneLoading
GameServices.GameServices.Runtime.Ads
```
