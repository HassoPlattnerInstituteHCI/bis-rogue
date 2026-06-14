# DualPanto-Rogue
Unity Version: **6000.4.6f1**

![Rogue for DualPanto](docs/img/cover.png)


## Resources

- [Rogue Gameplay Online](https://archive.org/details/RogueTheAdventureGameV1.11984MichaelC.ToyKennethC.R.C.ArnoldAdventureRolePlayingRPG#loading)
- [DualPanto Toolkit](https://github.com/HassoPlattnerInstituteHCI/unity-dualpanto-toolkit/) - Installation and Documentation

___

## Getting Started

> **💡 Tip:**
> When pressing Play, Unity shows a brown screen. Press **"b"** on your keyboard to make the scene visible.


## Rogue Sounds 📣

### Step-by-Step Tutorial

#### 1. Add ItemPickupSound to SoundManager
   - Select the Manager in the Scene and go to SoundManager. There you will find a List of Sounds.
     - Press `+` and give the sound a name (e.g. "ItemPickupSound", "HealSound").
     - Add a sound clip to the item
      <img src="docs/img/SoundTask1.png" height="600">
  - Now we need to play the sound when a player picks up an item.
     - **TODO**: go to `PlayerSimple.cs` and edit the TODO.

#### 2. Text-To-Speech
   - Select the Manager in the Scene and go to RogueGameManager. 
    - Enter a text (introductionText) you want to hear when starting the game
      <img src="docs/img/SoundTask2.png" height="600">
     - **TODO**: go to `RogueGameManager.cs` and edit the TODO.

#### 3. AI Speech Audio Clip
- go to ... and create an audio introduction clip
- import the sound into Unity by dropping it inside the Assets view. 
- Select the manager in the scene again and add the sound in the RogueGameManager.
  <img src="docs/img/SoundTask3.png" height="600">
  