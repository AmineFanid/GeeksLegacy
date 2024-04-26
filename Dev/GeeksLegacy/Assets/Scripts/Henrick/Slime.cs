using UnityEngine;
using Patterns;
using Unity.VisualScripting;
using System.Net.NetworkInformation;
using System.Collections;
using static UnityEditor.Progress;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]

public class Slime : MonoBehaviour
{
    [Header("Target to chase")]
    [SerializeField] private Transform _Cible;
    [Header("Mouvement settings")]
    [SerializeField] private float _ForceMouvement = 10.0f;
    [SerializeField] private float _MaxSpeed = 10.0f;
    [Header("Jump settings")]
    [SerializeField] private float _JumpHeight;
    [Header("Target detection settings")]
    [SerializeField] private bool _EstEnChasse = false;
    [SerializeField] private float _DistanceVision = 5.0f;
    [Header("Slime stats")]
    [SerializeField] private float _TotalHealth = 100.0f;

    private Animator _Animator;
    private Rigidbody2D _Rigidbody2D;
    public Vector2 DirectionMouvement;
    IEnumerator _Wander;
    IEnumerator _AttackPlayer;

    FiniteStateMachine<SlimeState> mFsm = new FiniteStateMachine<SlimeState>();

    private float angleSup;
    private float angleInf;
    private bool etaitEnChasse;
    private bool etaitAttack;
    private Vector2 _DirectionVision;
    private float _Timer;
    private Rigidbody2D _ChildRigidbody2D;
    private TileDetection _Child;
    private float _CurrentHealth;
    private Item weapon;

    public enum SlimeState 
    {
        IDLE,
        CHASE,
        ATTACK,
        DAMAGE,
        DIE,
    }

    public enum Weapon
    {
        Sword,
        Axe,
    }

    void Start()
    {
        //Debug.Log("Start");
        mFsm.Add(
            new State<SlimeState>(
                SlimeState.IDLE,
                "IDLE",
                OnEnterIDLE,
                OnExitIDLE,
                OnUpdateIDLE,
                null)
            );
        mFsm.Add(
            new State<SlimeState>(
                SlimeState.CHASE,
                "CHASE",
                OnEnterCHASE,
                null,
                OnUpdateCHASE,
                null)
            );
        mFsm.Add(
            new State<SlimeState>(
                SlimeState.ATTACK,
                "ATTACK",
                OnEnterATTACK,
                OnExitATTACK,
                OnUpdateATTACK,
                null)
            );
        mFsm.Add(
            new State<SlimeState>(
                SlimeState.DAMAGE,
                "DAMAGE",
                OnEnterDAMAGE,
                null,
                OnUpdateDAMAGE,
                null)
            );
        mFsm.Add(
            new State<SlimeState>(
                SlimeState.DIE,
                "DIE",
                OnEnterDIE,
                null,
                OnUpdateDIE,
                null)
            );
        Init_IdleState();
        Init_AttackState();
        Init_DieState();
        Init_DamageState();
        Init_ChaseState();
        mFsm.SetCurrentState(SlimeState.IDLE);
        _Animator = GetComponent<Animator>();
        _Rigidbody2D = GetComponent<Rigidbody2D>();
        _CurrentHealth = _TotalHealth;
    }

    /* --------------------------INIT------------------------------- */

    private void Init_IdleState() {
     
    }

    private void Init_ChaseState()
    {

    }

    private void Init_AttackState()
    {

    }

    private void Init_DamageState()
    {

    }

    private void Init_DieState()
    {

    }

    /* --------------------------UPDATE------------------------------- */

    private void Update()
    {
        mFsm.Update();
    }

    private void FixedUpdate()
    {
        mFsm.FixedUpdate();
    }

    /* --------------------------IDLE------------------------------- */

    #region Delegates implementation for the states.
    void OnEnterIDLE() //ENTRY OF THE IDLE STATE
    {
        _Wander = Wander();
        StartCoroutine(_Wander);
        Debug.Log("Enter the " + mFsm.GetCurrentState().Name.ToString());
    }

    void OnUpdateIDLE() //UPDATE OF THE IDLE STATE
    {
        //Debug.Log("SlimeState IDLE");
        TileDetection t = FindFirstObjectByType<TileDetection>();
        if (t.PlayerDetection()) mFsm.SetCurrentState(SlimeState.ATTACK);
        else
        {
            if (ChaseDetection() == false) mFsm.SetCurrentState(SlimeState.IDLE);
            else mFsm.SetCurrentState(SlimeState.CHASE);
        }
        Mouvement();
    }

    void OnExitIDLE()
    {
        StopCoroutine(_Wander);
    }

    /* --------------------------CHASE------------------------------- */

    void OnEnterCHASE() //ENTRY OF THE CHASE STATE
    {
        Debug.Log("Enter the " + mFsm.GetCurrentState().Name.ToString());
    }

    void OnUpdateCHASE() //UPDATE OF THE CHASE STATE
    {
        //Debug.Log(ChaseDetection());
        TileDetection t = FindFirstObjectByType<TileDetection>();
        if (t.PlayerDetection()) mFsm.SetCurrentState(SlimeState.ATTACK);
        else
        {
            if (ChaseDetection() == false) mFsm.SetCurrentState(SlimeState.IDLE);
            else mFsm.SetCurrentState(SlimeState.CHASE);
        }

        //Vient de tomber en chasse
        if (!etaitEnChasse)
        {
            StopCoroutine(_Wander);
            _Wander = Wander();
        }

        //Vector représentant la direction de la cible
        Vector2 delta = _Cible.position - this.gameObject.transform.position;
        //Normalisation du Vector
        _DirectionVision = delta.normalized;

        DirectionMouvement = new Vector2(_DirectionVision.x, 0.0f);

        Mouvement();
    }

    /* --------------------------ATTACK------------------------------- */

    void OnEnterATTACK() //ENTRY OF THE ATTACK STATE
    {
        Debug.Log("Enter the " + mFsm.GetCurrentState().Name.ToString());
        etaitAttack = false;
    }

    void OnUpdateATTACK() //UPDATE OF THE ATTACK STATE
    {
        TileDetection t = FindFirstObjectByType<TileDetection>();
        if (t.PlayerDetection()) mFsm.SetCurrentState(SlimeState.ATTACK);
        else
        {
            if (ChaseDetection() == false) mFsm.SetCurrentState(SlimeState.IDLE);
            else mFsm.SetCurrentState(SlimeState.CHASE);
        }

        //Vector représentant la direction de la cible
        Vector2 delta = _Cible.position - this.gameObject.transform.position;
        //Normalisation du Vector
        _DirectionVision = delta.normalized;

        DirectionMouvement = new Vector2(_DirectionVision.x, 0.0f);

        if (!etaitAttack)
        {
            _AttackPlayer = AttackPlayer();
            StartCoroutine(_AttackPlayer);
        }

        etaitAttack = t.etaitAttack;
    }

    void OnExitATTACK()
    {
        StopAllCoroutines();
    }

    /* --------------------------DAMAGE------------------------------- */

    void OnEnterDAMAGE() //ENTRY OF THE DAMAGE STATE
    {
        Debug.Log("Enter the " + mFsm.GetCurrentState().Name.ToString());
    }

    void OnUpdateDAMAGE() //UPDATE OF THE DAMAGE STATE
    {
        //Debug.Log("SlimeState DAMAGE");
        mFsm.SetCurrentState(SlimeState.DAMAGE);


        
    }

    /* --------------------------DIE------------------------------- */

    void OnEnterDIE()
    {
        //Debug.Log("Enter the " + mFsm.GetCurrentState().Name.ToString());
    }

    void OnUpdateDIE()
    {
        //Debug.Log("SlimeState DIE");
        mFsm.SetCurrentState(SlimeState.DIE);
    }
    #endregion

    /* --------------------------FUNCTION------------------------------- */

    private IEnumerator Wander() //Wander
    {
        while (true)
        {
            DirectionMouvement = Vector2.zero;
            yield return new WaitForSeconds(Random.value * 2 + 2);
            DirectionMouvement.x = Random.Range(-1.0f, 1.0f);
            yield return new WaitForSeconds(Random.value * 3 + 1);
        }
    }

    public Vector2 GetDirectionMouvement()
    { //Getter de la direction de mouvement
        if (DirectionMouvement != null) return DirectionMouvement;
        else return Vector2.zero;
    }

    private void Mouvement() {
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

        //Ajoute la force de déplacment de l'ennemi
        _Rigidbody2D.AddForce(DirectionMouvement * _ForceMouvement);

        //Vérification pour limité la vitesse de l'ennemi
        if (_Rigidbody2D.velocity.x >= _MaxSpeed)
            _Rigidbody2D.velocity = new Vector2(_MaxSpeed, _Rigidbody2D.velocity.y);
        if (_Rigidbody2D.velocity.x <= _MaxSpeed * -1)
            _Rigidbody2D.velocity = new Vector2(_MaxSpeed * -1, _Rigidbody2D.velocity.y);

        //Debug.Log(_Rigidbody2D.velocity);
    }

    private bool ChaseDetection()
    {
        angleSup = 5.0f;
        angleInf = -5.0f;

        etaitEnChasse = _EstEnChasse;
        //Vector représentant la direction de la cible
        Vector2 delta = _Cible.position - this.gameObject.transform.position;
        //Normalisation du Vector
        _DirectionVision = delta.normalized;

        int layerMask = LayerMask.GetMask(new[] {"Player", "Ground"});
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
        return _EstEnChasse;
    }

    private IEnumerator AttackPlayer() //Wander
    {
        TileDetection t = FindFirstObjectByType<TileDetection>();
        while (true)
        {
            _Rigidbody2D.velocity = Vector2.zero;
            yield return new WaitForSeconds(0.8f);
            /*------------*/
            if (t.CanJump())
            {
                _Rigidbody2D.velocity = new Vector2(DirectionMouvement.x * _ForceMouvement, Mathf.Sqrt(-2.0f * Physics2D.gravity.y * _JumpHeight));
            }
            yield return new WaitForSeconds(0.8f);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.layer == LayerMask.NameToLayer("Weapon")){
            mFsm.SetCurrentState(SlimeState.DAMAGE);
            //if (_CurrentHealth - )
            _CurrentHealth -= (_TotalHealth * 1 / 100);
        }
    }
}