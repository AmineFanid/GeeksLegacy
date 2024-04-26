using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]

public class ControlCharacters : MonoBehaviour
{

    private Animator AnimateurPlayer;
    private Rigidbody2D Rigidbody;

    private float ControleX;
    private float ControleY;
    [Header("Character Settings")]

    [SerializeField] private float _Speed = 5.0f;
    [SerializeField] private float _JumpForce = 2.0f;

    private int _MaxJump = 2;
    private int _NumJump = 0;
    public Player player;
    public CharacterInventory inventory;

    private bool _isGrounded;


    public void Start() {

        AnimateurPlayer = GetComponent<Animator>();
        Rigidbody = GetComponent<Rigidbody2D>();
        inventory = new CharacterInventory();
        player = new Player(1000.0f, inventory);

    }

    public void Update()
    {

        ControleX = Input.GetAxis("Horizontal");
        ControleY = Mathf.Max(0, Input.GetAxis("Vertical"));

        // Move horizontally    
        Vector2 movement = new Vector2(ControleX, 0f) * _Speed * Time.deltaTime;
        transform.Translate(movement);


        // Saute
        if (Input.GetButtonDown("Jump"))
        {
            if(_NumJump < _MaxJump)
            {
                Rigidbody.AddForce(Vector2.up * _JumpForce, ForceMode2D.Impulse);
                _NumJump++;
            }
        }


        // Pour la gestion de la vie du personnage
        /* 
         if(_Hp == 0.0f)
         {
             _Vivant == false;
         }
         if (!_Vivant)
         {
             Destroy(this.gameObject);
         }
        */
    }

    public void FixedUpdate()
    {
        int groundLayerMask = LayerMask.GetMask("Ground"); 
        _isGrounded = Physics2D.Raycast(transform.position, Vector2.down, 0.9f, groundLayerMask);

        if ((_NumJump >= _MaxJump) && _isGrounded)
        {
            _NumJump = 0;
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        // exemple de dommage suite à un contact avec un ennemi

        //if (collision.gameObject.layer == LayerMask.NameToLayer("Ennemis")){
        //  _Hp -= (_Hp * 1 / 100);
        //}
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



}
