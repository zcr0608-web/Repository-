# Mobile Strike: Tap Shooter — Unity Android Starter

This folder is a **Unity source starter**, not a compiled APK. It is separate from the existing browser tower-defense project.

## Create the Unity project
1. Install Unity Hub from https://unity.com/download
2. Create a **3D (URP)** project named `MobileStrikeTapShooter`.
3. In `Assets`, create `MobileStrike/Scripts` and copy the C# files from this repository folder into it.
4. For Android builds, install **Android Build Support**, **Android SDK & NDK Tools**, and **OpenJDK** through Unity Hub.
5. Open Package Manager and install **Netcode for GameObjects** before using `DuelNetworkHealth.cs`.

## Starter scripts
- `MobileStrikePlayerHealth.cs`: player health and death event.
- `MobileStrikeWeapon.cs`: tap-to-fire raycast, ammo, reload, muzzle flash, camera recoil, weapon switching.
- `MobileStrikeCrosshair.cs`: four-part dynamic crosshair.
- `DuelNetworkHealth.cs`: server-authoritative network health and duel winner notification foundation.

## Scene setup (first playable prototype)
1. Add a Plane as the ground and some Cubes as cover.
2. Create a Player object with a CharacterController and a child Camera.
3. Add `MobileStrikePlayerHealth` to Player; add `MobileStrikeWeapon` to the camera or a weapon object, then assign the Camera and optional muzzle-flash ParticleSystem.
4. Add a Canvas (Screen Space - Overlay). Add a centered crosshair made of four small UI Images; attach `MobileStrikeCrosshair` to a parent and assign the four Images.
5. Create UI Buttons for **Fire**, **Reload**, and **Switch Weapon**. Wire their OnClick events to `MobileStrikeWeapon.ShootOnce`, `Reload`, and `SwitchWeapon`.
6. Test in the Editor, then switch Build Settings to Android and build an APK for device testing.

## Important
- The current scripts are starter components; they do not automatically create a complete scene, character models, animation, matchmaking, dedicated server, or a Play Store release.
- A real 1v1 online match still needs a NetworkManager, NetworkObject player prefab, transport/lobby configuration, spawn points, and a deployed or hosted session. Test with two devices/builds before publishing.
- Google Play publishing requires a signed Android App Bundle (.aab), store assets, policy declarations, and compliance with current Play Console requirements.
