using UnityEngine;

public class GroundEntryState : MeleeBaseState
{
    public GroundEntryState(Collider2D hitBox, UnitAnimator animator) : base(hitBox, animator) { }

    public override void OnEnter(StateMachine stateMachine)
    {
        base.OnEnter(stateMachine);

        duration = 0.3f;
        animator.SetTriggerState(AnimationType.Attack1);
        animator.SetAttackIndex(1);
        animator.SetBoolState(AnimationType.IsAttackWindowOpened, false);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        if (timer >= duration)
        {
            if (shouldCombo == true)
                stateMachine.ChangeState(new GroundComboState(hitBox, animator));
            else
                stateMachine.SetIdleState();
        }
    }
}
