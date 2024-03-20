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
public class AIEmptySubState : AIBaseState
{
    private string _stringTrigger;
    private string _stringTriggerExit;
    private string _stringAnimation;
    private AudioClip _audioClip;

    public AIEmptySubState(AIStateMachine currentContext, AIStateFactory aiStateFactory) : base(currentContext, aiStateFactory)
    {
        IsRootState = false; // substate only
        InitializeSubState();
    }
    public override bool SetAIThreatPriority()
    {
        return false;
    }

    public override void EnterState() { }

    public override void UpdateState() { }

    public override void ExitState() { }

    public override void CheckSwitchState() { }

    public override void InitializeSubState() { }

}
