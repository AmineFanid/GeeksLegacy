using UnityEngine;
using UnityEngine.Events;

public class HealthBar : MonoBehaviour
{
    private UnityAction<object> _EcouteurBruit;

    private GameObject _PLayerObject;
    private ControlCharacters _PlayerControl;
    private Animator _Animator;

    public float playersLife;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _EcouteurBruit += updateHealthBar;
        _PLayerObject = GameObject.FindGameObjectWithTag("Player");
        _PlayerControl = _PLayerObject.GetComponent<ControlCharacters>();
        EventManager.StartListening(EventManager.PossibleEvent.eVieJoueurChange, _EcouteurBruit); // Subscribe to ePlayerLifeChanged event


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void updateHealthBar(object newLife)
    {
        //ICI ON VA UPDATE LES ANIMATIONS DE LA HEALTHBAR
        print("updating healthbar");
    }
}
