using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.LowLevel;

/*
 * CS6457 Attributions
 * Tiny Brain
 * Original Author:     Erik
 * Contributors:        
 */

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
    public float playerLookY = 0f;
    public float mouseXScale = 3f;
    public float mouseYScale = 3f;
    public bool playerActionGrab = false;
    public bool playerActionCrouch = false;
    public bool playerIsSprint = false;

    // Update is called once per frame
    void Update()
    {
        playerActionGrab = Input.GetButtonDown("Fire1");
        playerActionCrouch = Input.GetKeyDown(KeyCode.C);


        float inputH = Input.GetAxisRaw("Horizontal");
        float inputV = Input.GetAxisRaw("Vertical");

        // controls for controlling the camera relative to player mouse x
        float inputMouseX = Input.GetAxisRaw("Mouse X") * mouseXScale;
        // controls for controlling the camera relative to player mouse y
        float inputMouseY = Input.GetAxisRaw("Mouse Y") * mouseYScale;

        // right analog stick x
        float inputLookX = Input.GetAxisRaw("LookX");
        //Debug.Log("mouse, look: " + inputMouseX + " " + inputLookX);

        // forward movement
        curForwardVal = Mathf.Lerp(curForwardVal, inputV, Time.deltaTime * maxForwardInput);
        curForwardVal = Mathf.Clamp(curForwardVal, -maxForwardSpeed, maxForwardSpeed);
        playerForward = curForwardVal;

        if(Input.GetKeyDown(KeyCode.LeftShift))
            playerIsSprint = true;
        if (Input.GetKeyUp(KeyCode.LeftShift))
            playerIsSprint = false;


        float inputMouseXClamped = Mathf.Clamp(inputMouseX, -1f, 1f);
        float inputLookXClamped = Mathf.Clamp(inputLookX, -1f, 1f);
        if (Mathf.Abs(inputMouseXClamped) < 0.1f)
            inputMouseXClamped = 0f;
        if (Mathf.Abs(inputLookXClamped) < 0.25f)
            inputLookXClamped = 0f;

        if (Mathf.Abs(inputMouseXClamped) < 0.1f)
            inputMouseXClamped = 0f;

        // set the player look x output value to the largest magnitude of mouse
        // or right analog stick x 
        if (Mathf.Abs(inputMouseXClamped) < 0.01f)
            playerLookX = Mathf.Lerp(playerLookX, inputLookXClamped, Time.deltaTime * maxTurnInput);
        else
            playerLookX = Mathf.Lerp(playerLookX, inputMouseXClamped, Time.deltaTime * maxTurnInput);

        // vertical mouse look
        float inputMouseYClamped = Mathf.Clamp(inputMouseY, -1f, 1f);
        playerLookY = Mathf.Lerp(playerLookY, inputMouseYClamped, Time.deltaTime * maxTurnInput);



        // turn movement
        curTurnVal = Mathf.Lerp(curTurnVal, inputH, Time.deltaTime * maxTurnInput);
        // clamp?
        playerTurn = curTurnVal;
    }
}