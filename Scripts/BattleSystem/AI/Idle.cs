using System.Collections.Generic;
using UnityEngine;

public class Idle : EnemyBaseState
{
    private List<EnemyBaseState> _states;
    private Enemy _unit;

    public Idle(List<EnemyBaseState> states, Enemy unit) 
    {
        _states = states;
        _unit = unit;
    }

    public override void OnEnter(StateMachine stateMachine)
    {
        base.OnEnter(stateMachine);

        _unit.Animator.InterruptWalkAnimation();

        randomCount = Random.Range(1, 3);

        timer = 0;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        if (timer >= randomCount)
        {
            _unit.ResetAttackState();
            stateMachine.ChangeState(_states[Random.Range(0, _states.Count)]);
        }
    }
}
