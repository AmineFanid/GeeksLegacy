using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

public class TileDetection : MonoBehaviour
{
    [Header("Target to chase")]
    [SerializeField] public Transform _Cible;
    [Header("Jump settings")]
    [SerializeField] private float _JumpHeight;
    [Header("Parent of the Core")]
    [SerializeField] public Transform _Parent;
    [Header("Settings")]
    [SerializeField] private float _DistanceVision = 5.0f;

    private Rigidbody2D _ParentBody;
    private bool _CanJump = true;
    private bool _IsTileGround;
    private bool _IsPlayer;
    private Vector2 _DirectionAbsolute;
    private int layerMask;
    private float _DistanceAttackPlayer;

    void Start()
    {
        layerMask = LayerMask.GetMask(new[] {"Ground" });
        _ParentBody = _Parent.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        TileDetections();
        CanJump();

        //Dessin des raycast pour le Debug
        Debug.DrawRay(this.gameObject.transform.position, Vector2.down, _CanJump ? Color.red : Color.gray);
        Debug.DrawRay(this.gameObject.transform.position, _DirectionAbsolute, _IsTileGround ? Color.yellow : Color.cyan);

        JumpPhysics();
    }
     
    public void TileDetections() {
        _DirectionAbsolute = DirectionAbsolution();

        //Vérif de la hitbox de saut selon la direction du monstre
        _IsTileGround = Physics2D.Raycast(transform.position, _DirectionAbsolute, 0.6f, layerMask);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("ENTER");
        _IsPlayer = LayerMask.GetMask("Player") == collision.gameObject.layer;
        if (_IsPlayer)
        {
            CanJump();
            JumpPhysics();
        }
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        
    }

    public void CanJump()
    {
        //Vérif de la possibilité de sauter
        _CanJump = Physics2D.Raycast(transform.position, Vector2.down, 0.4f, layerMask);
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
        if ((_IsTileGround) && _CanJump) _ParentBody.velocity = new Vector2(0, Mathf.Sqrt(-2.0f * Physics2D.gravity.y * _JumpHeight));
    }
}
