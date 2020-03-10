using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable
{

    private Rigidbody2D _rigid;
    private bool resetJumpNeeded = false;

    [SerializeField]
    private float _jumpForce = 5.0f;

    [SerializeField]
    private float _speed = 5.0f;

    private bool _grounded = false;

    private PlayerAnimation _playerAnim;
    private SpriteRenderer _playerSprite;

    public int Health { get; set; }
    public int diamonds;

    void Start()
    {
        Health = 4;
        _rigid = GetComponent<Rigidbody2D>();
        _playerAnim = GetComponent<PlayerAnimation>();
        _playerSprite = GetComponentInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        if (Input.GetMouseButtonDown(0) && IsGrounded() == true)
        {
            _playerAnim.Attack();
        }
    }

    void Movement()
    {
        float move = Input.GetAxisRaw("Horizontal");
        _grounded = IsGrounded();
        Flip(move);

        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            _rigid.velocity = new Vector2(_rigid.velocity.x, _jumpForce);
            StartCoroutine(ResetJumpNeededRoutine());
            _playerAnim.Jump(true);
        }

        _rigid.velocity = new Vector2(move * _speed, _rigid.velocity.y);
        _playerAnim.Move(move);
    }

    bool IsGrounded()
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, Vector2.down, 0.6f, 1 << 8);
        Debug.DrawRay(transform.position, Vector2.down * 0.6f, Color.green);

        if (hitInfo.collider != null)
        {
            if (resetJumpNeeded == false)
            {
                _playerAnim.Jump(false);
                return true;
            }
        }
        return false;
    }

    void Flip(float move)
    {
        if (move > 0)
        {
            _playerSprite.flipX = false;
        }
        else if (move < 0)
        {
            _playerSprite.flipX = true;
        }

    }

    IEnumerator ResetJumpNeededRoutine()
    {
        resetJumpNeeded = true;
        yield return new WaitForSeconds(0.1f);
        resetJumpNeeded = false;
    }

    public void Damage()
    {
        if (Health < 1)
        {
            return;
        }
        Health--;
        UIManager.Instance.UpdateLives(Health);

        if (Health < 1)
        {
            _playerAnim.Death();
        }
    }

    public void AddGems(int amount)
    {
        diamonds += amount;
        UIManager.Instance.UpdateGemCount(diamonds);
    }

    public void SubtractGems(int amount)
    {
        diamonds -= amount;
        UIManager.Instance.UpdateGemCount(diamonds);
    }
}