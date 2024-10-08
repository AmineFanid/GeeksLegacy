// FiniteStateMachine 
// Namespace contenant le skelette de la FSM
// Auteurs: https://faramira.com/finite-state-machine-using-csharp-delegates-in-unity/ Code tirer a 100% de se site.
using System;
using System.Collections.Generic;
using System.Text;
using Unity;

namespace Patterns
{
    public class FiniteStateMachine<T>
    {
        // A Finite State Machine
        //    - consists of a set of states,
        //    - and at any given time, an FSM can exist in only one 
        //      State out of these possible set of states.

        // A dictionary to represent the a set of states.
        static private int _Guid = 0;
        public int _CurrentGuid = 0;


        protected Dictionary<T, State<T>> mStates;

        // The current state.
        protected State<T> mCurrentState; //Private ?

        public FiniteStateMachine()
        {
            mStates = new Dictionary<T, State<T>>();
            _CurrentGuid = _Guid;
            _Guid++;
        }

        public void Add(State<T> state)
        {
            mStates.Add(state.ID, state);
        }

        public void Add(T stateID, State<T> state) //Inutile ? pour 2 meme etat diff�rent
        {
            mStates.Add(stateID, state);
        }

        public State<T> GetState(T stateID)
        {
            if (mStates.ContainsKey(stateID))
                return mStates[stateID];
            return null;
        }

        public void SetCurrentState(T stateID)
        {
            //Check dans le dict ?
            State<T> state = mStates[stateID];
            SetCurrentState(state);
        }

        public State<T> GetCurrentState()
        {
            return mCurrentState;
        }

        public void SetCurrentState(State<T> state)
        {
            if (mCurrentState == state) //METTRE UN PRINT POUR L'�TAT (MOINS LONG)
            {
                return;
            }

            if (mCurrentState != null)
            {
                mCurrentState.Exit();
            }

            mCurrentState = state;

            if (mCurrentState != null)
            {
                mCurrentState.Enter();
            }
        }

        public void Update()
        {
            if (mCurrentState != null)
            {
                mCurrentState.Update();
            }
        }

        public void FixedUpdate()
        {
            if (mCurrentState != null)
            {
                mCurrentState.FixedUpdate();
            }
        }
    }
}
