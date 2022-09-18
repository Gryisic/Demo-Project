using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Unit))]
public class UnitBattleActions : MonoBehaviour
{
    private const float HEAVY_IMPACT = 8F;
    private const float LAUNCHER_IMPACT = 18F;

    public Collider2D HitBox => _hitBox;

    [SerializeField] private Collider2D _hitBox;
    [SerializeField] private List<GameObject> _specialsVFX;

    [Header("Ground Check")]
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private float _groundCheckRadius;
    [SerializeField] private LayerMask _groundMask;
    private bool _isGrounded;
    private bool _lastGroundedState = false;

    private int _movementSpeed = 5;
    private int _jumpForce = 18;
    private bool _lookedAtRight = true;

    private int _damage;

    private Unit _unit;
    private AttackType _attackType;

    public void SetMovementSpeed(Func<int, int> newSpeed) => _movementSpeed = newSpeed.Invoke(_movementSpeed);

    public void Death() => _unit.Animator.SetTriggerState(AnimationType.Death);

    public void Attack()
    {
        var targets = new Collider2D[10];
        ContactFilter2D contactFilter = new ContactFilter2D();
        contactFilter.useTriggers = true;
        var targetsCount = Physics2D.OverlapCollider(_hitBox, contactFilter, targets);

        for (int i = 0; i < targetsCount; i++)
        {
            if (targets[i].TryGetComponent(out Unit unit))
            {
                if (unit.gameObject.layer != gameObject.layer)
                {
                    unit.RaiseDamageTakenEvent(_damage);

                    Impact(unit.RigidBody);

                    break;
                }
            }
        }
    }

    public void SetAttack(AttackConfig config) 
    {
        _damage = config.Damage;
        _attackType = config.AttackType;
    }

    public void ActivateVFX(int index)
    {
        var vfx = Instantiate(_specialsVFX[index - 1], 
            new Vector2(_hitBox.bounds.center.x, _unit.transform.position.y), Quaternion.identity);

        vfx.transform.SetParent(_unit.transform);
    }

    public void Move(Vector2 direction)
    {
        if (_unit.CanMove == true)
        {
            float scaledMovementSpeed = _movementSpeed * Time.deltaTime;

            _unit.Animator.MoveAnimation(direction, _lookedAtRight);
            transform.Translate(direction * scaledMovementSpeed * (_lookedAtRight ? 1 : -1));
        }
    }

    public void Jump()
    {
        if (_isGrounded == true)
        {
            _unit.RigidBody.velocity = Vector2.up * _jumpForce;

            _unit.Animator.SetTriggerState(AnimationType.Jump);
        }
    }

    public void GroundCheck()
    {
        _isGrounded = Physics2D.OverlapCircle(_groundCheck.position, _groundCheckRadius, _groundMask);

        if (_isGrounded != _lastGroundedState)
        {
            if (_isGrounded == false)
                _unit.Animator.ResetTriggerState(AnimationType.Jump);

            _unit.Animator.SetBoolState(AnimationType.IsGrounded, _isGrounded);

            _lastGroundedState = _isGrounded;
        }
    }

    public bool LookAtRight()
    {
        bool right = transform.position.x < _unit.OpponentTransform.position.x;

        if (_lookedAtRight != right)
        {
            transform.rotation = Quaternion.Euler(0, right ? 0 : 180, 0);
            _lookedAtRight = right;

            _unit.Animator.InterruptWalkAnimation();
        }

        return right;
    }

    public int TakeDamage(int health, int damage)
    {
        string type = $"TakeHit{damage}";

        if (System.Enum.TryParse(type, out AnimationType myType))
        {
            _unit.Animator.SetTriggerState(myType);

            health -= damage;

            if (health <= 0)
            {
                health = 0;

                _unit.RaiseDeathEvent();
            }
        }

        return health;
    }

    private void Awake() => _unit = GetComponent<Unit>();

    private void Impact(Rigidbody2D rigidbody) 
    {
        switch (_attackType) 
        {
            case AttackType.Light:

                break;

            case AttackType.Heavy:
                rigidbody.velocity = Vector2.right * HEAVY_IMPACT * (_unit.LookedAtRight ? 1 : -1);

                break;

            case AttackType.Launcher:
                rigidbody.velocity = Vector2.up * LAUNCHER_IMPACT;

                break;
        }
    }
}
