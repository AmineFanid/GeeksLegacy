using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class MaterialsBehaviour : MonoBehaviour
{
    public float detectionRadius = 5.0f; // Rayon pour detecter le joueur quand il est proche
    private GameObject _InstanceOfPlayer;
    private ControlCharacters playerControl;
    private bool _Collected = false;
    private Vector3 _Start;
    private float _Duration = 3f;
    private float _ElapsedTime;
    [SerializeField] string itemName;
    private CharacterInventory _CharacInventory;



    void Start()
    {
        _InstanceOfPlayer = GameObject.FindGameObjectWithTag("Player");
        playerControl = _InstanceOfPlayer.GetComponent<ControlCharacters>();
        _CharacInventory = playerControl.findPlayerObject().GetPlayerInventory();
        _Start = transform.position;
    }

    void Update()
    {
        _ElapsedTime += Time.deltaTime;
        float distance = Vector3.Distance(transform.position, _InstanceOfPlayer.transform.position);
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
            if (_CharacInventory.canAddInventory()) {
                float percentageDestination = _ElapsedTime / _Duration;

                transform.position = Vector3.Lerp(transform.position, _InstanceOfPlayer.transform.position, Time.deltaTime / finalSpeed);
            }            
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_CharacInventory != null)
        {
            if(itemName != null && _CharacInventory.canAddInventory())
            {
                _CharacInventory.addItem(itemName);
                Destroy(gameObject);
            }
        }
    }
}
