using System.Collections.Generic;
using UnityEngine;

public class Enemy : Unit
{
    private List<EnemyBaseState> _states;

    private EnemyStateMachine _stateMachine;

    private bool _isAttacking = false;

    public override void Activate()
    {
        base.Activate();

        EnableAI();
    }

    public override void Deactivate()
    {
        base.Deactivate();

        DisableAI();
    }

    public override void RaiseDamageTakenEvent(int damage)
    {
        _stateMachine.SetIdleState();

        base.RaiseDamageTakenEvent(damage);
    }

    public void ResetAttackState() => _isAttacking = false;

    private void EnableAI() 
    {
        _states = new List<EnemyBaseState>()
    {
        new Approaching(this, opponentTransform, battleActions.Move, DistanceToOpponent),
        new Retreating(this, opponentTransform, battleActions.Move)
    };

        _stateMachine = new EnemyStateMachine(new Idle(_states, this));
        _stateMachine.SetIdleState();
    }

    private void DisableAI() => _stateMachine = null;

    private void FixedUpdate()
    {
        if (isActive) 
        {
            _stateMachine.Update();

            battleActions.LookAtRight();
            battleActions.GroundCheck();

            if (DistanceToOpponent() < 3f && _isAttacking == false)
            {
                _stateMachine.ChangeState(new Attacking(animator, sound, battleActions, stateMachine));
                _isAttacking = true;
            }
            else if (DistanceToOpponent() > 3f && _isAttacking == true) 
            {
                _isAttacking = false;
            }
        }
    }

    private float DistanceToOpponent() => Vector2.Distance(transform.position, opponentTransform.position);
}
