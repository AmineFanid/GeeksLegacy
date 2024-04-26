using System.Collections;
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
    [SerializeField] string itemName;
    private CharacterInventory characInventory;


    //public void 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        characInventory = FindFirstObjectByType<CharacterInventory>();
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
            if (characInventory.canAddInventory()) {
                float percentageDestination = _ElapsedTime / duration;

                transform.position = Vector3.Lerp(transform.position, player.transform.position, Time.deltaTime / finalSpeed);
            }

            
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

       //CharacInventory = other.GetComponent<CharacterInventory>();
        if (characInventory != null)
        {
            if(itemName != null && characInventory.canAddInventory())
            {
                characInventory.addItem(itemName);
                Destroy(gameObject);
            }
        }
    }

}
