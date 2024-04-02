using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]

public class Monster : MonoBehaviour
{
    [SerializeField]
    private Transform _Cible;

    [SerializeField]
    private float _ForceMouvement = 10.0f;

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
        bool etaitEnChasse = _EstEnChasse;
        Vector2 delta = _Cible.position - this.gameObject.transform.position;
        Vector2 _DirectionVision = delta.normalized;
        int layerMask = LayerMask.GetMask(new[] { "Obstacle", "Joueur" });
        RaycastHit2D hit = Physics2D.Raycast(this.gameObject.transform.position, _DirectionVision, _DistanceVision, layerMask);
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

        float vitesse = _Rigidbody2D.velocity.magnitude;
        _Animator.SetFloat("Vitesse", vitesse);
        if (vitesse > 0.01f)
        {
            Vector2 directionAssainie = ForceAnimationVirtualJoystick.ForceDirectionAxe(_DirectionMouvement);
            _Animator.SetFloat("MouvementX", directionAssainie.x);
            _Animator.SetFloat("MouvementY", directionAssainie.y);
        }
    }

    private void FixedUpdate()
    {
        _Rigidbody2D.AddForce(_DirectionMouvement * _ForceMouvement);
    }

    private IEnumerator PrintDebug(float InIntervalle)
    {
        int compte = 0;
        while (true)
        {
            Debug.Log(compte);
            compte++;
            yield return new WaitForSeconds(InIntervalle);
        }
    }

    private IEnumerator Errer()
    {
        while (true)
        {
            //TODO: mettre valeur random pour le temps de mouvement
            _DirectionMouvement = Vector2.zero;
            yield return new WaitForSeconds(1.5f);
            //TODO: mettre valeur random dans l'attente
            _DirectionMouvement = Random.insideUnitCircle.normalized;
            yield return new WaitForSeconds(1.0f);
        }
    }
}
