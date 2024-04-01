using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]

public class ControlCharacters : MonoBehaviour
{

    private Animator AnimateurRobin;
    private Rigidbody2D Rigidbody;

    private float ControleX;
    private float ControleY;
    [Header("Character Settings")]

    //[SerializeField] private float _Hp = 100.0f;
    [SerializeField] private float _Speed = 5.0f;
    [SerializeField] private float _JumpForce = 2.0f;

    //private bool _Vivant = true;


    private bool _isGrounded;


    public void Start() {

        AnimateurRobin = GetComponent<Animator>();
        Rigidbody = GetComponent<Rigidbody2D>();
    }

    public void Update()
    {

        ControleX = Input.GetAxis("Horizontal");
        ControleY = Mathf.Max(0, Input.GetAxis("Vertical"));

        //Vector3 direction = new Vector3(ControleX, (ControleY * 2), 0);
        //transform.Translate(direction * _Speed * Time.deltaTime);


        // Move horizontally    
        Vector2 movement = new Vector2(ControleX, 0f) * _Speed * Time.deltaTime;
        transform.Translate(movement);

        // Check si le joueur est au sol
        _isGrounded = Physics2D.Raycast(transform.position, Vector2.down, 0.1f);

        // Saute
        if (Input.GetButtonDown("Jump") && _isGrounded)
        {
            Rigidbody.AddForce(Vector2.up * _JumpForce, ForceMode2D.Impulse);
        }

        // Pour la gestion de la vie du personnage
       /* if(_Hp == 0.0f)
        {
            _Vivant == false;
        }
        if (!_Vivant)
        {
            Destroy(this.gameObject);
        }*/
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        // exemple de dommage suite à un contact avec un ennemi

        //if (collision.gameObject.layer == LayerMask.NameToLayer("Ennemis")){
        //  _Hp -= (_Hp * 1 / 100);
        //}
    }
}
