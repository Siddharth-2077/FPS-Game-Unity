# FPS-System V2.0
Custom FPS System developed in **Unity**, using the Standard Render Pipeline.


## Documentation

The System uses five main C# scripts to implement the FPS System.

 - *PlayerMovement.cs* <br /> 
 - *FpsControllerLPFP.cs* <br /> 
 - *AutomaticGunScriptLPFP.cs* <br /> 
 - *Weapon.cs* <br /> 
 - *WeaponHandler.cs* <br /> 

To have a quick look at the above scripts, you can view the contents of the 
folder called '*Scipt Files - Quick View*'.

- ### The 5-Level hierarchy of the FPS system

  ![FPS_Player_Hierarchy](https://user-images.githubusercontent.com/67199656/200593510-5a139e97-dd14-4a63-94bc-1fe34fd1367f.png)
  
  **Level-1:** The root-component "*FPS Player*": <br />
  Must consist of *Rigidbody*, *Character Controller*, and the custom *FpsControllerLPFP.cs* script. <br />
  
  **Level-2:** Under the "*FPS Player*" game object: <br />
  The Player-UI and HUD can be placed here.
  Place an empty game object as the "*Main Rig*" (you can call it anything you like).
  
  **Level-3:** Under the "*Main Rig*" game object: <br />
  Place an empty game object as the "*Weapons Rig*" (you can call it anything you like).
  
  **Level-4:** Under the "*Weapons Rig*" game object: <br />
  This level consist of the Gun-Camera, and another empty game object called "*Weapons*".
  
  **Level-5:** Under the "*Weapons*" game object: <br />
  This level contains all of the weapon prefabs that the player may possess in the game.
  The "*Weapons*" game object must have the *WeaponHandler.cs* component attached to it. The weapons that shall be placed under this
  game object must contain the *Weapon.cs* component attached to them, to mark them as a weapon.
  
  Each weapon under the "*Weapons*" game object contains the *AutomaticGunScriptLPFP.cs* component attached to it, if the weapon happens to be a gun.

## Screenshots

![SS1](https://user-images.githubusercontent.com/67199656/200614045-4ae5a8d8-425a-4d3d-88cc-364c9ea8b539.png)


![SS3](https://user-images.githubusercontent.com/67199656/200615927-3eb86501-9288-47d3-9770-7c5466069712.png)


## Gameplay & Controls

The game uses standard first-person shooter contols such as <br />
- *WASD* for Movement <br />
- *Mouse* to Look Around <br />
- *Spacebar* to Jump, and <br />
- *LeftShift* to Sprint <br />

## Author
**Siddharth M** <br />
MIT Manipal, India <br />


    
