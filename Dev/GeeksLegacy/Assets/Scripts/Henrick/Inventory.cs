using UnityEngine;

public class Inventory : MonoBehaviour
{

    private Animator _AnimatorInv;
    int InventoryIndex = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _AnimatorInv = GetComponent<Animator>();
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
