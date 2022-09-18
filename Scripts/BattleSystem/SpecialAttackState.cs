using UnityEngine;

public class SpecialAttackState : MeleeBaseState
{
    protected int attackIndex;

    public SpecialAttackState(Collider2D hitBox, UnitAnimator animator, int attackIndex) : base(hitBox, animator) =>
        this.attackIndex = attackIndex;

    public override void OnEnter(StateMachine stateMachine)
    {
        base.OnEnter(stateMachine);

        var specialType = (AnimationType)System.Enum.Parse(typeof(AnimationType), $"Special{attackIndex}");

        duration = 1.5f;
        animator.SetTriggerState(specialType);
        animator.SetAttackIndex(attackIndex);
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
