# DualPanto-Rogue

Unity Version: **2021.3.0f1**

![Rogue for DualPanto](docs/img/cover.png)

## Resources

- [Rogue Gameplay Online](https://archive.org/details/RogueTheAdventureGameV1.11984MichaelC.ToyKennethC.R.C.ArnoldAdventureRolePlayingRPG#loading)
- [DualPanto Toolkit](https://github.com/HassoPlattnerInstituteHCI/unity-dualpanto-toolkit/) - Installation and Documentation

___

## Rogue Gameplay 🎮 

### Step-by-Step Tutorial

#### Overview
- [DualPanto-Rogue](#dualpanto-rogue)
  - [Resources](#resources)
  - [Rogue Gameplay 🎮](#rogue-gameplay-)
    - [Step-by-Step Tutorial](#step-by-step-tutorial)
      - [Overview](#overview)
      - [1. Add Player and Keyboard Controls](#1-add-player-and-keyboard-controls)
      - [2. Add Collectible Food Items](#2-add-collectible-food-items)
      - [3. Create Room](#3-create-room)
      - [4. Add Enemy with Movement](#4-add-enemy-with-movement)
      - [5. Create Prefabs and Random Spawning](#5-create-prefabs-and-random-spawning)
      - [6. Add Procedural Map Generation](#6-add-procedural-map-generation)
      - [(extra) Apply material to GameObjects 🎨](#extra-apply-material-to-gameobjects-)

#### 1. Add Player and Keyboard Controls
  - **1.1** Create a capsule in the scene (right-click in the Hierarchy window → 3D Object → Capsule/Sphere)
    <img src="docs/img/1_newGO.png" height="600">
  - **1.2** Rename the GameObject to "Player"
    <img src="docs/img/1_rename.png" height="600">
  - **1.3** Change size of the GameObject in the Inspector (click on GameObject → right window → change values inside scale)
    <img src="docs/img/1_inspector.png" height="600">
  - **1.4** Attach the `PlayerController.cs` script to the Player GameObject **(ToDo)** (click on the GameObject → scroll down the Inspector window → click add component)
    <img src="docs/img/1_addComponent.png" height="600"><img src="docs/img/1_addComponent2.png" height="400">
  - **1.5** TODO: add more movement in the `PlayerController.cs` script
  - **1.6** Test movement using the arrow keys (press play on center top)
    <img src="docs/img/1_play.png" height="600">

#### 2. Add Collectible Food Items
  - **2.1** Create a capsule in the Hierarchy and rename it to "Food"
  - **2.2** Add a Capsule Collider component to the Food GameObject
  - **2.3** Create a new Tag called "Food":
  - **2.3.1** Select the Food GameObject
    - **2.3.2** In the Inspector, click on the Tag dropdown (below the GameObject name)
      <img src="docs/img/2_tag1.png" height="600">
    - **2.3.3** Select "Add Tag" and create "Food"
      <img src="docs/img/2_tag2.png" height="600">
      <img src="docs/img/2_tag3.png" height="600">
      <img src="docs/img/2_tag4.png" height="100">
    - **2.3.4** Assign this tag to the Food GameObject
      <img src="docs/img/2_tag5.png" height="600">
  - **2.4** In the Inspector, go to the capsule collider and select **isTrigger**
    <img src="docs/img/2_trigger1.png" height="600">
  - **2.5** Add a Rigidbody to the Player GameObject and select isKinematic
    <img src="docs/img/2_trigger2.png" height="300">
    <img src="docs/img/2_trigger3.png" height="600">
  - **2.6** TODO: Edit the `PlayerSimple.cs` script to handle collisions 

#### 3. Create Room
  - **3.1** The Map GameObject already includes a Mesh Collider (required for collision detection)
  - **3.2** Create a 3D Cube inside the Map GameObject (right-click on Map GameObject in Hierarchy → 3D Object → Cube)
  - **3.3** Scale and position the cube to form a room that contains the Player
  - **3.4** Remove the `PlayerController.cs` script from the Player
  - **3.5** Attach the `PlayerControllerCollision.cs` script to the Player GameObject
  - **3.6** This enables proper collision detection between Player and Map
    <img src="docs/img/3_rooms.png" height="600">

#### 4. Add Enemy with Movement
  - **4.1** Create an enemy GameObject similar to the Food item
  - **4.2** Create and assign a new Tag called "Enemy"
  - **4.3** Attach the `EnemyAttack.cs` and `EnemyMovement.cs` script  
    <img src="docs/img/4_enemy.png" height="600">
  - **4.4** TODO: edit the `EnemyMovement.cs` script and make the enemy follow the Player
    

#### 5. Create Prefabs and Random Spawning
  - **5.1** Attach the `RoomSpawnRandom.cs` script to the Map GameObject
  - **5.2** Create multiple rooms (they should either overlap or be connected by corridors)
    <img src="docs/img/5_more_rooms.png" height="600">
  - **5.3** Create and assign the Tag "Room" to each room where you want spawning to occur
  - **5.4** Create Prefabs:
    - **5.4.1** Navigate to the Prefabs folder in the Project window
    - **5.4.2** Drag the Enemy and Food GameObjects from the Hierarchy into the Prefabs folder
      <img src="docs/img/5_prefab1.png" height="600">
    - **5.4.3** The objects should turn blue, indicating they are now prefab instances
      <img src="docs/img/5_prefab2.png" height="600">
    - **5.4.4** Delete the Enemy and Food instances from the Scene
    - **5.4.5** Select the Map GameObject and locate the `RoomSpawnRandom.cs` component
    - **5.4.6** Drag the Enemy prefab into the "Enemy Prefab" field and drag the Food prefab into the "Food Prefab" field
      <img src="docs/img/5_prefab3.png" height="600">
  - **5.5** TODO: edit the `RoomSpawnRandom.cs` script to spawn random food objects

#### 6. Add Procedural Map Generation
  - **6.1** Remove all manually placed rooms inside the Map
    <img src="docs/img/6_removeRooms.png" height="600">
  - **6.2** Add the `GridRoomSpawner.cs` script to the Map GameObject
    <img src="docs/img/6_addRoomGen.png" height="600">
  - **6.3** Configure the spawner in the Inspector:
    - **6.3.1** Select the **Room Prefab** (drag from Prefabs folder or use the object picker)
    - **6.3.2** Select the **Corridor Prefab** (drag from Prefabs folder or use the object picker)
    - **6.3.3** Adjust **Rows** and **Columns** for grid size (e.g., 3x3)
    - **6.3.4** Set **Min/Max Room Size** (controls room dimensions within each cell)
    - **6.3.5** Adjust **Probability of Room in Cell** (percentage chance a cell contains a room)
  - **6.4** Press **Play** to generate a random dungeon layout with connected rooms


___

#### (extra) Apply material to GameObjects 🎨
- click on the GameObject you want add material
- go to MeshRenderer → Materials → +
- search for "green", ("yellow", "red")
  <img src="docs/img/extra_material_1.png" height="600">
  <img src="docs/img/extra_material_2.png" height="400">

___


  
