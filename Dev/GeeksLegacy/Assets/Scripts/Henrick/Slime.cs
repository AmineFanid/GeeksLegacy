using UnityEngine;
using Patterns;
using System.Collections;
using Unity.VisualScripting;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]

public class Slime : MonoBehaviour
{
    [Header("Mouvement settings")]
    [SerializeField] private float _ForceMouvement = 10.0f;
    [SerializeField] private float _MaxSpeed = 10.0f;
    [SerializeField] private float _MaxYSpeed = 20.0f;
    [Header("Jump settings")]
    [SerializeField] private float _JumpHeight;
    [Header("Target detection settings")]
    [SerializeField] private bool _EstEnChasse = false;
    [SerializeField] private float _DistanceVision = 5.0f;
    [Header("Slime stats")]
    [SerializeField] private float _TotalHealth = 100.0f;
    [Header("Others")]
    [SerializeField] public ControlCharacters player;
    [SerializeField] private float _KbTaken = 10.0f;

    private Animator _Animator;
    private Rigidbody2D _Rigidbody2D;
    public Vector2 DirectionMouvement;
    private Transform _Cible;

    private IEnumerator _Wander;
    private IEnumerator _AttackPlayer;
    private IEnumerator _Die;

    FiniteStateMachine<SlimeState> mFsm = new FiniteStateMachine<SlimeState>();

    private float angleSup;
    private float angleInf;
    private bool etaitEnChasse;
    private bool etaitAttack;
    private Vector2 _DirectionVision;
    private float _CurrentHealth;

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
        _Cible = GameObject.FindGameObjectWithTag("Player").transform;
        Debug.Log("FSM : " + mFsm._CurrentGuid); //Meme FSM ?????????
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
        //Debug.Log("Enter the " + mFsm.GetCurrentState().Name.ToString());
    }

    void OnUpdateIDLE() //UPDATE OF THE IDLE STATE
    {
        //Debug.Log(_Wander);
        //Debug.Log("SlimeState IDLE");
        TileDetection t = FindFirstObjectByType<TileDetection>();
        if (t.PlayerDetection()) mFsm.SetCurrentState(SlimeState.ATTACK);
        else
        {
            if (!ChaseDetection())
            {
                mFsm.SetCurrentState(SlimeState.IDLE);
            }
            else
            {
                mFsm.SetCurrentState(SlimeState.CHASE);
            }
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
        //Debug.Log("Enter the " + mFsm.GetCurrentState().Name.ToString());
    }

    void OnUpdateCHASE() //UPDATE OF THE CHASE STATE
    {
        //Debug.Log(ChaseDetection());
        TileDetection t = FindFirstObjectByType<TileDetection>();
        if (t.PlayerDetection()) 
        {
            mFsm.SetCurrentState(SlimeState.ATTACK);
        }
        else
        {
            if (ChaseDetection() == false) mFsm.SetCurrentState(SlimeState.IDLE);
            else mFsm.SetCurrentState(SlimeState.CHASE);
        }

        if (t.CanJump() && t.TileDetections()) {
            t.JumpPhysics();
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
        //Debug.Log("Enter the " + mFsm.GetCurrentState().Name.ToString());
        //etaitAttack = false;
        //Debug.Log("Mon ID: " + GetInstanceID()); //Pas le meme ID
        //Debug.Log("_Cible.position:  " + _Cible.position); //meme cible
        //Debug.Log("delta:  " + delta); //Pas le meme Delta
        //Debug.Log("this.gameObject.transform.position:  " + this.gameObject.transform.position); //Pas la meme position

        _AttackPlayer = AttackPlayer();
        StartCoroutine(_AttackPlayer);
    }

    void OnUpdateATTACK() //UPDATE OF THE ATTACK STATE
    {
        TileDetection t = FindFirstObjectByType<TileDetection>(); //GetcomponentInChildren ?????????????????????
        //Debug.Log("instance de tile : " + t.GetInstanceID()); // Pas le meme tile ID
        if (t.PlayerDetection()) mFsm.SetCurrentState(SlimeState.ATTACK);
        else
        {
            if (ChaseDetection() == false) mFsm.SetCurrentState(SlimeState.IDLE);
            else mFsm.SetCurrentState(SlimeState.CHASE);
        }

        if (t.CanJump() && t.TileDetections())
        {
            Mouvement();
            t.JumpPhysics();
        }

        //Vector représentant la direction de la cible
        Vector2 delta = _Cible.position - this.gameObject.transform.position; 
        //Normalisation du Vector
        _DirectionVision = delta.normalized; 

        DirectionMouvement = new Vector2(_DirectionVision.x, 0.0f);

        //if (!etaitAttack)
        //{
        //
        //}

        //etaitAttack = t.etaitAttack;
    }

    void OnExitATTACK()
    {
        StopAllCoroutines();
    }

    /* --------------------------DAMAGE------------------------------- */

    void OnEnterDAMAGE() //ENTRY OF THE DAMAGE STATE
    {
        //Debug.Log("Enter the " + mFsm.GetCurrentState().Name.ToString());
        Vector2 direction = GetDirectionMouvement();
        Vector2 directionForce = (direction.x < 0.01f) ? Vector2.left : Vector2.right;
        _Rigidbody2D.AddForce(directionForce * _KbTaken, ForceMode2D.Impulse);
    }

    void OnUpdateDAMAGE() //UPDATE OF THE DAMAGE STATE
    {
        //Debug.Log("SlimeState DAMAGE");
        mFsm.SetCurrentState(SlimeState.DAMAGE);

       
        //Debug.Log("Slime : " + _CurrentHealth);

    }

    /* --------------------------DIE------------------------------- */

    void OnEnterDIE()
    {
        //Debug.Log("Enter the " + mFsm.GetCurrentState().Name.ToString());
    }

    void OnUpdateDIE()
    {
        mFsm.SetCurrentState(SlimeState.DIE);
        _Die = Die();
        StartCoroutine(_Die);
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

    private IEnumerator AttackPlayer() //Wander
    {
        //Debug.Log("ATTACCKKKKKK");
        TileDetection t = FindFirstObjectByType<TileDetection>();
        while (true)
        {
            _Rigidbody2D.velocity = Vector2.zero;
            yield return new WaitForSeconds(0.5f);
            if (t.CanJump())
                _Rigidbody2D.velocity = new Vector2(DirectionMouvement.x * _ForceMouvement, Mathf.Sqrt(-2.0f * Physics2D.gravity.y * _JumpHeight));
            yield return new WaitForSeconds(0.8f);
        }
    }

    private IEnumerator Die() //Die
    { 
        while(true)
        {
            Debug.Log("MEURE");
            yield return new WaitForSeconds(2.0f);
            Destroy(this.gameObject);
            StopCoroutine(_Die);
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
        MaxingSpeed();

        //Debug.Log(_Rigidbody2D.velocity);
    }

    public void MaxingSpeed()
    {
        //Vérification pour limité la vitesse en x positif
        if (_Rigidbody2D.velocity.x >= _MaxSpeed)
        {
            _Rigidbody2D.velocity = new Vector2(_MaxSpeed, _Rigidbody2D.velocity.y);
            //Vérification pour limité la vitesse en y négatif
            if (_Rigidbody2D.velocity.y <= _MaxYSpeed * -1)
                _Rigidbody2D.velocity = new Vector2(_MaxSpeed, _MaxYSpeed * -1);
        }
        //Vérification pour limité la vitesse en x négatif
        if (_Rigidbody2D.velocity.x <= _MaxSpeed * -1)
        {
            _Rigidbody2D.velocity = new Vector2(_MaxSpeed * -1, _Rigidbody2D.velocity.y);
            //Vérification pour limité la vitesse en y négatif
            if (_Rigidbody2D.velocity.y <= _MaxYSpeed * -1)
                _Rigidbody2D.velocity = new Vector2(_MaxSpeed * -1, _MaxYSpeed * -1);
        }
        //Vérification pour limité la vitesse en y négatif
        if (_Rigidbody2D.velocity.y <= _MaxYSpeed * -1)
            _Rigidbody2D.velocity = new Vector2(_Rigidbody2D.velocity.x, _MaxYSpeed * -1);
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

    void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log(_Player);
        //Debug.Log("Slime : " + _CurrentHealth);
        if (collision.gameObject.layer == LayerMask.NameToLayer("Tool"))
        {
            Item collisionObject = collision.gameObject.GetComponent<Item>();
            //Debug.Log(collision.gameObject);
            if (collisionObject != null)
            {
                if (_CurrentHealth - collisionObject.DoDamage() >= 0)
                    _CurrentHealth -= collisionObject.DoDamage();
                else if (_CurrentHealth <= 0)
                    mFsm.SetCurrentState(SlimeState.DIE);
            }
            mFsm.SetCurrentState(SlimeState.DAMAGE);
        }
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            player.kBCounter = player.kBTotalTime;
            if (collision.gameObject.transform.position.x <= transform.position.x)
                player.knockRight = true;
            else
                player.knockRight = false;
            //_Player.TakeDamage(10.0f); //Deuxieme player null
        }
    }
}