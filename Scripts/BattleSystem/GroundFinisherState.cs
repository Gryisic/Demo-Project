using UnityEngine;

public class GroundFinisherState : MeleeBaseState
{
    public GroundFinisherState(Collider2D hitBox, UnitAnimator animator) : base(hitBox, animator) { }

    public override void OnEnter(StateMachine stateMachine)
    {
        base.OnEnter(stateMachine);

        duration = 0.5f;
        animator.SetTriggerState(AnimationType.Attack3);
        animator.SetAttackIndex(3);
        animator.SetBoolState(AnimationType.IsAttackWindowOpened, false);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        if (timer >= duration)
        {
            stateMachine.SetIdleState();
        }
    }
}
