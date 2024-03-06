using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.LowLevel;

public class PlayerInput : MonoBehaviour
{
  public float maxForwardInput = 5.0f;
  public float maxTurnInput = 5.0f;

  public float maxForwardSpeed = 1.0f;

  private float curForwardVal = 0f;
  private float curTurnVal = 0f;

  // resulting output control parameters 
  public float playerForward = 0f;
  public float playerTurn = 0f;
  public float playerLookX = 0f;
  public float mouseXScale = 3f;
    public bool playerActionGrab = false;
  public bool playerActionCrouch = false;

  // Update is called once per frame
  void Update()
  {
    playerActionGrab = Input.GetButtonDown("Fire1");
    playerActionCrouch = Input.GetButtonDown("Jump");

    float inputH = Input.GetAxisRaw("Horizontal");
    float inputV = Input.GetAxisRaw("Vertical");

    // controls for controlling the camera relative to player
    // mouse x
    float inputMouseX = Input.GetAxisRaw("Mouse X") * mouseXScale;
    // right analog stick x
    float inputLookX = Input.GetAxisRaw("LookX");
    //Debug.Log("mouse, look: " + inputMouseX + " " + inputLookX);

    // forward movement
    curForwardVal = Mathf.Lerp(curForwardVal, inputV, Time.deltaTime * maxForwardInput);
    curForwardVal = Mathf.Clamp(curForwardVal, -maxForwardSpeed, maxForwardSpeed);
    playerForward = curForwardVal;

    float inputMouseXClamped = Mathf.Clamp(inputMouseX, -1f, 1f);
    float inputLookXClamped = Mathf.Clamp(inputLookX, -1f, 1f);
    if (Mathf.Abs(inputMouseXClamped) < 0.1f)
      inputMouseXClamped = 0f;
    if (Mathf.Abs(inputLookXClamped) < 0.25f)
      inputLookXClamped = 0f;

    // set the player look x output value to the largest magnitude of mouse
    // or right analog stick x 
    if (Mathf.Abs(inputMouseXClamped) < 0.01f)
      playerLookX = Mathf.Lerp(playerLookX, inputLookXClamped, Time.deltaTime * maxTurnInput);
    else
      playerLookX = Mathf.Lerp(playerLookX, inputMouseXClamped, Time.deltaTime * maxTurnInput);

      

    // turn movement
    curTurnVal = Mathf.Lerp(curTurnVal, inputH, Time.deltaTime * maxTurnInput);
    // clamp?
    playerTurn = curTurnVal;

    // For debug, exit if the Escape key is pressed.
    if (Input.GetKeyUp(KeyCode.Escape))
    {
#if UNITY_EDITOR
      UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif      
    }
  }
}