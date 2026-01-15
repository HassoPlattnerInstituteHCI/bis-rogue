# DualPanto-Rogue

Unity-Version: **2021.3.0f1**

Rogue GamePlay Online:
- https://archive.org/details/RogueTheAdventureGameV1.11984MichaelC.ToyKennethC.R.C.ArnoldAdventureRolePlayingRPG#loading


Idea for BIS:


___
#### Rogue Gameplay

GameObjects inside the hirachy window:
- Panto
- Map
- Manager


1. add player and control via keyboard 
    - Place an capsule/sphere inside the scene (right click on the hirachy window and rename to Player)
    - attach PlayerController.cs to Player (ToDo for Students???)
    - test if player moves when arrow key pressed
2. create empty room inside map
    - map already has a mesh collider (needed for room collision)
    - use a 3d cube and place it so that the player is inside of the cube
3. add collison handling for player and map 
    - remove PlayerController.cs; 
    - attach PlayerControllerCollision.cs to Player (ToDo for Students???)
4. add food and collect with player
    - place a capsule in the hirachy window and rename to Food
    - add a Collider to the Food item (capsule colliider)
    - create a new Tag with name: "Food". (click on Food GameObject and inside the inspector on the right side there is a Tag option -> below the Name of Game Object)
    - now we create a new script for the player. Go to the Player and inside the inspector press "Add Component". Write "Player" and hit "New Srcipt"
    - Script (write onCollision)
5. add enemy and movement of enemy
    - same as food (new Tag: "Enemy")
    - we want to follow the player (DumbEnemy.cs; ToDo Students)
6. crate Enemy and Food Prefab
7. write random 
8. add random map 
9. dark room (nice to have)

___
#### Game Sounds

1. sound (application with a blind scene -> just audio output) -> different branche
2. add walking sound to handle
3. add soundmanager to game

8. room speech on entry -> SpeechIO???

___
#### DualPanto Haptics

1. add unity handle to game -> move to position, move around with unity handle
2. add player recoil when player collides with enemy



Extra todo:
- move plane to 0,0