using UnityEngine;

public class Player : Unit
{
    private PlayerInput _input;

    private Vector2 _moveDirection;

    public void EnableInput() => _input.Enable();

    public void DisableInput() => _input.Disable();

    public override void Activate()
    {
        base.Activate();

        EnableInput();
    }

    public override void Deactivate()
    {
        base.Deactivate();

        DisableInput();
    }

    protected override void Initialize()
    {
        _input = new PlayerInput();

        BindActionsToInput();

        base.Initialize();
    }

    private void FixedUpdate()
    {
        if (IsActive)
        {
            if (_moveDirection != Vector2.zero)
                battleActions.Move(_moveDirection);

            battleActions.LookAtRight();
            battleActions.GroundCheck();
        }
    }

    private void PerformSpecialAttack(int attackIndex) 
    {
        if (animator.GetBoolState(AnimationType.IsAttackWindowOpened) == true)
        {
            stateMachine.ChangeState(new SpecialAttackState(battleActions.HitBox, animator, attackIndex));

            sound.PlayRandomAudioClip(AudioType.AttackVoiceLine);
        }
    }

    private void BindActionsToInput() 
    {
        _input.Player.Special1.performed += context => PerformSpecialAttack(1);

        _input.Player.Special2.performed += context => PerformSpecialAttack(2);

        _input.Player.Special3.performed += context => PerformSpecialAttack(3);

        _input.Player.Jump.performed += context => battleActions.Jump();

        _input.Player.Move.started += context => _moveDirection = _input.Player.Move.ReadValue<Vector2>();

        _input.Player.Move.canceled += context =>
        {
            _moveDirection = Vector2.zero;

            animator.MoveAnimation(_moveDirection, battleActions.LookAtRight());
        };

        _input.Player.Run.started += context =>
        {
            battleActions.SetMovementSpeed(movementSpeed => movementSpeed * 2);

            animator.SetBoolState(AnimationType.Run, true);
        };

        _input.Player.Run.canceled += context =>
        {
            battleActions.SetMovementSpeed(movementSpeed => movementSpeed / 2);

            animator.SetBoolState(AnimationType.Run, false);
        };

        _input.Player.Attack.performed += context =>
        {
            if (animator.GetBoolState(AnimationType.IsAttackWindowOpened) == true) 
            {
                if (stateMachine.State.GetType() == typeof(IdleState))
                    stateMachine.ChangeState(new GroundEntryState(battleActions.HitBox, animator));
                else
                    stateMachine.AttackPerformed();

                sound.PlayRandomAudioClip(AudioType.AttackVoiceLine);
            }
        };
    }
}
