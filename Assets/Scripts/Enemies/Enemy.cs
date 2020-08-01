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
    public Animator _Animator;

    [SerializeField] public float LifeScore = 100f;
    [SerializeField] private float DemageSwordAttack = 50f;

    public bool _right = true;

    void Start()
    {
        _Rigidbody = transform.GetComponent<Rigidbody2D>();
        _Animator = transform.GetComponent<Animator>();
        _Animator.Play("Empty");
    }


    bool _isIlive = false;
    bool _wasBorn = false;
    void Update()
    {
        if (_wasBorn && !_isIlive)
            _Animator.SetBool("isBorning", true);
        if (LifeScore <= 0 && !_Animator.GetBool("isDeading"))
            _Animator.SetBool("isDeading", true);
    }

    private void FixedUpdate()
    {
        if (_isDamaging) _Animator.Play("EnemyDamage", 0);
        else this.GuardMode();

        this.CheckGrounded();
        this.Flip();
    }

    private void OnBecameVisible()
    {
        _waitGuardMode = true;
        Invoke("SetGuardModeOn", 1);
    }
    
    private void OnBecameInvisible()
    {
        Invoke("SetGuardModeOff", 1);
    }

    bool _waitGuardMode = false;
    private void SetGuardModeOff()
    {
        if (!_waitGuardMode)
            _isGuardMode = false;
    }

    private void SetGuardModeOn()
    {
        if (!_isIlive)
            _wasBorn = true;
        _isGuardMode = true;
        _waitGuardMode = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "PlayerSword")
            this.TakeDamage();
    }

    bool _isDamaging = false;
    float _timeDamage = 1;
    public void TakeDamage(){
        this.LifeScore -= DemageSwordAttack;
        _Rigidbody.velocity = new Vector2(0, _Rigidbody.velocity.y);
        _timeDamage = 0;

        if (LifeScore > 0)
        {
            _isDamaging = true;
            _Rigidbody.AddForce(new Vector2(100f, 100f));
        }
    }

    public void EndDamage()
    {
        _isDamaging = false;
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

        if (_Animator.GetBool("isDeading") || !_isGuardMode)
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
