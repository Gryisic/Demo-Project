using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image _healthImage;

    public void UpdateFillAmount(int maxHealth, int health) => 
        _healthImage.fillAmount = (float)health / (float)maxHealth;
}
