using System;
using UnityEngine;

public class Retreating : EnemyBaseState
{
    private Enemy _unit;
    private Transform _opponent;
    private Action<Vector2> _move;

    public Retreating(Enemy unit, Transform opponent, Action<Vector2> move)
    {
        _unit = unit;
        _opponent = opponent;
        _move = move;
    }

    public override void OnEnter(StateMachine stateMachine)
    {
        base.OnEnter(stateMachine);

        timer = 0;
        randomCount = UnityEngine.Random.Range(1, 2);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        if (timer <= randomCount)
        {
            var direction = (_unit.transform.position - _opponent.position).normalized;

            _unit.Animator.SetBoolState(AnimationType.WalkBackward, true);

            _move.Invoke(direction);
        }
        else
            stateMachine.SetIdleState();
    }
}
