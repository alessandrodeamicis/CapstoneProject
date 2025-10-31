using SGM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _speed = 8f;
    [SerializeField] private LayerMask groundLayerMask = 1 << 6;
    [SerializeField] private GameObject _attackPoint;
    [SerializeField] private GameObject _loseUI;
    [SerializeField] private GameObject _pauseUI;
    private Rigidbody2D _rb;
    private Animator _animator;
    private PlayerLifeController _playerLifeController;
    private bool _isGrounded;

    [SerializeField] private float gravity = -9.8f;
    private float verticalSpeed = 0f;

    public float jumpForce = 4f;
    private bool _isJumping = false;
    private bool _canMove = true;
    private bool _isAlive = true;
    public float groundCheckOffset = 1.1f;
    public float groundCheckRadius = .2f;

    public bool IsAlive => _isAlive;
    public bool IsJumping => _isJumping;
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _playerLifeController = GetComponent<PlayerLifeController>();
        _playerLifeController.OnDeath.AddListener(Death);
    }

    void Update()
    {
        if (_isAlive)
        {
            GroundChecker();

            if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow)) && _isGrounded) Jump();

            if (Input.GetKeyDown(KeyCode.P)) Attack();

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (_pauseUI.activeSelf)
                {
                    _pauseUI.SetActive(false);
                }
                else
                {
                    _pauseUI.SetActive(true);
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if (_isAlive && _canMove && S_SaveManager.IsTutorialDone()) Move();
    }

    private void Move()
    {
        float inputX = Input.GetAxis("Horizontal");

        if (!_isGrounded)
            verticalSpeed += gravity * Time.fixedDeltaTime;
        else if (verticalSpeed < 0)
            verticalSpeed = 0;

        Vector2 move = new Vector2(inputX * _speed * Time.fixedDeltaTime, verticalSpeed * Time.fixedDeltaTime);
        _rb.MovePosition(_rb.position + move);

        _animator.SetFloat("moveX", inputX);
        if (inputX < -.1f)
        {
            transform.rotation = Quaternion.Euler(0, -180, 0);
        }
        else if (inputX > .1f)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    private void GroundChecker()
    {
        bool wasGrounded = _isGrounded;
        _isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckOffset, groundLayerMask);
        if (!wasGrounded && _isGrounded && _isJumping) _isJumping = false;
    }

    private void Jump()
    {
        _isJumping = true;

        if (AudioManager.Instance != null) AudioManager.Instance.PlayJumpSound();

        verticalSpeed = jumpForce;

        _animator.SetTrigger("jump");
    }

    private void Attack()
    {
        _animator.SetTrigger("attack");
    }

    public void StartOfAnimationAttackEvent()
    {
        AudioManager.Instance.PlayAttackSound();
        _canMove = false;
    }

    public void StartOfAnimationPunchEvent()
    {
        _attackPoint.SetActive(true);
    }

    public void EndOfAnimationAttackEvent()
    {
        _attackPoint.SetActive(false);
        _canMove = true;
    }

    public void StartOfAnimationHurtEvent()
    {
        _canMove = false;
    }

    public void EndOfAnimationHurtEvent()
    {
        _canMove = true;
    }

    public void StartOfAnimationJumpEvent()
    {
        _isJumping = false;
    }

    public void EndOfAnimationJumpEvent()
    {
        _isJumping = true;
    }

    private void Death()
    {
        _isAlive = false;
        _animator.SetBool("dead", true);

        ShowLoseUI();
    }

    private void ShowLoseUI()
    {
        _loseUI.SetActive(true);
    }
}
