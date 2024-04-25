using UnityEngine;

public class MaterialsBehaviour : MonoBehaviour
{

    public float amplitude = 0.1f;
    public float speed = 4.0f;
    public float detectionRadius = 5.0f; // Rayon pour detecter le joueur quand y est proche
    public Transform player;


    private bool _Collected = false;
    private Vector3 _Destination = new Vector3(5, -2, 0);
    private Vector3 _Start;
    public float duration = 3f;
    private float _ElapsedTime;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _Start = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        _ElapsedTime += Time.deltaTime;

        if (!_Collected)
        {
            Vector3 p = transform.position;
            p.y = amplitude * Mathf.Cos(Time.time * speed);
            transform.position = p;

            if (Vector3.Distance(transform.position, player.position) < detectionRadius)
            {
                _Collected = true;
                print("DETEXCTED A PLAYERR");
            }
        }
        else
        {
            float percentageDestination = _ElapsedTime / duration;
            transform.position = Vector3.Lerp(transform.position, player.position, percentageDestination);

        }
        print(" LE COLLECTED ESTTTTTTTTT " + _Collected);
    }
}
