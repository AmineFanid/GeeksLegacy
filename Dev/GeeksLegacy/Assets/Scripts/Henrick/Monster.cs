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
    [Header("Target to chase")]
    [SerializeField]
    private Transform _Cible;

    [Header("Mouvement settings")]
    [SerializeField]
    private float _ForceMouvement = 10.0f;
    [SerializeField] private float _MaxSpeed = 10.0f;

    [Header("Target detection settings")]
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

        //Vector représentant la direction de la cible
        Vector2 delta = _Cible.position - this.gameObject.transform.position;
        //Normalisation du Vector
        Vector2 _DirectionVision = delta.normalized;

        int layerMask = LayerMask.GetMask(new[] { "Obstacle", "Player" });

        //raycast original de la détection du joueur
        RaycastHit2D hit = Physics2D.Raycast(this.gameObject.transform.position, _DirectionVision, _DistanceVision, layerMask);

        //Boucle pour générer plusieurs raycast a différent angle
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

            //Généré la hitbox
            RaycastHit2D Suphit = Physics2D.Raycast(this.gameObject.transform.position, directionRotated, _DistanceVision, layerMask);

            //Si les raycast détecte un joueur, met a True la variable
            _EstEnChasse = Suphit.collider && Suphit.collider.gameObject.layer == _Cible.gameObject.layer;
            Debug.DrawRay(this.gameObject.transform.position, directionRotated * _DistanceVision, _EstEnChasse ? Color.red : Color.gray);
        }
        //Si le raycast détecte un joueur, met a True la variable
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

        _Animator.SetBool("IsMoving", vitesseBouge);
        if (vitesseBouge) //Si l'ennemi bouge
        {
            //Active ses animations en fonction du mouvement
            Vector2 directionAssainie = ForceAnimationVirtualJoystick.ForceDirectionAxe(DirectionMouvement);
            _Animator.SetFloat("MouvementX", directionAssainie.x);
            _Animator.SetFloat("MouvementY", directionAssainie.y);
        }
    }

    private void FixedUpdate()
    {
        //Ajoute la force de déplacment de l'ennemi
        _Rigidbody2D.AddForce(DirectionMouvement * _ForceMouvement);

        //Vérification pour limité la vitesse de l'ennemi
        if (_Rigidbody2D.velocity.x >= _MaxSpeed)
            _Rigidbody2D.velocity = new Vector2(_MaxSpeed, _Rigidbody2D.velocity.y);
        if (_Rigidbody2D.velocity.x <= _MaxSpeed * -1)
            _Rigidbody2D.velocity = new Vector2(_MaxSpeed*-1, _Rigidbody2D.velocity.y);
    }

    private IEnumerator Errer() //Errence
    {
        while (true)
        {
            DirectionMouvement = Vector2.zero;
            yield return new WaitForSeconds(Random.value * 2+1);
            DirectionMouvement.x = Random.Range(-1.0f, 1.0f);
            yield return new WaitForSeconds(Random.value * 3+1);
        }
    }

    public Vector2 GetDirectionMouvement() { //Getter de la direction de mouvement
        if (DirectionMouvement != null) return DirectionMouvement;
        else return Vector2.zero;
    }
}
