using UnityEngine;
using Patterns;
using Unity.VisualScripting;
using System.Net.NetworkInformation;

public class Slime : MonoBehaviour
{
    FiniteStateMachine<SlimeState> mFsm = new FiniteStateMachine<SlimeState>();
    public enum SlimeState 
    {
        IDLE,
        CHASE,
        ATTACK,
        DAMAGE,
        DIE,
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mFsm.Add(
            new State<SlimeState>(
                SlimeState.IDLE,
                "IDLE",
                OnEnterIDLE,
                null,
                OnUpdateIDLE,
                null)
            );
        /*Init_IdleState();
        Init_AttackState();
        Init_DieState();
        Init_DamageState();
        Init_ChaseState();*/
    }

    private void Update()
    {
        mFsm.Update();
    }

    private void FixedUpdate()
    {
        mFsm.FixedUpdate();
    }

    #region Delegates implementation for the states.
    void OnEnterIDLE()
    {
        Debug.Log("Enter the " + mFsm.GetCurrentState().Name.ToString());
    }

    void OnUpdateIDLE()
    {
        Debug.Log("SlimeState IDLE");
        mFsm.SetCurrentState(SlimeState.IDLE);
    }

    void OnEnterCHASE()
    {
        Debug.Log("Enter the " + mFsm.GetCurrentState().Name.ToString());
    }

    void OnUpdateCHASE()
    {
        Debug.Log("SlimeState IDLE");
        mFsm.SetCurrentState(SlimeState.CHASE);
    }

    void OnEnterATTACK() 
    { 
        
    }

    void OnUpdateATTACK()
    {
        Debug.Log("SlimeState IDLE");
        mFsm.SetCurrentState(SlimeState.ATTACK);
    }

    void OnEnterDAMAGE()
    {

    }

    void OnUpdateDAMAGE()
    {
        Debug.Log("SlimeState IDLE");
        mFsm.SetCurrentState(SlimeState.DAMAGE);
    }

    void OnEnterDIE()
    {

    }

    void OnUpdateDIE()
    {
        Debug.Log("SlimeState IDLE");
        mFsm.SetCurrentState(SlimeState.DIE);
    }
    #endregion
}