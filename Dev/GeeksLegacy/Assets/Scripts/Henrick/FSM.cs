using System.Collections.Generic;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

/*
Le code a été tirer de ce site : https://faramira.com/generic-finite-state-machine-using-csharp/
Aussi aidé à l'aide de : https://faramira.com/enemy-behaviour-with-finite-state-machine-using-csharp-delegates-in-unity/
 */

//Namespaces are used in C# to organize and provide a level of separation of codes. (Google definition)
namespace FSM
{
    //<T> symbol for a generic type parameter. (Google definition)
    public class State<T>
    {
        //if we want to pass a function as a parameter? How does C# handles
        //the callback functions or event handler? The answer is - delegate. (Google definition)
        public delegate void DelegateNoArg();
        public DelegateNoArg OnEnter;
        public DelegateNoArg OnExit;
        public DelegateNoArg OnUpdate;
        public DelegateNoArg OnFixedUpdate;

        //Collection représentant un set de state
        protected Dictionary<T, State<T>> mStates;

        // Le state actuel
        protected State<T> mCurrentState;

        // Nom du state
        public string Name { get; set; } //getter et setter

        // Id du state
        public T ID { get; private set; } //getter et setter

        //constructor
        public State(T id)
        {
            ID = id;
        }

        //constructor
        public State(T id, string name) : this(id)
        {
            Name = name;
        }

        //default constructor
        public void FiniteStateMachine()
        {
            mStates = new Dictionary<T, State<T>>();
        }

        //add a new state
        public void Add(State<T> state)
        {
            mStates.Add(state.ID, state);
        }
        public void Add(T stateID, State<T> state)
        {
            mStates.Add(stateID, state);
        }

        //returns a State based on the key
        public State<T> GetState(T stateID) //will return null if a State of the same key has not been added previously to the FSM
        {
            if (mStates.ContainsKey(stateID))
                return mStates[stateID];
            return null;
        }

        //Virtual methods are method that can be overridden in a derived class (Google definition)
        virtual public void Enter()
        {
            OnEnter?.Invoke();
        }
        virtual public void Exit()
        {
            OnExit?.Invoke();
        }
        virtual public void Update()
        {
            OnUpdate?.Invoke();
        }
        virtual public void FixedUpdate()
        {
            OnFixedUpdate?.Invoke();
        }

        public void SetCurrentState(State<T> state)
        {
            if (mCurrentState == state)
            {
                return;
            }
            if (mCurrentState != null)
            {
                mCurrentState.Exit();
                mCurrentState = state;
                mCurrentState.Enter();
            }
        }

        public State(T id,
            DelegateNoArg onEnter,
            DelegateNoArg onExit = null,
            DelegateNoArg onUpdate = null,
            DelegateNoArg onFixedUpdate = null) : this(id)
        {
            OnEnter = onEnter;
            OnExit = onExit;
            OnUpdate = onUpdate;
            OnFixedUpdate = onFixedUpdate;
        }
        public State(T id,
            string name,
            DelegateNoArg onEnter,
            DelegateNoArg onExit = null,
            DelegateNoArg onUpdate = null,
            DelegateNoArg onFixedUpdate = null) : this(id, name)
        {
            OnEnter = onEnter;
            OnExit = onExit;
            OnUpdate = onUpdate;
            OnFixedUpdate = onFixedUpdate;
        }
    }
}

//EX D'UTILISATION : 

        /*mFsm = new FSM();
            mFsm.Add((int)StateTypes.IDLE, new NPCState(mFsm, StateTypes.IDLE, this));
            mFsm.Add((int)StateTypes.CHASE, new NPCState(mFsm, StateTypes.CHASE, this));
            mFsm.Add((int)StateTypes.ATTACK, new NPCState(mFsm, StateTypes.ATTACK, this));
            mFsm.Add((int)StateTypes.DAMAGE, new NPCState(mFsm, StateTypes.DAMAGE, this));
            mFsm.Add((int)StateTypes.DIE, new NPCState(mFsm, StateTypes.DIE, this));
            Init_IdleState();
            Init_AttackState();
            Init_DieState();
            Init_DamageState();
            Init_ChaseState();
            mFsm.SetCurrentState(mFsm.GetState((int)StateTypes.IDLE));*/