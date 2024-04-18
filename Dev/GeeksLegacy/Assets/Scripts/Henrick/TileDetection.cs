using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

public class TileDetection : MonoBehaviour
{
    [Header("Jump settings")]
    [SerializeField] private float _JumpHeight;
    [Header("Parent of the Core")]
    [SerializeField] private Transform _Parent;

    private Rigidbody2D _ParentBody;
    private bool _CanJump = true;
    private bool _IsTileGround;
    private GameObject _Other;
    private Monster _ParentMonster;
    private Vector2 _DirectionAbsolute;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _Other = _Parent.gameObject;
        _ParentBody = _Parent.GetComponent<Rigidbody2D>();
        _ParentMonster = _Other.gameObject.GetComponent<Monster>();
    }

    // Update is called once per frame
    void Update()
    {
        int layerMask = LayerMask.GetMask("Ground");
        if (_ParentMonster.GetDirectionMouvement().x < -0.01f) _DirectionAbsolute = Vector2.left;
        else if (_ParentMonster.GetDirectionMouvement().x > 0.01f) _DirectionAbsolute = Vector2.right;

        _CanJump = Physics2D.Raycast(transform.position, Vector2.down, 0.4f, layerMask);
        _IsTileGround = Physics2D.Raycast(transform.position, _DirectionAbsolute, 0.6f, layerMask);

        Debug.DrawRay(this.gameObject.transform.position, Vector2.down, _CanJump ? Color.red : Color.gray);
        Debug.DrawRay(this.gameObject.transform.position, _DirectionAbsolute, _IsTileGround ? Color.yellow : Color.cyan);

        //faster falling
        if (_ParentBody.velocity.y < 0) _ParentBody.velocity += Vector2.up * Physics.gravity.y * (3.0f - 1) * Time.deltaTime;
        if (_IsTileGround && _CanJump) _ParentBody.velocity = new Vector2(0, Mathf.Sqrt(-2.0f * Physics2D.gravity.y * _JumpHeight));
    }
}
