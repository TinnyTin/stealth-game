# Alpha Submission Readme
## Team: Tiny Brain
## Game: Kleptocrat

## Start Scene
### Global Scene ###
The game will not be visible in the editor with just GlobalScene loaded. MainMenu and Level1 will be loaded additively at runtime. 

## How to Play
The game can be played with a keyboard or mouse. 

### Keyboard/Mouse Controls
* W,A,S,D to move the player
* Space to crouch
* Left click to pick up stealable item
* Mouse move to rotate camera around player up axis

### Joypad Controls
Tested with Xbox One controller
* Left stick to move player 
  * Player movement speed is determined by stick angle
* Y to crouch
* A to pick up stealable item
* Right stick to rotate camera around player up axis

### Gameplay Loop
At the beginning of the level the camera will be focused on the extraction point. It will then do a flythrough of the level to show the player the location of the item they need to collect. The player must sneak through the level, collect the item and then proceed to the extraction point. 

But be careful, there are AI guards patrolling the area. They will become suspicious if they see you in a secure area, and will chase you if they identify you as a threat. To make matters worse, the feckless citizens have left their trash scattered around the town and the corporate tycoons have fired all the street cleaners. Kicking the detritus that litters the area can draw attention to your position, so tread carefully. 

If you are caught by the guards it's game over for you. Keep trying until you've successfully absconded with that shiny new gramophone that your lady wife wants so badly. No doubt you'll be tasked with stealing records for it in the near future, that new Jiles Gravis album is hot property. 

### Observable Technology Requirements
* Menu at the start of the game allows you to enter the level or exit the game
* Menu when the player succeeds or is caught allow you to restart the level or exit the game
* The player character showcases root motion animation, player control via keyboard/mouse and joypad and camera control
* AI Guards showcase root motion animation, NPC steering, AI behaviours and states
* Level detritus is modelled with rigid body physics objects that can be moved by the player to create cover, and can be knocked over by both the player and guards
* Background music and ambient audio are implemented as 2D audio sources
* Footsteps and knocked items emit one shot sounds as 3D audio sources

### Known Problem Areas
The alpha showcases the majority of our technical requirements that are required to build the final game. Due to the short time frame of the alpha the level is small and the gameplay loop is short. However, with the features implemented so far the game will scale up during the next development phase. 

Some problems that may be observed during gameplay:
* Animations are still a work in progress, some issues such as akward poses and buggy feet movement are to be expected
* The camera can clip through walls

## Manifest
Who did what, organised by team member names in alphabetical order. Assets that aren't listed here below aren't relevant to what can be seen in the alpha. 

### Erik
Erik’s main contributions were the player and camera control systems. Algorithmic contributions include:
* Input system supporting game controller and keyboard input for third-person perspective control of the player. The player can walk or crouch-walk to navigate on the nav-mesh. The camera system is an interpolated follow-cam, which allows the player to naturally move using either the keyboard or game controller with dual analog sticks.
* Animated introduction panning camera script
* System for highlighting the target “stealable” object and extraction point based on player proximity.

Erik also contributed to asset creation, including:
* A test scene to prototype with the chosen low poly asset pack
* Animation for player and some civilian NPCs
* Audio for footsteps

#### Assets Implemented
**Audio:**
* Footsteps

**Models and Animations:**
* Agent animations (civilian clips and animation controllers)
* Player animations (walk, run, crouch)
* Player Animation Controller

**Scenes:**
* Victorian Test Scene

**Prefabs:**
* Player
* Extraction Point
* Stealable Object
* Civilians

#### Scripts Implemented
* CameraAnimPlayer.cs
* ExtractionPoint.cs
* FollowCamera.cs
* FootstepEmitter.cs
* PlayerControl.cs
* PlayerInput.cs
* SceneCameraController.cs
* StealableObject.cs

#### 3rd party credits
* Footstep sounds: https://freesound.org/people/marb7e/packs/34204/
* "Steal Object" chime: https://freesound.org/people/Samulis/sounds/192636/
* "Reach extraction point" chime: https://freesound.org/people/Anthousai/sounds/398496/

### Jeesoo
Jeesoo’s main contributions were level development and UI scenes.
* Designed and built level 1 tutorial level using assets from the low poly asset pack
* Incorporated camera animations for pan scene
* Created sound emitting objects that contribute to the AI threat meter
* Created designs for main menu flow and end states

#### Assets Implemented
**Audio:**
* Background music
* Sounds
* Crate collision
* Glass collision
* Scraping wood

**UI:**
* Title image
* Backdrop images
* Font files
* Game Flow UI designs

**Animations:**
* Level1IntroAnimation

**Scenes:**
* Level1
* MainMenu

**Prefabs:**
* Crate
* Glass Bottle

#### Scripts Implemented
* SoundEmittingObject.cs
* SceneCameraController.cs

#### 3rd party credits
* Poly Steampunk Pack by Polyperfect: https://assetstore.unity.com/packages/3d/props/poly-steampunk-pack-265079

### Justin
Justin’s contributions were heavily in the AI system. Algorithm contributions include:
* Hierarchical FSM system for AI behavior states
* Audio sensory through sound listener based on listening radius
* FOV Sight sensory using physics radius, view angle & non-blocked raycasting from AI head to player 
* FOV conic view visualisation through dynamic mesh renderer
* 2D root-motion blended movement integrated into the navigational mesh, with waypoint navigation
* Threat-meter based behavior switching system, and UI displaying threat bar
* AI Manager creating events in Scriptable Object (highest elevated AI hostile state, AI requests player caught)

Other notable contributions:
* Enhancement on Erik’s footstep emitter- now the randomized collection-based footsteps can be chosen based on passed-in ground surface + movement type (not yet fully utilized in the alpha)

#### Assets Implemented
**Controller:**
* AI_guard.controller

**Animation:**
* From milestone 1:
  * Walkback.fbx
  * Walkbackandturn.fbx
  * Walkforward.fbx
  * Runs.fbx
  * Idles.fbx
  * Y_Bot@left_turn_90.fbx
  * Y_Bot@right_turn_90.fbx
* From Mixamo.com:
  * Nervously Look Around.fbx
  * Surprised.fbx
  * Terrified.fbx
  * Reacting.fbx

**Audio:**
* Fail-trumpet-sound-effect.mp3 https://www.youtube.com/watch?v=z8Jn3qnPOGg
* Male-gasp.mp3 https://www.youtube.com/watch?v=g9xXaDyi5_E

**Prefab:**
* AI_Guard.prefab
* MovableCrouchCover.prefab
* PathDebugger.prefab

**ScriptableObjects:**
* AIGlobalData.asset
* AIHighestStateChange.asset
* AIRequestPlayerCaught.asset
* DirtWalk.asset
* DirtRun.asset
* FootstepEmitter.asset
* FailSound.asset

**Scenes:**
* ai-algorithm (test, not used in alpha)

#### Scripts Implemented
* AIStateMachine.cs
* AIStateFactory.cs
* AIBaseState.cs
* AIInvestigateState.cs
* AIPursuitState.cs
* AIWaypointState.cs
* AIAudioListener.cs
* CanvasLookatCamera.cs
* PathDebugger.cs
* ThreatMeter.cs
* Waypoint.cs
* FootStepFactory.cs
* FootStepCollection.cs
* AIData.cs
* PlayerCaughtListener.cs
* AIThreatChangeEventListener.cs

#### 3rd party credits
* Heirarchical FSM - https://www.youtube.com/watch?v=kV06GiJgFhc&t=1535s
* FOV visualisation - https://www.youtube.com/watch?v=rQG9aUWarw
* Probuilder rapid prototyping - https://unity.com/features/probuilder

### Martin
Worked on end-user game flow, all UI logic and animations, and assistance with merging game systems together.
* Authored MainMenu scene, logic and animations.
* Mission Summary screen (exit, restart, return to menus) and associated camera logic.
* Minimap, ObjectiveTracker, and Alert status for the HUD.
* Loading screen implementation.
* Scripts for the user's success and game-over conditions.
* Ensuring controllers can navigate all menus in the game.
* Ability and text to skip the intro cutscene.
Piecing together navigation flow for the user from start to end of experience.

#### Assets Implemented

**Animations and Controllers:**
* All files under ModelsAndAnimations\UI

**Scenes:**
* MainMenu
* Level1 (Contributor)

**Prefabs:**
* Minimap Camera.prefab
* HUD Canvas.prefab
* Mission Summary Canvas.prefab

#### Scripts Implemented
* Most can be found under Scripts/UI/*
* AlertStatus.cs
* ChangeMissionText.cs
* GameQuitter.cs
* LimitCamera.cs
* MainMenuTransition.cs
* MainMenuCamera.cs
* MissionSummary.cs
* ObjectiveTracker.cs

#### 3rd party credits
* Milestone assignment for the GameQuitter

### Tom
Primarily worked on systems to support the development of the game. These systems included:
* AudioManager: Handles instantiating and playing 2D and 3D audio sources
* SceneController: Handles loading, unloading, and transitioning scenes at runtime
* Event System: Allows events to be raised via a GameEvent scriptable oject anywhere in the codebase and for the events to be responded to by parametised listener MonoBehaviors. 

Implemented Global Scene, which is the persistent scene into which all others are additively loaded at runtime. Global Scene contains manager scripts that are required to persist across scenes, this prevents wasted effort duplicating reused managers into each scene. 

Also added the ambient audio that can be heard throughout the level. 

#### Assets Implemented
* Assets\Audio\city_ambience
* Assets\Prefabs\AmbientSound2D
* Assets\Prefabs\Audio Manager
* Assets\Prefabs\Audio Test Capsule
* Assets\Prefabs\EventSound2D
* Assets\Prefabs\EventSound3d
* Assets\Scenes\Global Scene
* Assets\SO Instances\Events\OneShotAudio2D
* Assets\SO Instances\Events\OneShotAudio3D
* Assets\SO Instances\Events\OneShotAudio3DWithString
* Assets\SO Instances\Events\SceneLoadComplete

#### Scripts Implemented
* Assets\Scripts\Audio\AmbientAudioTest.cs
* Assets\Scripts\Audio\AmbientSound2D.cs
* Assets\Scripts\Audio\AudioSourceParams.cs
* Assets\Scripts\Audio\AudioTestCapsule.cs
* Assets\Scripts\Audio\EventSound2D.cs
* Assets\Scripts\Audio\EventSound3D.cs
* Assets\Scripts\Events\GameEventListener.cs
* Assets\Scripts\Events\GameEventListener1.cs
* Assets\Scripts\Events\GameEventListener2.cs
* Assets\Scripts\Events\GameEventListener3.cs
* Assets\Scripts\Events\GameEventListenerBase.cs
* Assets\Scripts\Events\I1ParamEventListener.cs
* Assets\Scripts\Events\I2ParamEventListener.cs
* Assets\Scripts\Events\I3ParamEventListener.cs
* Assets\Scripts\Events\IEventListener.cs
* Assets\Scripts\Events\OneShotAudio2DEventListener.cs
* Assets\Scripts\Events\OneShotAudio3DEventListener.cs
* Assets\Scripts\Events\OneShotAudio3DEventListenerWithString.cs
* Assets\Scripts\Events\SceneLoadCompletedListener.cs
* Assets\Scripts\Managers\AudioManager.cs
* Assets\Scripts\Managers\GameManager.cs
* Assets\Scripts\Managers\SceneController.cs
* Assets\Scripts\ScriptableObjects\Events\GameEvent.cs
* Assets\Scripts\ScriptableObjects\ScriptableObjectWithInit.cs

#### 3rd party credits
* Hierarchy Folders Package: https://github.com/xsduan/unity-hierarchy-folders 
* Eflatun.SceneReference Package: https://github.com/starikcetin/Eflatun.SceneReference
* City Ambience Audio: https://freesound.org/people/klankbeeld/sounds/279041/

