using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

/*
This script controls two cameras in the scene.  The first is the 
player camera which follows the player during gameplay.
The second camera is an animated camera which plays back
an animation clip panning from the goal stealable object
to the player start / extraction point.

It includes a fade to black at the end of the animated camera,
and fade from black to to the player camera.  During the animated
camera pan, player input is disabled.

To function properly, you must add this script as a component of 
a GameObject in the scene, and set the following public variables in
the component:

PlayerCamera: the MainCamera which follows the player during gameplay
IntroAnimationCamera:  the animated camera for the intro pan
ppVolumeGameObj:  the scene's post processing volume.  It must have a 
Color Adjustments override applied.  This is how we apply fade-in/out

*/

public class SceneCameraController : MonoBehaviour
{
  public GameObject PlayerCamera;
  public GameObject IntroAnimationCamera;
  public GameObject HUD;

  // IntroAnimationCamera Animator component
  private Animator introCameraAnimator;

  private string playStateName = "PlayCameraAnimation";
  private int playStateNameHash;

  // duration in seconds to fade out the IntroAnimationCamera
  public float FadeOutDuration = 2f;
  // duration in seconds to fade in the PlayerCamera
  public float FadeInDuration = 2f;

  private bool isPlayIntroAnimation = false;

  private PlayerControl playerControl;

  public GameObject ppVolumeGameObj;
  private Volume ppVolume;
  private VolumeProfile ppProfile;

  private ColorAdjustments ppColorAdjust;

  private float fadePlayerStartTime;
  private bool isFadePlayer = false;
    public GameObject skipIntroText;

  // Start is called before the first frame update
  void Start()
  {
    // get player in order to cache reference to PlayerControl 
    // as we need to disable input controls during
    // IntroAnimationCamera playback.

    GameObject player = GameObject.Find("player");
    if(player == null)
    {
      Debug.Log("SceneCameraController: player not found.");
      return;
    }
    playerControl = player.GetComponent<PlayerControl>();

    playStateNameHash = Animator.StringToHash("Base Layer." + playStateName);

    if (PlayerCamera == null || IntroAnimationCamera == null)
    {
      Debug.LogError("SceneCameraController: No PlayerCamera, IntroAnimationCamera.");
    }

    introCameraAnimator = IntroAnimationCamera.GetComponent<Animator>();
    if(introCameraAnimator == null)
    {
      Debug.LogError("SceneCameraController: No IntroAnimationCamera Animator component.");
    }

    // get the post process volume color adjust layer
    ppProfile = ppVolumeGameObj.GetComponent<UnityEngine.Rendering.Volume>()?.profile;
    ppProfile.TryGet(out ppColorAdjust);
    if(ppProfile == null)
    {
      Debug.LogError("SceneCameraController: Postprocess profile invalid.");
    }
    isPlayIntroAnimation = false;

    Play();
    
        //playerControl.isPlayerControlEnabled = true;
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape) || Input.GetKeyUp(KeyCode.Space))
        {
            skipIntro();
        }
    }

    void FixedUpdate()
  {
    //test checking the path hash
    if (introCameraAnimator.GetCurrentAnimatorStateInfo(0).fullPathHash != playStateNameHash)
      return;

    if (!isPlayIntroAnimation)
    {
        return;
    }

    if (!isFadePlayer)
    {
      // intro animation camera state, but we haven't begun fading effects.

      // manage fades based on the IntroAnimationCamera animation 
      // playback

      Animator introAnim = IntroAnimationCamera.GetComponent<Animator>();

      float animTimeNormalized = introAnim.GetCurrentAnimatorStateInfo(0).normalizedTime;
      float animLength = introAnim.GetCurrentAnimatorStateInfo(0).length;
      float animTime = (animLength * animTimeNormalized);
      float animTimeToEndOfState = animLength - animTime;

      if (animTimeNormalized >= 1f)
      {
        // the IntroAnimationCamera animation and fade is complete.
        IntroAnimationCamera.GetComponent<Camera>().enabled = false;
        PlayerCamera.GetComponent<Camera>().enabled = true;

        playerControl.isPlayerControlEnabled = true;

        isFadePlayer = true;
        fadePlayerStartTime = Time.realtimeSinceStartup;
      }
      else if (animTime <= FadeInDuration)
      {
        float t = 1f - (animTime / FadeInDuration);
        ppColorAdjust.postExposure.value = t * -16f;
      }
      else if (animTimeToEndOfState <= FadeOutDuration)
      {
        // near end of the IntroAnimationCamera's anim clip.  fade to black.
        float t = (FadeOutDuration - animTimeToEndOfState) / FadeOutDuration;
        ppColorAdjust.postExposure.value = t * -16f;
      }
    }
    else
    {
      // animated pan camera fade out complete. fade back to player camera
      // this fade is not based on any animation clip playback
      float fadeTime = Time.realtimeSinceStartup - fadePlayerStartTime;
      float t = 1f - (fadeTime / FadeInDuration);
      ppColorAdjust.postExposure.value = t * -16f;

      if (t <= 0f)
      {
        // done with all fades. reset the exposure adjustment to zero and activate HUD
        ppColorAdjust.postExposure.value = 0f;
        isPlayIntroAnimation = false;
        HUD.SetActive(true);
      }
    }
  }

  // switch to the IntroAnimationCamera and start playing the intro
  // pan animation.
  public void Play()
  {
    if (isPlayIntroAnimation)
    {
            return;
    }
    PlayerCamera.GetComponent<Camera>().enabled = false;
    IntroAnimationCamera.GetComponent<Camera>().enabled = true;
    HUD.SetActive(false); // Deactivate HUD while intro sequence is playing
    skipIntroText.SetActive(true);

    introCameraAnimator.Play(playStateName);

    playerControl.isPlayerControlEnabled = false;

    isPlayIntroAnimation = true;
    isFadePlayer = false;
  }

    private void skipIntro() {
        PlayerCamera.GetComponent<Camera>().enabled = true;
        skipIntroText.SetActive(false);
        IntroAnimationCamera.SetActive(false);
        playerControl.isPlayerControlEnabled = true;
        HUD.SetActive(true);
    }
}
