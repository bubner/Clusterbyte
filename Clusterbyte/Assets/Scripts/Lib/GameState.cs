using System;

namespace Lib
{
    /// <summary>
    /// A generic three-phase executive callback cycle.
    /// </summary>
    public class GameState
    {
        private static int idCounter;

        /// <summary>
        /// Unique ID of this GameState, in order of creation with other GameStates.
        /// </summary>
        public readonly int id = idCounter++;
        public Action OnStart { get; private set; }
        public Action Periodic { get; private set; }
        public Action OnEnd { get; private set; }

        /// <summary>
        /// Constructs a new GameState.
        /// </summary>
        /// <param name="onStart">Will be called once when this state is first activated.</param>
        /// <param name="periodic">Will be called once every game frame during the phase's execution.</param>
        /// <param name="onEnd">Will be called once when this state is removed or replaced.</param>
        public GameState(Action onStart, Action periodic, Action onEnd)
        {
            OnStart = onStart ?? (() => { });
            Periodic = periodic ?? (() => { });
            OnEnd = onEnd ?? (() => { });
        }
        
        public override string ToString()
        {
            return $"id:{id}";
        }
        
        public override bool Equals(object obj)
        {
            return obj is GameState other && id == other.id;
        }

        protected bool Equals(GameState other)
        {
            return id == other.id;
        }

        public override int GetHashCode()
        {
            return id;
        }

        public static bool operator ==(GameState a, GameState b)
        {
            return a?.id == b?.id;
        }

        public static bool operator !=(GameState a, GameState b)
        {
            return a?.id != b?.id;
        }
    }
}