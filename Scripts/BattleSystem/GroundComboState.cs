using UnityEngine;

public class GroundComboState : MeleeBaseState
{
    public GroundComboState(Collider2D hitBox, UnitAnimator animator) : base(hitBox, animator) { }

    public override void OnEnter(StateMachine stateMachine)
    {
        base.OnEnter(stateMachine);

        duration = 0.5f;
        animator.SetTriggerState(AnimationType.Attack2);
        animator.SetAttackIndex(2);
        animator.SetBoolState(AnimationType.IsAttackWindowOpened, false);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        if (timer >= duration)
        {
            if (shouldCombo == true)
                stateMachine.ChangeState(new GroundFinisherState(hitBox, animator));
            else
                stateMachine.SetIdleState();
        }
    }
}
