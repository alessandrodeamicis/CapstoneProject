using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PlayerLifeController : MonoBehaviour
{
    [SerializeField] private int _currentHp = 20;
    [SerializeField] private int _maxHp = 20;
    [SerializeField] private bool _fullHpOnAwake = true;
    public UnityEvent<int, int> OnHpChanged;
    //private PlayerController _playerController;
    public UnityEvent OnDeath;
    void Awake()
    {
        if (_fullHpOnAwake)
        {
            SetHp(_maxHp);
        }
    }

    private void Start()
    {
        //_playerController = GetComponent<PlayerController>();
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

            if(_currentHp == 0)
            {
                OnDeath?.Invoke();
                //SceneManager.LoadScene("LoseScene");
            }
        }
    }

    public void AddHp(int amount)
    {
        SetHp(_currentHp + amount);
    }
}
