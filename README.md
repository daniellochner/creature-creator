![Creature Creator Logo](./Logo.png)

Create creatures to explore online worlds with friends in this creative multiplayer experience!

---

## Steam:

This game is available to download on [Steam](https://store.steampowered.com/app/1990050/Creature_Creator/).


## Development Instructions:

  1. Download the Unity project from this repository.
  2. Start Unity Hub, and click "Open" in the top right corner.
  3. Locate and open the project's root folder (containing the "Assets" folder).
  4. Wait for the necessary packages to import (may take up to 5 minutes).

Since our game uses Unity's Gaming Services (https://unity.com/solutions/gaming-services), additional steps
are required to build and play the game:

  5. Sign up for Gaming Services using your Unity account.
  6. Log in to your account in the Unity editor
  7. Go to Edit -> Project Settings -> Services
  8. Create a Unity Project ID (https://docs.unity3d.com/Manual/SettingUpProjectServices.html) and link it to the project.
  9. Go to Window -> Startup and drag the window into your editor. This force enters the startup scene on play.

NOTE: Due to a bug in Unity's Netcode for GameObjects library (https://github.com/Unity-Technologies/com.unity.netcode.gameobjects), testing in the Unity editor may require the use of the "Network Prefab Fix" window, located in Window -> Networking. All network objects must be included in this list.

## External libraries:

All code which we did not implement ourselves can be found in the "Assets/Plugins/External/" folder.
This contains assets downloaded from the Unity Asset Store (https://assetstore.unity.com/).
