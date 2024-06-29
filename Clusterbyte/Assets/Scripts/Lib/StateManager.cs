using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        private readonly List<Tuple<GameStateChangeCriteria, GameState>> stateChanges = new();
        private GameState previousState;
        private bool initCall;

        internal void Update()
        {
            // Run all criteria checks for state changes
            foreach (Tuple<GameStateChangeCriteria, GameState> sc in
                     stateChanges.Where(sc => sc.Item1()))
            {
                state = sc.Item2;
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
            stateChanges.Add(new Tuple<GameStateChangeCriteria, GameState>(criteria, nextState));
        }

        /// <summary>
        /// Manually set the state that this manager should execute.
        /// </summary>
        /// <param name="newState">the state to execute now</param>
        public void SetState(GameState newState)
        {
            if (state == null)
                newState.OnStart();
            state = newState;
            previousState ??= newState;
        }
    }
}