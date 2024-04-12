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
    Vector2 _DirectionMouvement;

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
            RaycastHit2D Suphit = Physics2D.Raycast(this.gameObject.transform.position, _DirectionVision, _DistanceVision, layerMask);
            _EstEnChasse = Suphit.collider && Suphit.collider.gameObject.layer == _Cible.gameObject.layer;
            Debug.DrawRay(this.gameObject.transform.position, directionRotated * _DistanceVision, _EstEnChasse ? Color.red : Color.gray);
        }
        _EstEnChasse = hit.collider && hit.collider.gameObject.layer == _Cible.gameObject.layer;
        Debug.DrawRay(this.gameObject.transform.position, _DirectionVision * _DistanceVision, _EstEnChasse ? Color.green : Color.gray);


        if (_EstEnChasse)
        {
            //Vient de tomber en chasse
            if (!etaitEnChasse)
            {
                StopCoroutine(_Errer);
                _Errer = Errer();
            }
            _DirectionMouvement = _DirectionVision;
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
        bool vitesseBouge = vitesse > 0.01f;
        _Animator.SetBool("IsMoving", vitesseBouge);
        if (vitesseBouge)
        {
            Vector2 directionAssainie = ForceAnimationVirtualJoystick.ForceDirectionAxe(_DirectionMouvement);
            _Animator.SetFloat("MouvementX", directionAssainie.x);
        }
    }

    private void FixedUpdate()
    {
        Debug.Log(_DirectionMouvement * _ForceMouvement);
        _Rigidbody2D.AddForce(_DirectionMouvement * _ForceMouvement);
        if (_Rigidbody2D.velocity.x >= _MaxSpeed)
            _Rigidbody2D.velocity = new Vector2(_MaxSpeed, _MaxSpeed);
    }

    private IEnumerator Errer()
    {
        while (true)
        {
            _DirectionMouvement = Vector2.zero;
            yield return new WaitForSeconds(Random.value * 4+1);
            _DirectionMouvement.x = Random.Range(-1.0f, 1.0f);
            yield return new WaitForSeconds(Random.value * 2+1);
        }
    }
}
