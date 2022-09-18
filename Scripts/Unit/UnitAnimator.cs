using System.Collections.Generic;
using UnityEngine;

public enum AnimationType 
{
    WalkForward,
    WalkBackward,
    Run,
    Jump,
    Attack1,
    Attack2,
    Attack3,
    Special1,
    Special2,
    Special3,
    TakeHit1,
    TakeHit2,
    TakeHit3,
    Death,
    Victory,
    Restore,
    IsAttackWindowOpened,
    IsGrounded
}

public class UnitAnimator 
{
    private Animator _animator;

    private Dictionary<AnimationType, int> _animations = new Dictionary<AnimationType, int>();

    private int _attackIndexHash;

    public UnitAnimator(Animator animator) 
    {
        _animator = animator;

        Initialize();
    }

    public void SetTriggerState(AnimationType type) => _animator.SetTrigger(_animations[type]);

    public void ResetTriggerState(AnimationType type) => _animator.ResetTrigger(_animations[type]);

    public void SetBoolState(AnimationType type, bool state) => _animator.SetBool(_animations[type], state);

    public void SetAttackIndex(int value) => _animator.SetInteger(_attackIndexHash, value);

    public int GetAttackIndex() => _animator.GetInteger(_attackIndexHash);

    public bool GetBoolState(AnimationType type) => _animator.GetBool(_animations[type]);

    public void MoveAnimation(Vector2 direction, bool lookAtRight)
    {
        bool move = direction != Vector2.zero;

        if (move == false) 
        {
            InterruptWalkAnimation();

            return;
        }

        bool forward = lookAtRight ? direction.x >= 0 : direction.x <= 0;
        AnimationType directionType = forward ? AnimationType.WalkForward : AnimationType.WalkBackward;

        SetBoolState(directionType, move);
    }

    public void InterruptWalkAnimation() 
    {
        SetBoolState(AnimationType.WalkForward, false);
        SetBoolState(AnimationType.WalkBackward, false);
    }

    private void AddAnimationToDictionary(AnimationType key, int value) => _animations.Add(key, value);

    private void Initialize() 
    {
        var animations = System.Enum.GetValues(typeof(AnimationType));

        foreach (AnimationType animation in animations) 
        {
            AddAnimationToDictionary(animation, Animator.StringToHash(animation.ToString()));
        }

        _attackIndexHash = Animator.StringToHash("AttackIndex");
    }
}
