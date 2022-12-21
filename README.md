![Creature Creator Logo](./Logo.png)

<p align="center"><b><a href="https://www.youtube.com/watch?v=FgXHPeQhEXo">Watch trailer</a> | <a href="https://bit.ly/creature-creator-demo">Try demo (itch.io)</a> | <a href="https://store.steampowered.com/app/1990050/Creature_Creator">Download game (Steam)</a></b></p>

<p align="center">Create creatures to explore online worlds with friends in this creative multiplayer experience!</p>

<p align="center">The creator consists of three modes. Build mode lets you shape your creature's body and attach customizable parts to it. Each part contributes to your overall statistics and may include special abilities! Paint mode lets you color your creature's body and attached parts, and change the pattern of your creature's skin. Finally, play mode lets you bring your creation to life!</p>

---


## Getting Involved

[![Discord](https://img.shields.io/discord/648800197702320137?logo=discord&style=flat)](https://discord.com/invite/CpugBB4r7W)
[![YouTube](https://img.shields.io/youtube/channel/subscribers/UCGLR3v7NaV1t92dnzWZNSKA?logo=youtube&style=flat&label=subscribe)](https://www.youtube.com/channel/UCGLR3v7NaV1t92dnzWZNSKA?sub_confirmation=1)
[![Twitter](https://img.shields.io/twitter/follow/daniellochner?logo=twitter&style=flat&label=follow)](https://twitter.com/daniellochner)

Join the community Discord server and follow on YouTube and Twitter to stay in the loop. Once you're a member, there are a load of different ways to contribute to this open source project, regardless of your expertise!

### Player

Submitting bug reports or feedback and suggestions is a great way to get involved without any game-development experience. To do so, create an [issue](https://github.com/daniellochner/creature-creator-game/issues) in this GitHub repository. That way, you'll be able to track the progress of your submission and follow it to release on Steam!

### Artist

As you may have noticed, some of the body parts in the editor were created by members of the community! You can design these parts in any 3D modeling software, and then submit them on the Discord server. A developer will then import your models into the game.

### Developer

For those interested in becoming developers, there are few steps required before you can load the project in Unity:

  1. Create a fork and clone this repository to your local hard drive using GitHub desktop.
  2. Drag-and-drop the paid-for assets into the "Assets/Plugins/External/Paid/" directory. Yes, this project unfortunately uses several paid-for assets from the Unity Asset Store which cannot be freely included in this public repository. If you can confirm that you have purchased all the assets listed [here](https://assetstore.unity.com/lists/list-280315), then an existing developer will be able to send you a zip file of this directory.
  3. Start Unity Hub, and open the project's root folder.
  4. Wait for the necessary packages to import (this may take a while).
  5. While you're waiting, sign up for Unity's Gaming Services, which will be necessary to test the multiplayer.
  6. Log in to your account in the Unity editor.
  7. Go to "Edit > Project Settings > Services" and then create a Unity Project ID and link it to the project.
  8. Go to "Window > Startup" and drag the window into your editor. This force enters the startup scene on play.
  9. Finally, due to a bug in Unity's Netcode for GameObjects library, testing in the Unity editor may require the use of the "Network Prefab Fix" window, located in "Window > Networking". All network objects must be included in this list.

Contributions are then managed through pull requests, which I'll first need to confirm before they're applied to the main branch.



## License
![GitHub](https://img.shields.io/github/license/daniellochner/creature-creator-game?logo=github&style=flat)

Creature Creator and related products by Daniel Lochner have been open-sourced under the [GNU General Public License v3.0](./LICENSE.md), however, contributions made by members of the community are usable at the discretion of their author(s).

> Permissions of this strong copyleft license are conditioned on making available complete source code of licensed works and modifications, which include larger works using a licensed work, under the same license. Copyright and license notices must be preserved. Contributors provide an express grant of patent rights.



## Support
![GitHub Sponsors](https://img.shields.io/github/sponsors/daniellochner?logo=github&style=flat)

By purchasing the game on Steam, you are already supporting! If you'd like to further support the development of this project though, then you can donate through [Ko-Fi](https://ko-fi.com/daniellochner)!



## Disclaimer
This project is not affiliated, associated, authorized, endorsed by, or in any way officially connected to Spore, EA, or any of its subsidiaries or affiliates.
