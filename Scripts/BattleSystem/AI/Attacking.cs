using System.Collections;
using UnityEngine;

public class Attacking : EnemyBaseState
{
    private UnitAnimator _animator;
    private CombatStateMachine _stateMachine;
    private UnitAudio _audio;
    private UnitBattleActions _actions;

    public Attacking(UnitAnimator animator, UnitAudio audio, UnitBattleActions actions, CombatStateMachine stateMachine) 
    {
        _animator = animator;
        _stateMachine = stateMachine;
        _audio = audio;
        _actions = actions;
    }

    public override void OnEnter(StateMachine stateMachine)
    {
        base.OnEnter(stateMachine);

        randomCount = Random.Range(1, 4);

        _animator.InterruptWalkAnimation();
        _actions.StartCoroutine(Attack());
    }

    private IEnumerator Attack() 
    {
        for (int i = randomCount; i > 0; i--) 
        {
            if (_animator.GetBoolState(AnimationType.IsAttackWindowOpened) == true)
            {
                if (_stateMachine.State.GetType() == typeof(IdleState))
                    _stateMachine.ChangeState(new GroundEntryState(_actions.HitBox, _animator));
                else
                    _stateMachine.AttackPerformed();

                _audio.PlayRandomAudioClip(AudioType.AttackVoiceLine);
            }

            yield return new WaitUntil(() => _animator.GetBoolState(AnimationType.IsAttackWindowOpened) == false);

            yield return new WaitUntil(() => _animator.GetBoolState(AnimationType.IsAttackWindowOpened) == true);
        }

        yield return new WaitForSeconds(1f + (randomCount / 5));

        stateMachine.SetIdleState();
    }
}
