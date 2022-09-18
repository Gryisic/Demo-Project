using System.Collections;
using UnityEngine;

public class Battle : MonoBehaviour
{
    [Header("Units")]
    [SerializeField] private Unit _player;
    [SerializeField] private Unit _enemy;
    [SerializeField] private Transform _playerPossition;
    [SerializeField] private Transform _enemyPosition;

    [Space]
    [Header("Announcer")]
    [SerializeField] private Announcer _announcer;

    [Space]
    [Header("Camera")]
    [SerializeField] private BattleCamera _battleCamera;

    [Space]
    [Header("Fade")]
    [SerializeField] private Animator _animator;
    [SerializeField] private UISetup _uiSetup;

    private int _roundCounter = 0;

    private CustomCoroutine _initiateRoutine;
    private CustomCoroutine _concludeRoutine;
    private CustomCoroutine _slowMotionRoutine;

    private void Awake()
    {
        if (_player == null || _enemy == null)
            throw new System.NullReferenceException("Player or enemy is null");

        _initiateRoutine = new CustomCoroutine(this, Initiate);
        _concludeRoutine = new CustomCoroutine(this, Conclude);
        _slowMotionRoutine = new CustomCoroutine(this, Impact);

        _battleCamera.Setup(_player.transform, _enemy.transform, this);

        Begin();
    }

    private void FixedUpdate() => _battleCamera.Update();

    private void Begin() => _initiateRoutine?.Start();

    private void End() => _concludeRoutine?.Start();

    private void SlowMotion() => _slowMotionRoutine?.Start();

    private void SetupNextRound()
    {
        _player.transform.position = _playerPossition.position;
        _enemy.transform.position = _enemyPosition.position;

        _player.Restore();
        _enemy.Restore();
    }

    private void ActivateUnit(Unit unit) 
    {
        unit.OnTakeDamage += _battleCamera.Shake;
        unit.OnTakeDamage += SlowMotion;
        unit.OnDeath += End;
        unit.Activate();
    }

    private void DeactivateUnit(Unit unit)
    {
        unit.Deactivate();
        unit.OnDeath -= End;
        unit.OnTakeDamage -= SlowMotion;
        unit.OnTakeDamage -= _battleCamera.Shake;
    }

    private IEnumerator Initiate() 
    {
        yield return new WaitForSeconds(1f);

        _roundCounter = _roundCounter + 1 >= 10 ? 1 : _roundCounter + 1;
        _announcer.AnnounceRound(_roundCounter);

        yield return new WaitForSeconds(1.5f);

        _announcer.Announce(Announcer.AnnouncementType.Battle);
        _uiSetup.Setup(UISetup.UIType.Battle);

        yield return new WaitForSeconds(1f);

        _uiSetup.Setup(UISetup.UIType.Health);

        _player.AttachOpponent(_enemy.transform);
        _enemy.AttachOpponent(_player.transform);

        ActivateUnit(_player);
        ActivateUnit(_enemy);
    }

    private IEnumerator Conclude() 
    {
        _announcer.Announce(Announcer.AnnouncementType.KO);
        _uiSetup.DisableHealth();
        _uiSetup.Setup(UISetup.UIType.KO);

        DeactivateUnit(_player);

        DeactivateUnit(_enemy);

        Time.timeScale = 0.3f;

        yield return new WaitForSecondsRealtime(1.5f);

        Time.timeScale = 1f;

        var victor = _player.IsActive == false ? _enemy : _player;

        victor.Animator.SetTriggerState(AnimationType.Victory);

        yield return new WaitForSeconds(1.5f);

        _animator.SetTrigger("Reset");

        yield return new WaitForSeconds(1f);

        SetupNextRound();

        Begin();
    }

    private IEnumerator Impact() 
    {
        Time.timeScale = 0f;

        yield return new WaitForSecondsRealtime(0.15f);

        Time.timeScale = 1f;
    }
}
