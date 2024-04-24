using UnityEngine;

public class MaterialsBehaviour : MonoBehaviour
{
    [SerializeField]
    [Range(0.1f, 0.6f)]
    public float amplitude = 0.5f;
    [SerializeField]
    public float speed = 1.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 p = transform.position;
        p.y = amplitude * Mathf.Cos(Time.time * speed);
        transform.position = p;
    }
}
