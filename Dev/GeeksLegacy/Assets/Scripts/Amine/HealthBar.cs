// HealthBar
// Gestion de la barre de vie
// Auteurs: Amine Fanid et Henrick Baril
using UnityEngine;
using UnityEngine.Events;

[DefaultExecutionOrder(35)]

public class HealthBar : MonoBehaviour
{
    private UnityAction<object> _EcouteurBruit; 
    private GameObject _PLayerObject;
    private ControlCharacters _PlayerControl;
    private Animator _Animator;
    public float playersLife;

    void Start()
    {
        _Animator = GetComponent<Animator>();
        _EcouteurBruit += updateHealthBar;
        _PLayerObject = GameObject.FindGameObjectWithTag("Player");
        _PlayerControl = _PLayerObject.GetComponent<ControlCharacters>();
        playersLife = _PlayerControl.findPlayerObject().GetLifePoint(); 
        EventManager.StartListening(EventManager.PossibleEvent.eVieJoueurChange, _EcouteurBruit); 
    }

    void Update()
    {
        _Animator.SetFloat("CurrentHP", playersLife); 
    }

    public void updateHealthBar(object newLife)
    {
        playersLife = (float)newLife;       
    }
}
