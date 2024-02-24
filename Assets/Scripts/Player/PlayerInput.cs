using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
  public bool playerActionGrab = false;
  public bool playerActionCrouch = false;

  // Update is called once per frame
  void Update()
  {
    playerActionGrab = Input.GetButtonDown("Fire1");
    playerActionCrouch = Input.GetButtonDown("Jump");

    float inputH = Input.GetAxisRaw("Horizontal");
    float inputV = Input.GetAxisRaw("Vertical");

    // forward movement
    curForwardVal = Mathf.Lerp(curForwardVal, inputV, Time.deltaTime * maxForwardInput);
    curForwardVal = Mathf.Clamp(curForwardVal, -maxForwardSpeed, maxForwardSpeed);
    playerForward = curForwardVal;

    // turn movement
    curTurnVal = Mathf.Lerp(curTurnVal, inputH, Time.deltaTime * maxTurnInput);
    // clamp?
    playerTurn = curTurnVal;
  }
}