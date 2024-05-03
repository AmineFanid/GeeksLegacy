using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class MaterialsBehaviour : MonoBehaviour
{
    public float detectionRadius = 5.0f; // Rayon pour detecter le joueur quand y est proche
    private GameObject pushingP;
    private Player player;
    private ControlCharacters playerControl;
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
        //characInventory = FindFirstObjectByType<CharacterInventory>();
        ///////////////
        pushingP = GameObject.FindGameObjectWithTag("Player");
        playerControl = pushingP.GetComponent<ControlCharacters>();
        ///////////
        characInventory = playerControl.inventory;
        //player = GameObject.FindGameObjectWithTag("Player");
        //player = playerControl.findPlayerObject();
        _Start = transform.position;
        

    }

    // Update is called once per frame
    void Update()
    {
        _ElapsedTime += Time.deltaTime;
        float distance = Vector3.Distance(transform.position, pushingP.transform.position);
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

                transform.position = Vector3.Lerp(transform.position, pushingP.transform.position, Time.deltaTime / finalSpeed);
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
