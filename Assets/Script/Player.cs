using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float m_speedRun;
    [SerializeField] private float m_speedJump;
    [SerializeField] private float m_jumpForce = 0.5f;
    private Rigidbody2D _Rigidbody;
    private Animator _Animator;
    
    private bool AttakingPressed = false;
    private bool JumpPressed = false;

    void Start()
    {
        _Rigidbody = transform.GetComponent<Rigidbody2D>();
        _Animator = transform.GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Attack") && !_Animator.GetBool("isAttacking") && !AttakingPressed)
        {
            _Animator.SetTrigger("isAttacking");
            AttakingPressed = true;
        }
        if (Input.GetButtonUp("Attack"))
            AttakingPressed = false;

        _speed = 0;
        if (Input.GetAxisRaw("Horizontal") > 0 && !_Animator.GetBool("isAttacking"))
            this.RunAnimation(m_speedRun);
        else if (Input.GetAxisRaw("Horizontal") < 0 && !_Animator.GetBool("isAttacking"))
            this.RunAnimation(-m_speedRun);
        else
            _Animator.SetBool("isRunning", false);

        if (Input.GetButtonDown("Jump") && _grounded && !JumpPressed)
        {
            _Rigidbody.AddForce(new Vector2(0, m_jumpForce), ForceMode2D.Impulse);
            JumpPressed = true;
        }
        if (Input.GetButtonUp("Jump"))
            JumpPressed = false;

        
        this.CheckGround();
    }

    void RunAnimation(float speed)
    {
        if (speed > 0)
            if (_Animator.GetBool("isJumping"))
                _speed = m_speedJump;
            else
                _speed = m_speedRun;
        else if (speed < 0)
            if (_Animator.GetBool("isJumping"))
                _speed = -m_speedJump;
            else
                _speed = -m_speedRun;
            

        if (!_Animator.GetBool("isJumping"))
            _Animator.SetBool("isRunning", true);
        else
            _Animator.SetBool("isRunnig", false);
    }

    private float _speed;
    void FixedUpdate()
    {   
        _Rigidbody.velocity = new Vector2(_speed, _Rigidbody.velocity.y);

        this.Flip();
    }
    
    void Flip()
    {
        if (_Rigidbody.velocity.x > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else if (_Rigidbody.velocity.x < 0)
            transform.localScale = new Vector3(-1, 1, 1);
    }


    [SerializeField] private float m_distanceCollision = 0.5f;
    [SerializeField] private LayerMask m_groundLayer;

    bool _grounded = false;
    void CheckGround()
    {
        _grounded = false;
        Vector2 position = transform.position;
        Vector2 directionDown = new Vector2(0, -m_distanceCollision);

        Debug.DrawRay(position, directionDown, Color.green);

        RaycastHit2D hit = Physics2D.Raycast(position, directionDown, m_distanceCollision, m_groundLayer);
        if (hit.collider != null)
        {
            _grounded = true;
        }
        _Animator.SetBool("isJumping", !_grounded);
    }
    
}
