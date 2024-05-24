// TileDetection 
// Gestion de la détection des tilemap pour les ennemies leur permettant de sauter et de se mouvoir dans la map.
// Auteurs: Henrick Baril
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileDetection : MonoBehaviour
{
    [Header("Target to chase")]
    [SerializeField] private float _AttackDistance;
    [Header("Jump settings")]
    [SerializeField] private float _JumpHeight;
    [Header("Parent of the Core")]
    [SerializeField] public Transform _Parent;

    private Rigidbody2D _ParentBody;
    private bool _CanJump;
    private bool _IsTileGround;
    private bool _EstEnAttack = false;
    private Vector2 _DirectionAbsolute;
    private int layerMask;
    public Vector2 attackd;
    public bool etaitAttack;
    private Transform _Cible;

    void Start()
    {
        layerMask = LayerMask.GetMask(new[] {"Ground"});
        _ParentBody = _Parent.GetComponent<Rigidbody2D>();
        attackd = new Vector2(_AttackDistance, _AttackDistance);
        _Cible = GameObject.FindGameObjectWithTag("Player").transform;
        //Debug.Log(_Cible);
    }

    void Update()
    {
        TileDetections();
        CanJump();
        JumpPhysics();
       
        //if (PlayerDetection()) AttackPlayer();
    }

    public bool PlayerDetection() {
        etaitAttack = _EstEnAttack;
        //Player and ennemie distance
        Vector2 distance = _Cible.position - this.gameObject.transform.position;
        //Debug.Log(distance);
        return LtEq(distance, attackd) && GtEq(distance, attackd * -1.0f);
    }

    public static bool LtEq(Vector2 first, Vector2 second) { 
        if (first.x <= second.x) if (first.y <= second.y) return true;
        return false;
    }

    public static bool GtEq(Vector2 first, Vector2 second)
    {
        if (first.x >= second.x) if (first.y >= second.y) return true;
        return false;
    }

    public bool TileDetections() {
        _DirectionAbsolute = DirectionAbsolution();
        //Vérif de la hitbox de saut selon la direction du monstre
        Debug.DrawRay(this.gameObject.transform.position, _DirectionAbsolute * 0.6f, _IsTileGround ? Color.red : Color.gray);
        return _IsTileGround = Physics2D.Raycast(transform.position, _DirectionAbsolute, 0.6f, layerMask);
    }

    public bool CanJump()
    {
        //Vérif de la possibilité de sauter
        Debug.DrawRay(this.gameObject.transform.position, Vector2.down * 0.3f, _CanJump ? Color.red : Color.gray);
        return _CanJump = Physics2D.Raycast(transform.position, Vector2.down, 0.3f, layerMask);
    }

    public Vector2 DirectionAbsolution() {
        //Absolution de la direction du slime pour la vérif de la hitbox de saut
        if (_ParentBody.velocityX < -0.01f) _DirectionAbsolute = Vector2.left;
        else if (_ParentBody.velocityX > 0.01f) _DirectionAbsolute = Vector2.right;
        return _DirectionAbsolute;
    }

    public void JumpPhysics() {
        //Meilleur physique de jump
        if (_ParentBody.velocity.y < 0) _ParentBody.velocity += Vector2.up * Physics.gravity.y * (3.0f - 1) * Time.deltaTime;
        if (TileDetections() && CanJump()) _ParentBody.velocity = new Vector2(0, Mathf.Sqrt(-2.5f * Physics2D.gravity.y * _JumpHeight-1));
    }

    public void AttackPlayer()
    {
        _DirectionAbsolute = DirectionAbsolution();
        if (_CanJump) {
            Debug.Log(_ParentBody.velocity);
            _ParentBody.velocity = new Vector2(0, Mathf.Sqrt(-2.0f * Physics2D.gravity.y * _JumpHeight));
        }
    }
}
