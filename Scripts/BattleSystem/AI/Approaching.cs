using System;
using UnityEngine;

public class Approaching : EnemyBaseState
{
    private Enemy _unit;
    private Transform _opponent;
    private Action<Vector2> _move;
    private Func<float> _distance;

    public Approaching(Enemy unit, Transform opponent, Action<Vector2> move, Func<float> distance) 
    {
        _unit = unit;
        _opponent = opponent;
        _move = move;
        _distance = distance;
    }

    public override void OnEnter(StateMachine stateMachine)
    {
        base.OnEnter(stateMachine);

        timer = 0;
        randomCount = UnityEngine.Random.Range(1, 4);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        if (timer <= randomCount) 
        {
            if (_distance.Invoke() > 0.5f) 
            {
                var direction = (_opponent.position - _unit.transform.position).normalized;

                _move.Invoke(direction);
            }
        }
        else
            stateMachine.SetIdleState();
    }
}
