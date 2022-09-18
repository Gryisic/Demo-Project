using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class UISetup 
{
    public enum UIType 
    {
        Battle,
        KO,
        Health
    }

    [SerializeField] private Image _battleImage;
    [SerializeField] private Image _koImage;
    [SerializeField] private GameObject _playerHealth;
    [SerializeField] private GameObject _enemyHealth;

    private Animator _playerHealthAnimator;
    private Animator _enemyHealthAnimator;

    public void Setup(UIType type) 
    {
        if (_playerHealthAnimator == null && _enemyHealthAnimator == null) 
        {
            if (_playerHealth.TryGetComponent(out Animator playerAnimator))
            {
                _playerHealthAnimator = playerAnimator;
            }

            if (_enemyHealth.TryGetComponent(out Animator enemyAnimator))
            {
                _enemyHealthAnimator = enemyAnimator;
            }
        }

        switch (type)
        {
            case UIType.Battle:
                ReActivateGameObject(_battleImage.gameObject);
                break;

            case UIType.KO:
                ReActivateGameObject(_koImage.gameObject);
                break;

            case UIType.Health:
                ReActivateGameObject(_playerHealth.gameObject);
                ReActivateGameObject(_enemyHealth.gameObject);
                break;
        }
    }

    public void DisableHealth() 
    {
        _playerHealthAnimator.SetTrigger("Reset");
        _enemyHealthAnimator.SetTrigger("Reset");
    }

    private void ReActivateGameObject(GameObject gameObject) 
    {
        gameObject.SetActive(false);
        gameObject.SetActive(true);
    }
}
