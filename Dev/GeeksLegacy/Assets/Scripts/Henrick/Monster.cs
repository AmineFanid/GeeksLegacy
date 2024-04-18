using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]

public class Monster : MonoBehaviour
{
    [SerializeField]
    private Transform _Cible;

    [SerializeField]
    private float _ForceMouvement = 10.0f;

    [SerializeField] private float _MaxSpeed = 10.0f;

    [SerializeField]
    private bool _EstEnChasse = false;

    [SerializeField]
    private float _DistanceVision = 5.0f;

    private Animator _Animator;
    private Rigidbody2D _Rigidbody2D;
    public Vector2 DirectionMouvement;

    IEnumerator _Errer;

    // Start is called before the first frame update
    void Start()
    {
        _Animator = GetComponent<Animator>();
        _Rigidbody2D = GetComponent<Rigidbody2D>();
        _Errer = Errer();
        StartCoroutine(_Errer);
    }

    // Update is called once per frame
    void Update()
    {
        float angleSup = 5.0f;
        float angleInf = -5.0f;
        bool etaitEnChasse = _EstEnChasse;
        Vector2 delta = _Cible.position - this.gameObject.transform.position;
        Vector2 _DirectionVision = delta.normalized;
        int layerMask = LayerMask.GetMask(new[] { "Obstacle", "Player" });
        RaycastHit2D hit = Physics2D.Raycast(this.gameObject.transform.position, _DirectionVision, _DistanceVision, layerMask);
        for (int i = 0; i < 10; i++)
        {
            Quaternion rotation;
            if (i < 4)
            {
                rotation = Quaternion.AngleAxis(angleSup, Vector3.forward);
                angleSup += 5.0f;
            }
            else
            {
                rotation = Quaternion.AngleAxis(angleInf, Vector3.forward);
                angleInf -= 5.0f;
            }

            Vector2 directionRotated = rotation * _DirectionVision;
            RaycastHit2D Suphit = Physics2D.Raycast(this.gameObject.transform.position, directionRotated, _DistanceVision, layerMask);
            _EstEnChasse = Suphit.collider && Suphit.collider.gameObject.layer == _Cible.gameObject.layer;
            Debug.DrawRay(this.gameObject.transform.position, directionRotated * _DistanceVision, _EstEnChasse ? Color.red : Color.gray);
        }
        _EstEnChasse = hit.collider && hit.collider.gameObject.layer == _Cible.gameObject.layer;
        Debug.DrawRay(this.gameObject.transform.position, _DirectionVision * _DistanceVision, _EstEnChasse ? Color.green : Color.blue);


        if (_EstEnChasse)
        {
            //Vient de tomber en chasse
            if (!etaitEnChasse)
            {
                StopCoroutine(_Errer);
                _Errer = Errer();
            }
            DirectionMouvement = new Vector2(_DirectionVision.x, 0.0f);
        }
        else
        { //Errance
            //Vient de tomber en errance
            if (etaitEnChasse)
            {
                StartCoroutine(_Errer);
            }
        }

        float vitesse = _Rigidbody2D.velocityX;
        bool vitesseBouge = vitesse > 0.01f || vitesse < -0.01f;
        //Debug.Log(vitesse);
        _Animator.SetBool("IsMoving", vitesseBouge);
        if (vitesseBouge)
        {
            Vector2 directionAssainie = ForceAnimationVirtualJoystick.ForceDirectionAxe(DirectionMouvement);
            _Animator.SetFloat("MouvementX", directionAssainie.x);
            _Animator.SetFloat("MouvementY", directionAssainie.y);
        }
    }

    private void FixedUpdate()
    {
        _Rigidbody2D.AddForce(DirectionMouvement * _ForceMouvement);
        if (_Rigidbody2D.velocity.x >= _MaxSpeed)
            _Rigidbody2D.velocity = new Vector2(_MaxSpeed, _Rigidbody2D.velocity.y);
        if (_Rigidbody2D.velocity.x <= _MaxSpeed * -1)
            _Rigidbody2D.velocity = new Vector2(_MaxSpeed*-1, _Rigidbody2D.velocity.y);
    }

    private IEnumerator Errer()
    {
        while (true)
        {
            DirectionMouvement = Vector2.zero;
            yield return new WaitForSeconds(Random.value * 2+1);
            DirectionMouvement.x = Random.Range(-1.0f, 1.0f);
            yield return new WaitForSeconds(Random.value * 3+1);
        }
    }

    public Vector2 GetDirectionMouvement() {
        if (DirectionMouvement != null) return DirectionMouvement;
        else return Vector2.zero;
    }
}

/*
 public interface IState
{
    public void Enter();
    public void Execute();
    public void Exit();
}
 
public class StateMachine
{
    IState currentState;
 
    public void ChangeState(IState newState)
    {
        if (currentState != null)
            currentState.Exit();
 
        currentState = newState;
        currentState.Enter();
    }
 
    public void Update()
    {
        if (currentState != null) currentState.Execute();
    }
}
 
public class TestState : IState
{
    Unit owner;
 
    public TestState(Unit owner) { this.owner = owner; }
 
    public void Enter()
    {
        Debug.Log("entering test state");
    }
 
    public void Execute()
    {
        Debug.Log("updating test state");
    }
 
    public void Exit()
    {
        Debug.Log("exiting test state");
    }
}
 
public class Unit : MonoBehaviour
{
    StateMachine stateMachine = new StateMachine();
   
    void Start()
    {
        stateMachine.ChangeState(new TestState(this));
    }
 
    void Update()
    {
        stateMachine.Update();
    }
}
 
 */
