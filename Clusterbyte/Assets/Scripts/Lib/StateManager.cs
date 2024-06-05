using System;
using System.Collections;
using UnityEngine;

namespace Lib
{
    /// <summary>
    /// Generic game state manager to be implemented by a Game Manager.
    /// </summary>
    public abstract class StateManager : MonoBehaviour
    {
        /// <summary>
        /// Current state as represented by this manager.
        /// </summary>
        public GameState state { get; private set; }

        /// <summary>
        /// A bool supplier to determine when a game state change should happen.
        /// </summary>
        protected delegate bool GameStateChangeCriteria();

        private readonly ArrayList stateChanges = new();
        private GameState previousState;

        internal void Update()
        {
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                Application.Quit();
            }

            // Run all criteria checks for state changes
            foreach (StateChange sc in stateChanges)
            {
                if (!sc.Criteria()) continue;
                state = sc.NextState;
                break;
            }

            // Sometimes, the state may be null as it has not been set yet
            if (state == null) return;

            // Handle switching of states by ending the old one and starting the new one
            if (state != previousState)
            {
                Debug.Log($"State changed from {previousState} to {state}.");
                previousState.OnEnd();
                state.OnStart();
            }

            // Run periodic methods and update last known states
            state.Periodic();
            previousState = state;
        }

        /// <summary>
        /// Add a boolean supplier to a GameState when this manager should switch to this GameState
        /// </summary>
        /// <param name="criteria">when true, nextState will be the new state</param>
        /// <param name="nextState">the new state to change to when the criteria is met</param>
        protected void ChangeStateOnEvent(GameStateChangeCriteria criteria, GameState nextState)
        {
            stateChanges.Add(new StateChange(criteria, nextState));
        }

        /// <summary>
        /// Manually set the state that this manager should execute.
        /// </summary>
        /// <param name="newState">the state to execute now</param>
        public void SetState(GameState newState)
        {
            state = newState;
            state.OnStart();
            previousState ??= newState;
        }

        // Represents a pair of objects being the delegate and next state
        private class StateChange
        {
            public GameStateChangeCriteria Criteria { get; }
            public GameState NextState { get; }

            public StateChange(GameStateChangeCriteria criteria, GameState nextState)
            {
                Criteria = criteria;
                NextState = nextState;
            }
        }
    }
}