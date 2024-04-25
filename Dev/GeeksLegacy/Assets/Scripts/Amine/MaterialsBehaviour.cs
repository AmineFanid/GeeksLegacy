using Unity.VisualScripting;
using UnityEngine;

public class MaterialsBehaviour : MonoBehaviour
{
    public float detectionRadius = 5.0f; // Rayon pour detecter le joueur quand y est proche
    private GameObject player;
    private bool _Collected = false;
    private Vector3 _Start;
    public float duration = 3f;
    private float _ElapsedTime;
    public Vector3 characpos;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        _Start = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        _ElapsedTime += Time.deltaTime;
        float distance = Vector3.Distance(transform.position, player.transform.position);
        float finalSpeed = (distance / 5);


        if (!_Collected)
        {

            if (distance < detectionRadius)
            {
                _Collected = true;
            }
        }
        else
        {

            float percentageDestination = _ElapsedTime / duration;

            transform.position = Vector3.Lerp(transform.position, player.transform.position, Time.deltaTime/finalSpeed);

        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        CharacterInventory CharacInventory = other.GetComponent<CharacterInventory>();
        if(CharacInventory != null)
        {
            print("charactingeru!!");
            CharacInventory.ItemsCollected();
            gameObject.SetActive(false);
        }
    }
}
