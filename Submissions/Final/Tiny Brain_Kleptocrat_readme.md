# Final Submission Readme/Manifest
## Team: Tiny Brain
## Game: Kleptocrat

## Starting Scene File
The game requires additive scene loading to work correctly, however you can load up just `Level1_Re` and the game will be playable, albeit with some sounds missing and only the 'Exit Game' option in the menus functional. 

To get the full game experience you need to have `Global Scene` loaded in the hierachy view in Unity. By default `Global Scene\Managers\Scene Controller` in the hierarchy view should have the `MainMenu` scene as the only element in the scene playlist. This setup will allow the game to function correctly from start to finish with all functionality present. 

## How to Play
The game can be played with a keyboard and mouse. Joypad support has been deprecated since the alpha. 

### Keyboard/Mouse Controls
* W,A,S,D to move the player
* C to crouch
* Shift to sprint
* Left click to pick up stealable item
* Mouse move to rotate camera to look around the player
* Escape to bring up pause menu

### Gameplay Loop
At the beginning of the level the camera will do a short pan to show the player the location of the item they need to steal. The player must sneak through the level, collect the item and then proceed to the extraction point. 

But be careful, there are AI guards patrolling the area. They will become suspicious if they see you in a secure area, and will chase you if they identify you as a threat. To make matters worse, the feckless citizens have left their trash scattered around the town and the corporate tycoons have fired all the street cleaners. Kicking the detritus that litters the area can draw attention to your position, so tread carefully. You can use your abilities to hide behind objects, sprint away from guards and even hide in trees and grassy areas. 

If you are caught by the guards it's game over for you. Keep trying until you've successfully absconded with that shiny new gramophone that your lady wife wants so badly. No doubt you'll be tasked with stealing records for it in the near future, that new Jiles Gravis album is hot property. 

### Observable Technology Requirements 
* Menu at the start of the game allows you to enter the level or exit the game
* Menu when the player succeeds or fails the mission allow you to restart the level or exit the game
* In game HUD shows the alert status of the AI guards, the mission objective, and a mini-map
* The player character showcases root motion animation and control via keyboard and mouse
* AI Guards showcase root motion animation, NPC steering, AI behaviours and states
* Level detritus is modelled with rigid body physics objects that can be moved by the player to create cover, and can be knocked over by both the player and guards
* Main menu music and some sound effects are implemented as 2D audio sources
* Footsteps, knocked items, ambient sound effects and in-level music tracks are one shot and looping 3D audio sources

### Known Problem Areas
The final build showcases the majority the features we hoped to include in the game. Unfortunately due to time constraints and the loss of a team member post-alpha we had to focus on polishing a single level and abandoned efforts to include a second playable level. 

The following is a list of problems that you may experience while playing the game:
* The camera may at times clip through certain walls
* Guards have been known to clip through gates and walls on rare occasions 

## Manifest
Who did what, organised by team member names in alphabetical order. 

### Erik
Erik’s main contributions were the player and camera control systems, "stealable object", and the extraction point.  Algorithmic contributions include:
* Input system supporting game controller and keyboard input for third-person perspective control of the player. The player can walk, crouch-walk, or sprint to navigate on the nav-mesh. The camera system is an interpolated follow-cam using Cinemachine, which allows the player to naturally move using the keyboard and mouse.
* Animated introduction panning camera script
* System for highlighting the target “stealable” object and extraction point based on player proximity.

Erik also contributed to asset creation, including:
* A test scene to prototype with the chosen low poly asset pack
* Animation controllers for player and some civilian NPCs
* Audio for footsteps and sprint stamina depletion

#### Assets Implemented
**Audio:**
* Footstep sounds: https://freesound.org/people/marb7e/packs/34204/
* "Out of breath" sound: https://freesound.org/people/saha213131/sounds/680766/
* "Steal Object" chime: https://freesound.org/people/Samulis/sounds/192636/
* "Reach extraction point" chime: https://freesound.org/people/Anthousai/sounds/398496/

**Models and Animations:**
* From Mixamo.com:
  * Agent animations (civilian clips and animation controllers)
  * Player animations (walk, run, crouch)
    * X_Bot@Standing.fbx
    * X_Bot@Running.fbx
    * X_Bot@Walking.fbx
    * X_Bot@Sneak_Walk.fbx
    * X_Bot@Crouching_Idle.fbx
  * Dance animations: https://assetstore.unity.com/packages/3d/animations/dance-animations-free-161313

**Scenes:**
* Victorian Test Scene
* contributed to Level1_Re scene

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

**Algorithm Contributions** 

* Authored Hierarchical FSM system for AI behavior states
* Audio sensory through sound listener based on listening radius
* FOV Sight sensory using physics radius, view angle & non-blocked raycasting from AI head to player 
* FOV conic view visualisation through dynamic mesh renderer
* 2D root-motion blended movement integrated into the navigational mesh, with waypoint navigation
* Threat-meter based behavior switching system, and UI displaying threat bar
* AI Manager creating events in Scriptable Object (highest elevated AI hostile state, AI requests player caught)

**Other Script contributions**

* Fixed player control bugs using Rigid body force/velocity instead of direct transform updates
* ScriptableObject communication for player/AI/gameplay
* Trigger scripts for doors, sounds, collapsing boxes and hiding zones
* Improved Erik’s footstep emitter- now the randomized collection-based footsteps can be chosen based on passed-in ground surface + movement type (not yet fully utilized in the alpha)

**Other contributions**
* Level whiteboxing and set decoration
* Trailer + Gameplay videos

**Scripts Implemented**

* Assets\Scripts\AI\ThreatMeter.cs
* Assets\Scripts\AI\Waypoint.cs
* Assets\Scripts\AI\Audio\AIAudioListener.cs
* Assets\Scripts\AI\Audio\FootstepEmitter.cs
* Assets\Scripts\AI\Debuggers\CanvasLookatCamera.cs
* Assets\Scripts\AI\Debuggers\PathDebugger.cs
* Assets\Scripts\AI\Debuggers\TemplateAIThreatChangeCallback.cs
* Assets\Scripts\AI\Debuggers\testSetAIActiveState.cs
* Assets\Scripts\AI\FOV\FieldOfView.cs
* Assets\Scripts\AI\FOV\Editor\FieldOfViewEditor.cs
* Assets\Scripts\AI\FSM\AIAnimationSubState.cs
* Assets\Scripts\AI\FSM\AIBaseState.cs
* Assets\Scripts\AI\FSM\AIEmptySubState.cs
* Assets\Scripts\AI\FSM\AIInvestigateState.cs
* Assets\Scripts\AI\FSM\AIPursuitState.cs
* Assets\Scripts\AI\FSM\AIStateFactory.cs
* Assets\Scripts\AI\FSM\AIStateMachine.cs
* Assets\Scripts\AI\FSM\AIWaypointState.cs
* Assets\Scripts\Audio\FootStepCollection.cs
* Assets\Scripts\Audio\FootStepFactory.cs
* Assets\Scripts\Events\AIThreatChangeEventListener.cs
* Assets\Scripts\Events\SoundThreatEventListener.cs1
* Assets\Scripts\Gameplay\TriggerActivateGameObject.cs
* Assets\Scripts\Gameplay\TriggerCamouflage.cs
* Assets\Scripts\Gameplay\TriggerDeActivateGameObject.cs
* Assets\Scripts\Gameplay\triggerDisableCollider.cs
* Assets\Scripts\Managers\AIManager.cs
* Assets\Scripts\ScriptableObjects\Data Containers\MainCameraData.cs

**Also Contributed to:**
* Assets\Scripts\Player\PlayerControl.cs
* Assets\Scripts\ScriptableObjects\Data Containers\PlayerData.cs

**Shaders:**
* URPStylizedWater.shadergraph
* WindLeaves.shadergraph
* DepthFade.shadergraph
* Movement.shadergraph

**ScriptableObjects:**
* AIGlobalData.asset
* AIHighestStateChange.asset
* AIRequestPlayerCaught.asset
* DirtWalk.asset
* DirtRun.asset
* FootstepEmitter.asset
* FailSound.asset

**Prefab Assets:**
* AI_Guard.prefab
* MovableCrouchCover.prefab
* PathDebugger.prefab
* TriggerAIActiveState.prefab
* WaterShaderFountain.prefab
* Broken Glass.prefab
* Chest.prefab
* GlassBottle.prefab

**Animations:** From Mixamo.com
  * Nervously Look Around.fbx
  * Surprised.fbx
  * Terrified.fbx
  * Reacting.fbx

**Audio:**
* Fail-trumpet-sound-effect.mp3 https://www.youtube.com/watch?v=z8Jn3qnPOGg
* Male-gasp.mp3 https://www.youtube.com/watch?v=g9xXaDyi5_E
* glassbreak.mp3 https://freesound.org/people/scholzi982/sounds/566197/

**Scenes:**
* Level1_Re - Whiteboxing and initial set dressing, trigger placements.
* ai-algorithm (test, not used in alpha)

**Controller:**
* AI_guard.controller

#### 3rd party credits
* Heirarchical FSM - https://www.youtube.com/watch?v=kV06GiJgFhc&t=1535s
* FOV visualisation - https://www.youtube.com/watch?v=rQG9aUWarw
* Probuilder rapid prototyping - https://unity.com/features/probuilder

### Martin
Worked on end-user game flow, all UI logic and animations, and assistance with merging game systems together.
* Authored MainMenu scene, logic and animations.
* Mission Summary screen (exit, restart, return to menus) and associated camera logic.
* Minimap, ObjectiveTracker, Sprint Stamina Bar and Alert status for the HUD.
* Extraction Zone distance and the Extraction zone's appearance.
* Loading screen implementation.
* Scripts for the user's success and game-over conditions.
* Ensuring controllers can navigate all menus in the game.
* Ability and text to skip the intro cutscene.
* Gameplay Bundle prefab
* Pause Menu
* Designed Level1_Re and diagrams conceptually.
Piecing together navigation flow for the user from start to end of experience.

#### Assets Implemented

**Animations and Controllers:**
* All files under ModelsAndAnimations\UI

**Scenes:**
* MainMenu
* Level1_Re (Contributor)

**Prefabs:**
* Gameplay Bundle.prefab
* Minimap Camera.prefab
* HUD Canvas.prefab
* Mission Summary Canvas.prefab
* ExtractionPooint.prefab
PauseMenu

#### Scripts Implemented
* All scripts found under Scripts/UI/*
* AlertStatus.cs
* ChangeMissionText.cs
* DisableCursor.cs
* ExtractionDistance.cs
* LevelSelect.cs
* GameQuitter.cs
* LimitCamera.cs
* MainMenuTransition.cs
* MainMenuCamera.cs
* MissionSummary.cs
* ObjectiveTracker.cs
* PauseMenu.cs
* SprintBar.cs

Contributions:
* SceneCameraController.cs
* PlayerControl.cs
* StealableObject.cs
* ExtractionPoint.cs

#### 3rd party credits
* Milestone assignment for the GameQuitter

### Tom
Primarily worked on systems to support the development of the game. These systems included:
* AudioManager: Handles instantiating and playing 2D and 3D audio sources
* SceneController: Handles loading, unloading, and transitioning scenes at runtime
* Event System: Allows events to be raised via a GameEvent scriptable oject anywhere in the codebase and for the events to be responded to by parametised listener MonoBehaviors. 

Implemented Global Scene, which is the persistent scene into which all others are additively loaded at runtime. Global Scene contains manager scripts that are required to persist across scenes, this prevents wasted effort duplicating reused managers into each scene. 

Added the ambient audio that can be heard throughout the level. Contributed to the decoration of the main level including static items in the library and stealable object room and NPCs in the main street. 

#### Assets Implemented
* Assets\Prefabs\Audio\AmbientSound2D
* Assets\Prefabs\Audio\AmbientSound3D
* Assets\Prefabs\Audio\Audio Test Capsule
* Assets\Prefabs\Audio\EventSound2D
* Assets\Prefabs\Audio\EventSound3d
* Assets\Prefabs\Civilians\AI_Civilian
* Assets\Prefabs\Managers\Audio Manager
* Assets\Prefabs\Managers\Game Manager
* Assets\Prefabs\Managers\Scene Controller
* Assets\Prefabs\Wife_AI
* Assets\Scenes\Global Scene
* Assets\SO Instances\Events\OneShotAudio2D
* Assets\SO Instances\Events\OneShotAudio3D
* Assets\SO Instances\Events\OneShotAudio3DWithString
* Assets\SO Instances\Events\SceneLoadComplete

#### Scripts Implemented/Contributed To
* Assets\Scripts\AI\AIIdleState_Civilian.cs
* Assets\Scripts\AI\AIInvestigateState_Wife.cs
* Assets\Scripts\AI\AIPursuitState_Wife.cs
* Assets\Scripts\AI\AIStateFactory.cs
* Assets\Scripts\AI\AIStateMachine_Civilian.cs
* Assets\Scripts\AI\AIStateMachine_Wife.cs
* Assets\Scripts\Audio\AmbientAudioTest.cs
* Assets\Scripts\Audio\AmbientSound2D.cs
* Assets\Scripts\Audio\AmbientSound3D.cs
* Assets\Scripts\Audio\AudioSourceParams.cs
* Assets\Scripts\Audio\AudioTestCapsule.cs
* Assets\Scripts\Audio\EventSound2D.cs
* Assets\Scripts\Audio\EventSound3D.cs
* Assets\Scripts\Debug\DebugCameraSwitch.cs
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
* Assets\Scripts\Events\PlayerCaughtListener.cs
* Assets\Scripts\Events\PlayerGoalCameraPanCompletedListener.cs
* Assets\Scripts\Events\PlayerGoalCameraPanStartedListener.cs
* Assets\Scripts\Events\PlayerReachedExitListener.cs
* Assets\Scripts\Events\PlayerStoleItemListener.cs
* Assets\Scripts\Events\SceneLoadCompletedListener.cs
* Assets\Scripts\Game Save\GameSave.cs
* Assets\Scripts\Game Save\GameSaveUtil.cs
* Assets\Scripts\Gameplay\Checkpoint.cs
* Assets\Scripts\Gameplay\MissionTimer.cs
* Assets\Scripts\Gameplay\TriggerActiveGameObject.cs
* Assets\Scripts\Managers\AudioManager.cs
* Assets\Scripts\Managers\GameManager.cs
* Assets\Scripts\Managers\SceneController.cs
* Assets\Scripts\Player\ExtractionPoint.cs
* Assets\Scripts\UI\DisableCursor.cs
* Assets\Scripts\UI\EnableCursor.cs
* Assets\Scripts\ScriptableObjects\Events\GameEvent.cs
* Assets\Scripts\ScriptableObjects\ScriptableObjectWithInit.cs

#### 3rd party credits
##### Packages
* Hierarchy Folders Package: https://github.com/xsduan/unity-hierarchy-folders 
* Eflatun.SceneReference Package: https://github.com/starikcetin/Eflatun.SceneReference

##### Audio
Assets\Audio\Birds\
Bird songs cut from clips from: https://dl.allaboutbirds.org/free-bird-song-download-from-the-cornell-lab

Assets\Audio\Music\
Party room music: Quantum Jazz - https://freemusicarchive.org/music/Quantum_Jazz/End_of_Line/06_-_Quantum_Jazz_-_Balcarabic_Chicken/

Assets\Audio\Phonograph\
All tracks taken from 'Antique Phonograph Music Program 01/05/2010` available at: https://freemusicarchive.org/music/Antique_Phonograph_Music_Program_Various_Artists/

Assets\Audio\Scratches\
Scratch sounds played on stealable gramophone taken from: https://freesound.org/people/jswonger/sounds/47924/

Assets\Audio
* 47924__jswonger__record-scratches: https://freesound.org/people/jswonger/sounds/47924/
* 74222_girl_nonsense: https://freesound.org/people/Koen/sounds/74222/
* 80928_door_slam: https://freesound.org/people/bennstir/sounds/80928/
* 106486_car_engine: https://freesound.org/people/KlankOntwerp/sounds/106486/
* 149193_tractor_idle: https://freesound.org/people/videog/sounds/149193/
* 161607__xserra__alhambra-fountain-1: https://freesound.org/people/xserra/sounds/161607/
* 219587_gate_shut: https://freesound.org/people/qubodup/sounds/219587/
* 232175__szalonegacie__talking-people-2: https://freesound.org/people/szalonegacie/sounds/232175/
* 277361_steam_hiss: https://freesound.org/people/beerbelly38/sounds/277361/
* 413203__joepayne__clean-and-pompous-fanfare-trumpet: https://freesound.org/people/joepayne/sounds/413203/
* 676638_plane_engine: https://freesound.org/people/Exoticgaming/sounds/676638/
* city_ambience: https://freesound.org/people/klankbeeld/sounds/279041/
* tindeck_1: https://www.myinstants.com/en/instant/metal-gear-solid-alert/
* wife_hmmm: https://freesound.org/people/esperar/sounds/170766/

##### Animations
Assets\Animations
* The following were all acquired from Mixamo
* Chicken Dance
* Guitar Playing
* Guitar Playing 2
* Identify Point
* Jumping Jacks
* Peering
* Shopping Cart Dance
* Smoking
* Thinking 
* Wild Dance

##### Models
Assets\Studio Nik\
Guitar models: https://assetstore.unity.com/packages/3d/props/low-poly-guitars-pack-161857