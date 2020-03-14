using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable
{

    private Rigidbody2D _rigid;
    private bool resetJumpNeeded = false;

    [SerializeField]
    private float _jumpForce = 7.0f;

    [SerializeField]
    private float _speed = 5.0f;

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
        Flip(move);

        if (IsGrounded() && Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Space"+Input.GetKeyDown(KeyCode.Space));
            _rigid.velocity = new Vector2(_rigid.velocity.x, _jumpForce);
            StartCoroutine(ResetJumpNeededRoutine());
            _playerAnim.Jump(true);
        }

        _rigid.velocity = new Vector2(move * _speed, _rigid.velocity.y);
        _playerAnim.Move(move);
    }

    bool IsGrounded()
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, Vector2.down, 1.6f, 1 << 20);
        Debug.DrawRay(transform.position, Vector2.down * 1.6f, Color.green);
        Debug.Log("Walking");
        if (hitInfo.collider != null)
        {
            Debug.Log("Here1");
            if (resetJumpNeeded == false)
            {
                Debug.Log("Here2");
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

    //public void AddGems(int amount)
    //{
    //    diamonds += amount;
    //    UIManager.Instance.UpdateGemCount(diamonds);
    //}

    //public void SubtractGems(int amount)
    //{
    //    diamonds -= amount;
    //    UIManager.Instance.UpdateGemCount(diamonds);
    //}
}