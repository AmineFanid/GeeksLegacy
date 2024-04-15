using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]

public class TileDetection : MonoBehaviour
{
    [SerializeField] private float _JumpAmount;
    [SerializeField] private float _JumpHeight;
    [SerializeField] private Transform _Parent;

    private Rigidbody2D _Rigidbody2D;
    private Collider2D _Collider;
    private Rigidbody2D _ParentBody;
    private int layerMask;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _Collider = GetComponent<Collider2D>();
        _Rigidbody2D = GetComponent<Rigidbody2D>();
        _ParentBody = _Parent.GetComponent<Rigidbody2D>();
        layerMask = LayerMask.GetMask(new[] {"Ground"});
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(_ParentBody.gameObject);
    }

    
    private void OnTriggerEnter2D(Collider2D collider)
    {

        if (_Collider.IsTouchingLayers(layerMask))
        {
            Debug.Log("JUMP");
            _ParentBody.AddForce(Vector2.up * _JumpAmount, ForceMode2D.Impulse);
        }
    }
}
