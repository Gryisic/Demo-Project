using System;
using UnityEngine;

[RequireComponent(typeof(UnitUtils))]
[RequireComponent(typeof(Animator), typeof(Rigidbody2D), typeof(UnitBattleActions))]
public class Unit : MonoBehaviour
{
    public int MaxHealth => maxHealth;
    public int Health => health;
    public bool IsActive => isActive;
    public bool LookedAtRight => battleActions.LookAtRight();
    public bool CanMove => utils.CanMove;
    public UnitAnimator Animator => animator;
    public Rigidbody2D RigidBody => m_rigidbody;
    public Transform OpponentTransform => opponentTransform;

    public event Action<int, int> OnHealthChange;
    public event Action OnAttack;
    public event Action OnTakeDamage;
    public event Func<int, int, int> OnDamageTaken;
    public event Action OnDeath;

    [Space]
    [Header("Battle")]
    protected UnitBattleActions battleActions;

    [Space]
    [Header("Sound")]
    [SerializeField] protected UnitAudio sound;

    [Space]
    [Header("Stats")]
    [SerializeField] protected int maxHealth;
    protected int health;
    protected bool isActive = false;

    [Space]
    [Header("UI")]
    [SerializeField] private HealthBar _healthBar;

    protected Transform opponentTransform;
    protected UnitAnimator animator;
    protected Rigidbody2D m_rigidbody;
    protected CombatStateMachine stateMachine;
    protected UnitUtils utils;

    public virtual void Activate() => isActive = true;

    public virtual void RaiseDamageTakenEvent(int damage)
    {
        if (utils.IsInvulnerable == false)
        {
            sound.PlayRandomAudioClip(AudioType.TakeHitVoiceline);
            sound.PlayRandomAudioClip(AudioType.Effect);
            health = OnDamageTaken.Invoke(health, damage);
        }

        RaiseHealthChangeEvent();
    }

    public void RaiseDeathEvent()
    {
        isActive = false;

        OnDeath?.Invoke();
    }


    public void RaiseAttackEvent() => OnAttack?.Invoke();

    public void RaiseHealthChangeEvent() => OnHealthChange?.Invoke(maxHealth, health);

    public void RaiseTakeDamageEvent() => OnTakeDamage?.Invoke();

    public void AttachOpponent(Transform opponentTransform)
    {
        this.opponentTransform = opponentTransform;

        battleActions.LookAtRight();
    }

    public void Restore() 
    {
        health = maxHealth;

        RaiseHealthChangeEvent();
        animator.SetTriggerState(AnimationType.Restore);
    }

    private void OnEnable()
    {
        OnAttack += battleActions.Attack;
        OnDamageTaken += battleActions.TakeDamage;
        OnHealthChange += _healthBar.UpdateFillAmount;
        OnDeath += battleActions.Death;
        OnDeath += sound.PlayDeathAudioClip;
    }

    private void OnDisable()
    {
        OnAttack -= battleActions.Attack;
        OnDamageTaken -= battleActions.TakeDamage;
        OnHealthChange -= _healthBar.UpdateFillAmount;
        OnDeath -= battleActions.Death;
        OnDeath -= sound.PlayDeathAudioClip;
    }

    private void Awake() => Initialize();

    private void Update() => stateMachine.Update();

    protected virtual void Initialize() 
    {
        stateMachine = new CombatStateMachine();
        animator = new UnitAnimator(GetComponent<Animator>());

        m_rigidbody = GetComponent<Rigidbody2D>();
        battleActions = GetComponent<UnitBattleActions>();
        utils = GetComponent<UnitUtils>();

        utils.AttachUnit(this);

        health = maxHealth;
    }
}
