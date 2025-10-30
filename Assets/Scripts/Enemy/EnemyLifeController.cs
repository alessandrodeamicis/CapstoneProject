using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class EnemyLifeController : MonoBehaviour
{
    [SerializeField] private int _currentHp = 10;
    [SerializeField] private int _maxHp = 10;
    public UnityEvent<int, int> OnHpChanged;
    public UnityEvent OnEnemyDeath;
    void Awake()
    {
        SetHp(_maxHp);
    }

    public int CurrentHp => _currentHp;

    void SetHp(int hp)
    {
        int oldHp = _currentHp;

        hp = Mathf.Clamp(hp, 0, _maxHp);
        _currentHp = hp;

        if (oldHp != _currentHp)
        {
            OnHpChanged?.Invoke(_currentHp, _maxHp);

            if (_currentHp == 0)
            {
                OnEnemyDeath?.Invoke();
            }
        }
    }

    public void AddHp(int amount)
    {
        SetHp(_currentHp + amount);
    }
}
