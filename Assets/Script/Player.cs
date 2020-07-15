using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float _SpeedRun;
    private Rigidbody2D _Rigidbody;
    private Animator _Animator;
    
    private bool AttakingPressed = false;

    void Start()
    {
        _Rigidbody = transform.GetComponent<Rigidbody2D>();
        _Animator = transform.GetComponent<Animator>();
    }

    private void Update()
    {
        
    }

    void FixedUpdate()
    {

        if (Input.GetButtonDown("Attack") && !_Animator.GetBool("isAttacking") && !AttakingPressed)
        {
            _Animator.SetTrigger("isAttacking");
            AttakingPressed = true;
        }
        if (Input.GetButtonUp("Attack"))
            AttakingPressed = false;

        float _speed = 0;
        if (Input.GetAxisRaw("Horizontal") > 0 && !_Animator.GetBool("isAttacking"))
        {
            _Animator.SetBool("isRunning", true);
            _speed = _SpeedRun;
        }
        else if (Input.GetAxisRaw("Horizontal") < 0 && !_Animator.GetBool("isAttacking"))
        {
            _Animator.SetBool("isRunning", true);
            _speed = -_SpeedRun;
        }
        else
            _Animator.SetBool("isRunning", false);
        
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
    
}
