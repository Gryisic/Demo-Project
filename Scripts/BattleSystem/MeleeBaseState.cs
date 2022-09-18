using UnityEngine;

public class MeleeBaseState : State
{
    protected float duration;
    protected bool shouldCombo;

    protected Collider2D hitBox;
    protected UnitAnimator animator;

    protected float attackPressedTimer = 0;

    public MeleeBaseState(Collider2D hitBox, UnitAnimator animator) 
    {
        this.hitBox = hitBox;
        this.animator = animator;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        if (attackPressedTimer > 0)
            attackPressedTimer -= Time.deltaTime;

        if (animator.GetBoolState(AnimationType.IsAttackWindowOpened) == true && attackPressedTimer > 0)
            shouldCombo = true;
    }

    public void AttackPerformed() => attackPressedTimer = 2;
}
