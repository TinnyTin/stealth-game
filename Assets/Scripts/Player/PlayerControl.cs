using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Note - to function properly, make sure to turn on isKinemetic parameter 
// in the player GameObject's RigidBody.

[RequireComponent(typeof(Animator), typeof(Rigidbody), typeof(PlayerInput))]
public class PlayerControl : MonoBehaviour
{
  private Rigidbody rbody;
  private Animator anim;
  private PlayerInput input;

  public float animationSpeed = 1f;
  public float rootMovementSpeed = 1f;
  public float rootTurnSpeed = 1f;

  // cached values from PlayerInput
  private float _playerForward = 0f;
  private float _playerTurn = 0f;
  private bool _playerActionGrab = false;
  private bool _playerActionCrouch = false;

  private bool isCrouched = false;

  // Start is called before the first frame update
  void Start()
  {

  }

  void Awake()
  {
    rbody = GetComponent<Rigidbody>();
    if(rbody == null)
    {
      Debug.LogError("PlayerControl: no RigidBody component.");
    }
    anim = GetComponent<Animator>();
    if (anim == null)
    {
      Debug.LogError("PlayerControl: no Animator component.");
    }

    input = GetComponent<PlayerInput>();
    if (input == null)
    {
      Debug.LogError("PlayerControl: no PlayerInput component.");
    }
  }

  // Update is called once per frame
  void Update()
  {
    // cache the player input parameters
    _playerForward = input.playerForward;
    _playerTurn = input.playerTurn;

    // don't overwrite the cached input buttons (if true, not yet handled)
    _playerActionGrab = input.playerActionGrab || _playerActionGrab;
    _playerActionCrouch = input.playerActionCrouch || _playerActionCrouch;
  }

  private void FixedUpdate()
  {
    anim.SetFloat("velx", _playerTurn);
    anim.SetFloat("vely", _playerForward);

    if (_playerForward > 0.1f || _playerTurn > 0.1f)
    {
      anim.SetBool("move", true);
    }
    else
    {
      anim.SetBool("move", false);
    }

    if (_playerActionCrouch)
    {
      //anim.SetBool("doButtonPress", _playerActionCrouch);
      _playerActionCrouch = false;
      isCrouched = !isCrouched;
      Debug.Log("Crouch: " + isCrouched);
    }

    if(_playerActionGrab)
    {
      _playerActionGrab = false;
      Debug.Log("Grab item");
    }

  }

  // manage the root motion depending on animator state and input
  void OnAnimatorMove()
  {
    Quaternion newRot = anim.rootRotation;
    Vector3 newPos = anim.rootPosition;

    Vector3 newRotEuler = newRot.eulerAngles;
    newRotEuler.x = 0f; 
    newRotEuler.z = 0f;
    newRot.eulerAngles = newRotEuler;

    this.transform.position = newPos;
    this.transform.rotation = newRot;


    // allow faster transitions
    //newPos = Vector3.LerpUnclamped(this.transform.position, newPos, rootMovementSpeed);
    //newRot = Quaternion.LerpUnclamped(this.transform.rotation, newRot, rootTurnSpeed);

    rbody.MovePosition(newPos);
    rbody.MoveRotation(newRot);
  }



}
