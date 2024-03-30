/*
 * CS6457 Attributions
 * Tiny Brain
 * Original Author:     Justin Wu
 * Contributors:        Tom
 * Description: Idle AI State class - Civilian specific
 * External Source Credit: https://www.youtube.com/watch?v=kV06GiJgFhc&t=1535s
 *                      
 */
public class AIIdleState_Civilian : AIBaseState
{
    public string animStateName;

    public AIIdleState_Civilian(AIStateMachine currentContext, AIStateFactory aiStateFactory, string animStateName) : base(currentContext, aiStateFactory)
    {
        IsRootState = true;
        InitializeSubState();
        this.animStateName = animStateName; 
    }

    public override bool SetAIThreatPriority()
    {
        return false;
    }

    public override void EnterState()
    {
        Ctx.agent.isStopped = false; 

        if (Ctx is AIStateMachine_Civilian civilianCtx)
        {
            // play default idle animation
            AIBaseState idleAnimation = Factory.animationSubState(animStateName, "trigger" + animStateName, null, true);
            SwitchSubState(idleAnimation);
        }
    }

    public override void UpdateState()
    {
        
    }

    public override void ExitState()
    {
        
    }

    public override void CheckSwitchState()
    {

    }

    public override void InitializeSubState()
    {

    }
}
