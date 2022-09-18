using UnityEngine;

public enum AttackType 
{
    Light, 
    Heavy, 
    Launcher 
}

[CreateAssetMenu(menuName = "Attacks / Attack", fileName = "AttackBase")]
public class AttackConfig : ScriptableObject
{
    public int Damage => ValidatedValue(_damage);
    public AttackType AttackType => _attackType;

    [SerializeField] private int _damage;
    [SerializeField] private AttackType _attackType;

    private int ValidatedValue(int value) 
    {
        if (value < 0 || value > 3)
            throw new System.ArgumentOutOfRangeException($"{value} less then 0 or greater then 3");

        return value;
    }
}
