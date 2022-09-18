using UnityEngine;

public class UnitUtils : MonoBehaviour
{
    public bool IsInvulnerable => _isInvulnerable;
    public bool CanMove => _canMove;

    private Unit _unit;
    private bool _isInvulnerable = false;
    private bool _canMove = true;

    public void AttachUnit(Unit unit) => _unit = unit;

    public void ResetAttackWindow() => _unit.Animator.SetBoolState(AnimationType.IsAttackWindowOpened, true);

    public void FreezeVerticalPosition() => _unit.RigidBody.simulated = false;

    public void SetInvulnerability() => _isInvulnerable = true;

    public void RemoveInvulnerability() => _isInvulnerable = false;

    public void SetImmobility() => _canMove = false;
    public void ResetImmobility() => _canMove = true;

    public void Push(float amount) =>
    _unit.RigidBody.velocity = Vector2.right * amount * (_unit.LookedAtRight ? 1 : -1);

    public void UnFreezeVerticalPosition()
    {
        _unit.RigidBody.simulated = true;
        _unit.RigidBody.velocity = Vector3.zero;
    }
}
