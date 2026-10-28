# Mobile Strike: Tap Shooter — Unity Android Starter

This folder contains a Unity source starter for a modern military first-person mobile shooter. It is source code, not a compiled APK, and has not yet been tested in the Unity Editor.

## Create the project
1. Install Unity Hub: https://unity.com/download
2. Create a **3D (Built-In Render Pipeline)** project named `MobileStrikeTapShooter`.
3. In Unity's Project window, create `Assets/MobileStrike/Scripts`.
4. For the first prototype, copy only these six files into that folder:
   - `MobileStrikePrototypeBootstrap.cs`
   - `MobileStrikePlayerController.cs`
   - `MobileStrikeTouchPad.cs`
   - `MobileStrikePlayerHealth.cs`
   - `MobileStrikeWeapon.cs`
   - `MobileStrikeCrosshair.cs`
5. **Do not copy `DuelNetworkHealth.cs` or `DuelRoundManager.cs) yet.** Those two files require the Netcode for GameObjects package; we'll add them later when setting up online PVP.
6. Create an empty GameObject in the scene named `PrototypeBootstrap`, add `MobileStrikePrototypeBootstrap.cs`, and press Play.

## Runtime prototype
The bootstrap script creates a simple 3D training yard, cover blocks, player camera, rifle and pistol placeholders, target dummies, main menu, HUD, crosshair, touch movement/look pads, and fire/reload/switch buttons at runtime. No scene asset needs to be hand-authored for the first test.

## First test
1. Add the bootstrap component to an empty GameObject.
2. Press Play, then select **START TRAINING**.
3. Use WASD to move in the Editor; hold the right mouse button and move the mouse to look. On a touchscreen build, use the left MOVE pad, drag the right side to look, and tap FIRE / RELOAD / SWITCH.
4. This is a graybox prototype: replace primitive cubes/capsules with licensed 3D models and add animations, audio, and visual effects before publishing.

## Android build
In Unity Hub install **Android Build Support**, **Android SDK & NDK Tools**, and **OpenJDK**. In Unity, switch the build platform to Android and build an APK for device testing. Google Play distribution normally requires a signed Android App Bundle (.aab), store artwork, policy declarations, and compliance with current Play Console requirements.

## Multiplayer status — not yet complete
The included Netcode scripts are not a finished online game. A real 1v1 PVP build still needs Netcode for GameObjects, NetworkManager setup, NetworkObject player prefabs, transport/lobby configuration, server-validated hit detection, spawn points, reconnect/leave handling, and two-device testing.
