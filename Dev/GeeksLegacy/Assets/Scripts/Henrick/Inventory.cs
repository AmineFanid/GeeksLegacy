using UnityEngine;

public class Inventory : MonoBehaviour
{
    private GameObject _PLayerObject;
    private ControlCharacters _PlayerControl;
    public CharacterInventory inventory;

    private Animator _AnimatorInv;
    int InventoryIndex = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _AnimatorInv = GetComponent<Animator>();

        _PLayerObject = GameObject.FindGameObjectWithTag("Player");
        _PlayerControl = _PLayerObject.GetComponent<ControlCharacters>();
        inventory = _PlayerControl.findPlayerObject().GetPlayerInventory();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxisRaw("Scroll") > 0.0f)
        {
            _AnimatorInv.SetTrigger("ScrollUp");
            InventoryIndex++;
        }
        if (Input.GetAxisRaw("Scroll") < 0.0f)
        {
            _AnimatorInv.SetTrigger("ScrollDown");
            InventoryIndex--;
        }
    }
}
