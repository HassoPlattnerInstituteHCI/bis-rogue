# DualPanto-Rogue
Unity Version: **6000.3.17f1**

![Rogue for DualPanto](docs/img/cover.png)


## Resources

- [Rogue Gameplay Online](https://archive.org/details/RogueTheAdventureGameV1.11984MichaelC.ToyKennethC.R.C.ArnoldAdventureRolePlayingRPG#loading)
- [DualPanto Toolkit](https://github.com/HassoPlattnerInstituteHCI/unity-dualpanto-toolkit/) - Installation and Documentation

___

## Getting Started

> **💡 Tip:**
> When pressing Play, Unity shows the screen. Press **"b"** on your keyboard to make the scene invisible.


## Rogue Sounds 📣

### Step-by-Step Tutorial

#### 0. Check
- Make sure you have run `git submodule update --init --recursive` on this repository.

#### 1. Add ItemPickupSound to SoundManager
   - Select the Manager in the Scene and find the SoundManager component. There you will find a list of sounds.
     - Press `+` and give the sound a name (e.g. "ItemPickupSound", "HealSound").
     - Add a sound clip to the entry.
      <img src="docs/img/SoundTask1.png" height="600">
  - Now we need to play the sound when a player picks up an item.
     - **TODO**: go to `PlayerSimple.cs` and complete the TODO.

#### 2. Text-To-Speech
   - Select the Manager in the Scene and find the RogueGameManager component.
   - Enter the text (introductionText) you want to hear when the game starts.
      <img src="docs/img/SoundTask2.png" height="600">
     - **TODO**: go to `RogueGameManager.cs` and complete the TODO.

#### 3. TTS for every room
   - Every room has a `RoomSpeechOnEntry` component, which speaks its `introductionText` (via TTS) when the player enters it.
   - **TODO**: go to `GridRoomSpawner.cs` and uncomment the line that sets `introductionText = $"Room {roomData.id}"` on each spawned room.
   - Press Play and walk into a room. You should hear "Room \<id\>" announced when you enter.
 
#### 4. Speech Audio Clip
- Go to [luvvoice.com](https://luvvoice.com), or use your phone, to create an audio introduction clip (mp3 or wav).
- Import the sound into Unity by dragging it into the Assets view.
- Add the clip to the SoundManager and give it a name.
  <img src="docs/img/SoundTask1.png" height="600">
- Select the Manager in the scene again and enter the clip's name in the RogueGameManager.
 <img src="docs/img/SoundTask4.png" height="200">
  

