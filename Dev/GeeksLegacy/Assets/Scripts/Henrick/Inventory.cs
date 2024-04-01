using UnityEngine;

public class Inventory : MonoBehaviour
{

    private Animator _AnimatorInv;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _AnimatorInv = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(Input.GetAxisRaw("Scroll"));
        if (Input.GetAxisRaw("Scroll") > 0.0f)
        {
            _AnimatorInv.SetTrigger("ScrollUp");
        }
        if (Input.GetAxisRaw("Scroll") < 0.0f)
        {
            _AnimatorInv.SetTrigger("ScrollDown");
        }
    }
}
