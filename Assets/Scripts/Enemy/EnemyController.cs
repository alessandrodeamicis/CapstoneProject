using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float _minDist = 1.4f;
    [SerializeField] private float _speed = 6f;
    [SerializeField] private GameObject _attackPoint;
    [SerializeField] private GameObject _lifePanel;
    private GameObject _player;
    private PlayerController _playerController;
    private Rigidbody2D _rb;
    private Animator _animator;
    private EnemyLifeController _enemyLifeController;
    private SpriteRenderer _spriteRenderer;
    private bool _isNearTheTarget;
    private bool _isAlive = true;
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _player = GameObject.FindGameObjectWithTag("Player");
        _playerController = _player.GetComponent<PlayerController>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _enemyLifeController = GetComponent<EnemyLifeController>();
        _enemyLifeController.OnEnemyDeath.AddListener(Death);
    }

    void Update()
    {
        if (_isAlive)
        {
            _isNearTheTarget = Vector2.Distance(transform.position, _player.transform.position) < _minDist;

            CheckAnimations();
        }
    }

    private void FixedUpdate()
    {
        if (_playerController.IsAlive && _isAlive)
        {
            CheckDirection();

            if (!_isNearTheTarget)
            {
                Move();
            }
        }
    }

    private void CheckDirection()
    {
        if (transform.position.x > _player.transform.position.x)
        {
            transform.rotation = Quaternion.Euler(0, -180, 0);
        }
        else if (transform.position.x < _player.transform.position.x)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        _attackPoint.transform.rotation = Quaternion.identity;
        _lifePanel.transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    private void Move()
    {
        transform.position = Vector2.MoveTowards(transform.position, new Vector2(_player.transform.position.x, transform.position.y), _speed * Time.deltaTime);
    }

    public void StartOfAnimationAttackEvent()
    {
        _attackPoint.SetActive(true);
    }

    public void EndOfAnimationAttackEvent()
    {
        _attackPoint.SetActive(false);
    }

    private void CheckAnimations()
    {
        _animator.SetBool("isMoving", !_isNearTheTarget && _playerController.IsAlive);
        _animator.SetBool("isAttacking", _isNearTheTarget && _playerController.IsAlive);
    }

    private void Death()
    {
        _isAlive = false;
        _animator.SetBool("dead", true);
        PolygonCollider2D attackPointCollider = _attackPoint.GetComponent<PolygonCollider2D>();
        attackPointCollider.enabled = false;
    //StartCoroutine(DisableAnimatorAfterDeath());
}
}
