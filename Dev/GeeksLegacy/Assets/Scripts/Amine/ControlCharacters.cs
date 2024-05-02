using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;
using Debug = UnityEngine.Debug;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]

public class ControlCharacters : MonoBehaviour
{

    private Animator _Animator;
    private Rigidbody2D _Rigidbody;

    private float ControleX;
    [Header("Character Settings")]
    [SerializeField] private float _JumpHeight = 2.0f;
    [SerializeField] private float _Speed = 5.0f;
    [SerializeField] private float _JumpForce = 2.0f;
    [SerializeField] private float _GravityScale = 1.0f;
    [SerializeField] private float _FallingGravityScale = 10.0f;
    [SerializeField] private float _MaxSpeed = 7.0f;
    [SerializeField] private float _PlayerLife = 100.0f;
    [SerializeField] private float _KnockBack = 4.0f;
    [SerializeField] public float _KBCounter = 0.0f;
    [SerializeField] public float _KBTotalTime = 1.0f;
    [SerializeField] public float _InvCounter = 0.0f;
    [SerializeField] public float _InvTotalTime = 1.0f;

    private int _MaxJump = 2;
    private bool _HasCollided;
    private int _NumJump = 0;
    private Vector2 movement;
    private IEnumerator _Invincibility;
    public float _CurrentLife;
    public Player player;
    public CharacterInventory inventory;
    public bool knockRight;

    private bool _isGrounded;


    public void Start() {
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Ennemies"), this.gameObject.layer, false);
        _Animator = GetComponent<Animator>();
        _Rigidbody = GetComponent<Rigidbody2D>();
        inventory = new CharacterInventory();
        player = new Player(_PlayerLife, inventory);
    }

    public void Update()
    {
        ControleX = Input.GetAxis("Horizontal");

        // Move horizontally    
        movement = new Vector2(ControleX, 0f);

        _Rigidbody.AddForce(movement * _Speed);

        // Saute
        if (Input.GetButtonDown("Jump"))
        {
            if(_NumJump < _MaxJump)
            {
                _Rigidbody.AddForce(Vector2.up * _JumpHeight, (ForceMode2D)ForceMode.Impulse);
                _NumJump++;
            }
        }

        if (_Rigidbody.velocityY < 0)
            _Rigidbody.gravityScale = _FallingGravityScale;
        else if (_Rigidbody.velocityY >= 0)
            _Rigidbody.gravityScale = _GravityScale;
    }

    public void FixedUpdate()
    {
        if (_KBCounter <= 0)
        {

            Debug.Log("no hit");
            int groundLayerMask = LayerMask.GetMask("Ground");
            _isGrounded = Physics2D.Raycast(transform.position, Vector2.down, 0.9f, groundLayerMask);

            if ((_NumJump >= _MaxJump) && _isGrounded)
            {
                _NumJump = 0;
            }

            float vitesse = _Rigidbody.velocityX;
            bool vitesseBouge = vitesse > 0.01f || vitesse < -0.01f;
            _Animator.SetBool("IsMoving", vitesseBouge);
            if (vitesseBouge) //Si l'ennemi bouge
            {
                //Active ses animations en fonction du mouvement
                Vector2 directionAssainie = ForceAnimationVirtualJoystick.ForceDirectionAxe(movement);
                _Animator.SetFloat("MouvementX", directionAssainie.x);
                _Animator.SetFloat("MouvementY", directionAssainie.y);
            }

            //V�rification pour limit� la vitesse de l'ennemi
            if (_Rigidbody.velocity.x >= _MaxSpeed)
                _Rigidbody.velocity = new Vector2(_MaxSpeed, _Rigidbody.velocity.y);
            if (_Rigidbody.velocity.x <= _MaxSpeed * -1)
                _Rigidbody.velocity = new Vector2(_MaxSpeed * -1, _Rigidbody.velocity.y);
        }
        else {
            if (knockRight)
                _Rigidbody.velocity = new Vector2(-_KnockBack, _KnockBack/2);
            else
                _Rigidbody.velocity = new Vector2(_KnockBack, _KnockBack/2);
            _KBCounter -= Time.deltaTime;

        }

        if (_InvCounter <= 0)
        {
            Debug.Log("normal");
            _Animator.SetBool("GetHit", false);
        }
        else
        {
            _Animator.SetBool("GetHit", true);
            _InvCounter -= Time.deltaTime;
        }


    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ennemies")) {
            IEnumerator Inv = Invincibility(collision);
        }
        
    }

    IEnumerator Invincibility(Collision2D collision)
    {
        Debug.Log("La coroutine a d�marr�.");
        Physics2D.IgnoreCollision(this.gameObject.GetComponent<Collider2D>(), collision.collider, true);
        // Attendre pendant 3 secondes
        yield return new WaitForSeconds(1.0f);
        Physics2D.IgnoreCollision(this.gameObject.GetComponent<Collider2D>(), collision.collider, false);
        Debug.Log("La coroutine a termin� apr�s avoir attendu 3 secondes.");
    }
}



public class Player
{
    public float lifePoint;
    public CharacterInventory inventory;

    public Player(float lifePoint, CharacterInventory inventory)
    {
        this.lifePoint = lifePoint;
        this.inventory = inventory;
    }

    public float GetLifePoint() {
        return this.lifePoint;
    }

    public void TakeDamage(float damage) {       
        this.lifePoint -= damage;
    }
}
