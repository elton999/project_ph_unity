using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float m_speedRun;
    [SerializeField] private Transform m_CheckGrounded;
    [SerializeField] private Transform m_CheckWall;
    [SerializeField] private LayerMask m_groundLayer;
    [SerializeField] private float m_checkRadio;

    Rigidbody2D _Rigidbody;
    Animator _Animator;

    public bool _right = true;

    void Start()
    {
        transform.GetComponent<SpriteRenderer>().sprite = null;
        _Rigidbody = transform.GetComponent<Rigidbody2D>();
        _Animator = transform.GetComponent<Animator>();
    }


    bool _isIlive = false;
    bool _wasBorn = false;
    void Update()
    {
        if (_wasBorn && !_isIlive)
            _Animator.SetBool("isBorning", true);    
    }

    private void FixedUpdate()
    {
        this.GuardMode();
        this.CheckGrounded();
        this.Flip();
    }
    
    private void OnBecameVisible()
    {
        _isGuardMode = true;
        if (!_isIlive)
            _wasBorn = true;
    }

    private void OnBecameInvisible()
    {
        _isGuardMode = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "PlayerSword")
            _Animator.SetBool("isDeading", true);
    }

    bool _isGuardMode;
    void GuardMode()
    {
        if (_isGuardMode && _isIlive && !_Animator.GetBool("isDeading"))
        {
            if (_right)
                _Rigidbody.velocity = new Vector2(m_speedRun, _Rigidbody.velocity.y);
            else
                _Rigidbody.velocity = new Vector2(-m_speedRun, _Rigidbody.velocity.y);
        }

        if (_Animator.GetBool("isDeading"))
            _Rigidbody.velocity = new Vector2(0, _Rigidbody.velocity.y);
    }
    
    void CheckGrounded()
    {
        if (!Physics2D.OverlapCircle(m_CheckGrounded.position, m_checkRadio, m_groundLayer) || 
            !Physics2D.OverlapCircle(m_CheckWall.position, m_checkRadio, m_groundLayer))
            _right = !_right;
    }
    
    void Flip()
    {
        if (_right)
            transform.localScale = new Vector3(1, 1, 1);
        else
            transform.localScale = new Vector3(-1, 1, 1);
    }

    void SetIliveEnemy()
    {
        _isIlive = true;
    }

    void DestroyGameObject()
    {
        Destroy(gameObject);
    }
}
