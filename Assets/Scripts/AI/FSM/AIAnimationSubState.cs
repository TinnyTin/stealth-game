using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * CS6457 Attributions
 * Tiny Brain
 * Original Author:     Justin Wu
 * Contributors:
 * Description: Play blended animation that will exit upon completion or exit when superstate exits.
 *              This requires animator controller to have 
 *                  parameter trigger<animation>, 
 *                  animation state <animation>,
 *                  trigger<animation>Exit
 *                      
 */
public class AIAnimationSubState : AIBaseState
{
    private string _stringTrigger;
    private string _stringTriggerExit;
    private string _stringAnimation;
    private AudioClip _audioClip;

    private bool _freezeFOV;

    public string StringAnimation { get { return _stringAnimation; } }

    public AIAnimationSubState(AIStateMachine currentContext, AIStateFactory aiStateFactory, string strAnimation, string strTrigger, AudioClip audioClip, bool freezeFOV) : base(currentContext, aiStateFactory)
    {
        IsRootState = false; // substate only
        InitializeSubState();

        // set strings
        _stringTrigger = strTrigger;
        _stringTriggerExit = strTrigger + "Exit";
        _stringAnimation = strAnimation;

        // set audioclip
        _audioClip = audioClip;

        // freeze FOV
        _freezeFOV = freezeFOV;
    }
    public override bool SetAIThreatPriority()
    {
        return false;
    }
    public override void EnterState()
    {
        // reset trigger exit animation
        Ctx.anim.ResetTrigger(_stringTriggerExit);

        // trigger animation
        Ctx.anim.SetTrigger(_stringTrigger);

        // raise audio sound 3D
        if (_audioClip != null)
            Ctx.AudioChannel.Raise(_audioClip, Ctx.transform.position, AudioSourceParams.Default);

        // Check if animation should not affect FOV moving around
        if (_freezeFOV)
        {
            lastFOVupdateState = Ctx.FOV.updateTransform;
        }
    }
    public override void UpdateState()
    {
        CheckSwitchState();
    }
    public override void ExitState()
    {
        Ctx.anim.ResetTrigger(_stringTrigger);
        Ctx.anim.SetTrigger(_stringTriggerExit);

        // reset FOV update state
        if (_freezeFOV)
        {
            Ctx.FOV.updateTransform = true;
        }
    }
    public override void CheckSwitchState()
    {
        // check animation has finished
        if (Ctx.anim.GetCurrentAnimatorStateInfo(0).IsName(_stringAnimation) && Ctx.anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
        {
            // exit substate by switching to empty substate
            SwitchState(Factory.EmptySubState());
        }
    }
    public override void InitializeSubState() { }

    public static bool CheckAnimationString(AIBaseState baseState, string stringAnimation)
    {

        return ((baseState is AIAnimationSubState) && ((AIAnimationSubState)baseState).StringAnimation == stringAnimation);
    }
}
