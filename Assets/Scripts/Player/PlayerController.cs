using SGM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _speed = 8f;
    [SerializeField] private LayerMask groundLayerMask = 1 << 6;
    [SerializeField] private LayerMask _enemiesLayer = 1 << 7;
    [SerializeField] private GameObject _attackPoint;
    private SpriteRenderer _spriteRenderer;
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
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
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
        }
    }

    private void FixedUpdate()
    {
        if (_isAlive && _canMove) Move();
    }

    private void Move()
    {
        float inputX = Input.GetAxis("Horizontal");
        //Vector2 moveX = new Vector2(inputX * _speed * Time.fixedDeltaTime, transform.position.y);
        //_rb.MovePosition(_rb.position + moveX);
        //Vector2 targetPosition = new Vector2(
        //    _rb.position.x + inputX * _speed * Time.fixedDeltaTime,
        //    _rb.position.y
        //);

        //_rb.MovePosition(targetPosition);

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

        //_rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        verticalSpeed = jumpForce;

        _animator.SetTrigger("jump");
    }

    private void Attack()
    {
        _animator.SetTrigger("attack");
    }

    public void StartOfAnimationAttackEvent()
    {
        //StartCoroutine(DisableMove());
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

    private void Death()
    {
        _isAlive = false;
        _animator.SetBool("dead", true);
    }

    IEnumerator DisableMove()
    {
        _canMove = false;
        Debug.Log(_canMove);
        yield return new WaitForSeconds(2f);
        _canMove = true;
        Debug.Log(_canMove);
    }
}
