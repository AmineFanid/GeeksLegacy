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
        playersLife = _PlayerControl.findPlayerObject().GetLifePoint(); // Pour initialiser la variable playersLife avec la vie actuelle du joueur
        EventManager.StartListening(EventManager.PossibleEvent.eVieJoueurChange, _EcouteurBruit); // Subscribe to eVieJoueurChange event
    }

    void Update()
    {
        _Animator.SetFloat("CurrentHP", playersLife); //Pour mettre a jour le sprite (animation) de la barre de vie
    }

    public void updateHealthBar(object newLife)//Pour mettre a jour playersLife
    {
        playersLife = (float)newLife;       
    }
}
