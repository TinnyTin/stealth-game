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
    private bool hasRequestedCatchPlayer = false;
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

        // trigger to catch player 
        hasRequestedCatchPlayer = false;
    }
    public override void UpdateState()
    {
        // Feature to find target if you're in pursuit State and the target is already close by
        Vector3? targetpos = Ctx.FOV.FindTargetWithinRadius(Ctx.pursuitAutoSenseRadius);
        if (targetpos != null)
        {
            Ctx.lastThreat = (Vector3)targetpos;
        }

        // pursue Target
        Ctx.agent.SetDestination(Ctx.lastThreat);

        // All checks to make sure the player is able to get caught.
        // 1. agent is close enough to its destination by finalCaughtRadius AND
        if ((Ctx.agent.remainingDistance < Ctx.finalCaughtRadius) && !Ctx.agent.pathPending)
        {
            // 2. player position is within that finalCaughtRadius AND
            // 3. player is either in view OR within AI auto-sense radius 
            if ((Ctx.playerData.PlayerPosition != null) &&
                (Vector3.Magnitude(Ctx.playerData.PlayerPosition - Ctx.agent.nextPosition) < Ctx.finalCaughtRadius + Ctx.finalCaughtNavMeshTolerance) &&
                ((Ctx.countInView != 0) || (targetpos.HasValue)))
            {
                // loop animation Angry Point 
                if (!AIAnimationSubState.CheckAnimationString(CurrentSubState, "Angry"))
                {
                    AIBaseState animationAngryPoint = Factory.animationSubState("Angry", "triggerAngry", null, true);
                    SwitchSubState(animationAngryPoint);
                }
                if (!hasRequestedCatchPlayer)
                {
                    Ctx.CatchPlayer();
                    hasRequestedCatchPlayer = true;
                }
            }
            // only look around confused when you are close to your final caught radius
            else
            {
                // loop animation Look 
                if (!AIAnimationSubState.CheckAnimationString(CurrentSubState, "Look"))
                {
                    AIBaseState animationLookAround = Factory.animationSubState("Look", "triggerLook", null, true);
                    SwitchSubState(animationLookAround);
                }


            }
        }
        // exit out of animation lock if the target is in view or a sound is made
        if (((Ctx.countInView != 0) || (targetpos.HasValue)))
            if (!AIAnimationSubState.CheckAnimationString(CurrentSubState, "Angry"))
                SwitchSubState(Factory.EmptySubState());

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
