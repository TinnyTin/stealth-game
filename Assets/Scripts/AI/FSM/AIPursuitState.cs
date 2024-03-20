using UnityEngine;

/*
 * CS6457 Attributions
 * Tiny Brain
 * Original Author:     Justin Wu
 * Contributors:
 * Description: Pursuit AI State class
 * External Source Credit: https://www.youtube.com/watch?v=kV06GiJgFhc&t=1535s
 *                      
 */
public class AIPursuitState : AIBaseState
{
    private float lastSpeed;
    public AIPursuitState(AIStateMachine currentContext, AIStateFactory aiStateFactory) : base(currentContext, aiStateFactory)
    {
        IsRootState = true;
        InitializeSubState();
    }
    public override bool SetAIThreatPriority()
    {
        Ctx.aiThreatPriority = AIThreatPriority.Pursuit;
        return true;
    }
    public override void EnterState()
    {
        lastSpeed = Ctx.agent.speed;
        Ctx.setSpeed(Ctx.runSpeed);
        // play metal gear solid sound
        Ctx.AudioChannel.Raise(Ctx.audioClipAlert, Ctx.transform.position, AudioSourceParams.Default);
    }
    public override void UpdateState()
    {
        // Feature to find target if you're in pursuit State and the target is already close by
        Vector3? targetpos = Ctx._FOV.FindTargetWithinRadius(Ctx.pursuitAutoSenseRadius);
        if (targetpos != null)
        {
            Ctx.lastThreat = (Vector3)targetpos;
            Ctx.agent.SetDestination(Ctx.lastThreat);
        }

        // pursue Target
        Ctx.agent.SetDestination(Ctx.lastThreat);
        if ((Ctx.agent.remainingDistance < Ctx.finalCaughtRadius) && !Ctx.agent.pathPending)
        {
            // if the target is no longer in view, need to find the guy
            if ((Ctx.countInView == 0) && (targetpos == null))
            {
                // loop animation LookAroundCut 
                if (!AIAnimationSubState.CheckAnimationString(CurrentSubState, "Look"))
                {
                    AIBaseState animationLookAround = Factory.animationSubState("Look", "triggerLook", null);
                    SwitchSubState(animationLookAround);
                }
            }
            // Else, target is in view. catch him
            else
            {
                // loop animation LookAroundCut 
                if (!AIAnimationSubState.CheckAnimationString(CurrentSubState, "Angry"))
                {
                    AIBaseState animationAngryPoint = Factory.animationSubState("Angry", "triggerAngry", null);
                    SwitchSubState(animationAngryPoint);
                }
                Ctx.CatchPlayer();

            }
        }
    }
    public override void ExitState()
    {
        Ctx.setSpeed(lastSpeed);
    }
    public override void CheckSwitchState()
    {

    }
    public override void InitializeSubState() { }


}
