using System;
using System.Collections;
using UnityEngine;
using static UnityEditor.FilePathAttribute;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]

public class ControlCharacters : MonoBehaviour
{
    [Header("Character Settings")]
    [Header("Jump Settings")]
    [SerializeField] private float _JumpHeight = 2.0f;
    [SerializeField] private int _MaxJump = 2;
    [SerializeField] private float _GravityScale = 1.0f;
    [SerializeField] private float _FallingGravityScale = 10.0f;
    [Header("Speed Settings")]
    [SerializeField] private float _Speed = 5.0f;
    [SerializeField] private float _MaxSpeed = 7.0f;
    [SerializeField] private float _MaxYSpeed = 20.0f;
    [Header("Player Interactions")]
    [SerializeField] private float _PlayerLife = 100.0f;
    [SerializeField] private float _KnockBack = 4.0f;
    [SerializeField] public float kBCounter = 0.0f;
    [SerializeField] public float kBTotalTime = 1.0f;

    public IEnumerator invincibility;
    private IEnumerator _AttackingAnimation;
    public Player player;
    public CharacterInventory inventory;
    public bool knockRight;
    private int _NumJump = 0;
    private Vector2 _Movement;
    private bool _isGrounded;
    private Animator _Animator;
    private Rigidbody2D _Rigidbody;
    private float _ControleX;
    private int playerLayer;
    private int ennemiesLayer;
    private Rigidbody2D _ToolRigidBody;

    public void Start() {
        _Animator = GetComponent<Animator>();
        _Rigidbody = GetComponent<Rigidbody2D>();
        inventory = new CharacterInventory();
        player = new Player(_PlayerLife, inventory);
        playerLayer = LayerMask.NameToLayer("Player");
        ennemiesLayer = LayerMask.NameToLayer("Ennemies");
        _ToolRigidBody = this.gameObject.GetComponentInChildren<Rigidbody2D>();
    }

    public void Update()
    {
        //Vérif pour l'animation d'attaque
        MousePosition();

        //Mouvement horizontal
        _ControleX = Input.GetAxis("Horizontal");  
        _Movement = new Vector2(_ControleX, 0f);
        _Rigidbody.AddForce(_Movement * _Speed);

        //Mouvement vertical
        if (Input.GetButtonDown("Jump"))
        {
            if(_NumJump < _MaxJump)
            {
                _Rigidbody.AddForce(Vector2.up * _JumpHeight, (ForceMode2D)ForceMode.Impulse);
                _NumJump++;
            }
        }

        //Vérification du maximum de vitesse
        MaxingSpeed();
    }

    public void FixedUpdate()
    {
        //Vérification de vitesse
        GravitiCheck();

        //Vérification de KnockBack
        KnockBackVerif();
    }

    public void MousePosition() {
        bool IsRight = false;
        if (Input.GetKeyUp(KeyCode.Mouse0)) {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (mousePosition.x > this.gameObject.transform.localPosition.x) {
                IsRight = true;
            }
            if (!_Animator.GetBool("Attacking"))
            {
                _AttackingAnimation = AttackingAnimation(IsRight);
                StartCoroutine(_AttackingAnimation);
            }
        }
    }
    private IEnumerator AttackingAnimation(bool IsRight) //Attacking
    {
        _Animator.SetBool("FaceRight", IsRight);
        _Animator.SetBool("Attacking", true);
        yield return new WaitForSeconds(0.35f);
        _Animator.SetBool("Attacking", false);
        _Animator.SetBool("FaceRight", false);
        StopCoroutine(_AttackingAnimation);
    }

    public void KnockBack()
    {
        if (knockRight)
            _Rigidbody.velocity = new Vector2(-_KnockBack, _KnockBack / 2);
        else
            _Rigidbody.velocity = new Vector2(_KnockBack, _KnockBack / 2);
    }

    public void KnockBackVerif()
    {
        if (kBCounter <= 0)
        {
            _Animator.SetBool("GetHit", false);
            Physics2D.IgnoreLayerCollision(playerLayer, ennemiesLayer, false);

            int groundLayerMask = LayerMask.GetMask("Ground");
            _isGrounded = Physics2D.Raycast(transform.position, Vector2.down, 0.9f, groundLayerMask);

            if ((_NumJump >= _MaxJump) && _isGrounded)
                _NumJump = 0;

            MouvementAnimation();
        }
        else
        {
            _Animator.SetBool("GetHit", true);
            Physics2D.IgnoreLayerCollision(playerLayer, ennemiesLayer, true);

            KnockBack();
            kBCounter -= Time.deltaTime;
        }
    }

    public void MouvementAnimation()
    {
        float vitesse = _Rigidbody.velocityX;
        bool vitesseBouge = vitesse > 0.1f || vitesse < -0.1f;
        _Animator.SetBool("IsMoving", vitesseBouge);
        if (vitesseBouge)
        {
            //Active ses animations en fonction du mouvement
            Vector2 directionAssainie = ForceAnimationVirtualJoystick.ForceDirectionAxe(_Movement);
            _Animator.SetFloat("MouvementX", directionAssainie.x);
            _Animator.SetFloat("MouvementY", directionAssainie.y);
        }
    }

    public Vector2 GetDirectionPersonnage() {
        Vector2 directionAssainie = ForceAnimationVirtualJoystick.ForceDirectionAxe(_Movement);
        return directionAssainie;
    }

    public void MaxingSpeed()
    {
        //Vérification pour limité la vitesse en x positif
        if (_Rigidbody.velocity.x >= _MaxSpeed)
        {
            _Rigidbody.velocity = new Vector2(_MaxSpeed, _Rigidbody.velocity.y);
            //Vérification pour limité la vitesse en y négatif
            if (_Rigidbody.velocity.y <= _MaxYSpeed * -1)
                _Rigidbody.velocity = new Vector2(_MaxSpeed, _MaxYSpeed * -1);
        }
        //Vérification pour limité la vitesse en x négatif
        if (_Rigidbody.velocity.x <= _MaxSpeed * -1)
        {
            _Rigidbody.velocity = new Vector2(_MaxSpeed * -1, _Rigidbody.velocity.y);
            //Vérification pour limité la vitesse en y négatif
            if (_Rigidbody.velocity.y <= _MaxYSpeed * -1)
                _Rigidbody.velocity = new Vector2(_MaxSpeed * -1, _MaxYSpeed * -1);
        }
        //Vérification pour limité la vitesse en y négatif
        if (_Rigidbody.velocity.y <= _MaxYSpeed * -1)
            _Rigidbody.velocity = new Vector2(_Rigidbody.velocity.x, _MaxYSpeed * -1);
    }

    public void GravitiCheck()
    {
        if (_Rigidbody.velocity.y >= -0.001f)
            _Rigidbody.gravityScale = _GravityScale;
        else
            _Rigidbody.gravityScale += _FallingGravityScale;
    }

    public Player findPlayerObject()
    {
        return player;
    }
}

public class Player
{
    private float _LifePoint;
    private CharacterInventory _Inventory;

    public Player(float lifePoint, CharacterInventory inventory)
    {
        this._LifePoint = lifePoint;
        this._Inventory = inventory;
    }

    public float GetLifePoint()
    {
        return this._LifePoint;
    }        
    
    public void SetLifePoint(float NewLifePoint)
    {
        this._LifePoint = NewLifePoint;
    }    
    
    public CharacterInventory GetPlayerInventory()
    {
        return this._Inventory;
    }
    
    public void SetPlayerInventory(CharacterInventory NewInventory)
    {
        this._Inventory = NewInventory;
    }    

    public void TakeDamage(float damage)
    {
        this._LifePoint -= damage;
        EventManager.TriggerEvent(EventManager.PossibleEvent.eVieJoueurChange, this._LifePoint);
    }
}
