# Mobile Strike: Tap Shooter — Unity Android Starter

This folder contains a **Unity source starter** for a modern military first-person mobile shooter. It is source code, not a compiled APK, and has not yet been tested in the Unity Editor.

## Create the project
1. Install Unity Hub: https://unity.com/download
2. Create a **3D (Built-in Render Pipeline)** project named `MobileStrikeTapShooter`.
3. In Unity's Project window, create `Assets/MobileStrike/Scripts`.
4. Download/copy every `.cs` file from `MobileStrikeUnity/Assets/MobileStrike/Scripts` in this GitHub folder into that directory.
5. Create an empty GameObject in the scene named `PrototypeBootstrap`, add `MobileStrikePrototypeBootstrap.cs`, and press Play.

## Runtime prototype
The bootstrap script creates a simple 3D training yard, cover blocks, player camera, rifle placeholder, target dummies, main menu, HUD, crosshair, touch movement/look pads, and fire/reload/switch buttons at runtime. No scene asset needs to be hand-authored for the first test.

## Included scripts
- `MobileStrikePrototypeBootstrap.cs`: creates the prototype map, player, target dummies, menu and HUD.
- `MobileStrikePlayerController.cs`: desktop keyboard/mouse and mobile touch movement/look.
- `MobileStrikeTouchPad.cs`: touch input pads.
- `MobileStrikePlayerHealth.cs`: health and death events.
- `MobileStrikeWeapon.cs`: tap-to-fire raycast, ammo, reload, weapon switching, muzzle-flash hook and recoil.
- `MobileStrikeCrosshair.cs`: reusable dynamic crosshair component.
- `DuelNetworkHealth.cs` and `DuelRoundManager.cs`: initial Netcode duel-health/result foundation only.

## First test
1. Add the bootstrap component to an empty GameObject.
2. Press Play, then select **START TRAINING**.
3. Use WASD to move in the Editor; hold the right mouse button and move the mouse to look. On a touchscreen build, use the left MOVE pad, drag the right side to look, and tap FIRE / RELOAD / SWITCH.
4. This is a graybox prototype: replace primitive cubes/capsules with licensed 3D models and add animations, audio, and visual effects before publishing.

## Android build
In Unity Hub install **Android Build Support**, **Android SDK & NDK Tools**, and **OpenJDK**. In Unity, switch the build platform to Android and build an APK for device testing. Google Play distribution normally requires a signed Android App Bundle (.aab), store artwork, policy declarations, and compliance with current Play Console requirements.

## Multiplayer status — not yet complete
The included Netcode scripts are not a finished online game. A real 1v1 PVP build still needs NetworkManager setup, NetworkObject player prefabs, transport/lobby configuration, server-validated hit detection, spawn points, reconnect/leave handling, and two-device testing. Do not advertise online matchmaking until those are implemented and tested.
