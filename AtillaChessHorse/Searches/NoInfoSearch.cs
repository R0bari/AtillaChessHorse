using AtillaChessHorse.States;
using System;
using System.Collections.Generic;
using static AtillaChessHorse.States.FieldState;

namespace AtillaChessHorse.Searches
{
    public abstract class NoInfoSearch : ISearch
    {
        protected IEnumerable<IState> OpenStates { get; set; }
        private readonly Dictionary<int, IState> ClosedStates = new Dictionary<int, IState>();
        private readonly MoveDirections[] AllMoves = new MoveDirections[]
        {
                MoveDirections.TopLeft,
                MoveDirections.TopRight,
                MoveDirections.RightTop,
                MoveDirections.RightBottom,
                MoveDirections.BottomRight,
                MoveDirections.BottomLeft,
                MoveDirections.LeftBottom,
                MoveDirections.LeftTop
        };

        public List<IState> Search(IState initState)
        {
            List<IState> availableStates = DetermineAvailableStates(initState);
            if (availableStates.Count == 0)
            {
                throw new Exception("No states available at start");
            }
            AddToOpenStates(availableStates);

            IState currentState = DeleteFromOpenStates();
            do
            {
                if (currentState.IsResult())
                {
                    return FormResultStateSequence(currentState);
                }
                AddToOpenStates(DetermineAvailableStates(currentState));
                AddToClosedStates(currentState);
                currentState = DeleteFromOpenStates();
            } while (currentState != null);
            throw new Exception("Way not found");
        }
        protected abstract IState DeleteFromOpenStates();
        protected abstract void AddToOpenStates(IState state);
        protected void AddToOpenStates(IEnumerable<IState> states)
        {
            foreach(var state in states)
            {
                AddToOpenStates(state);
            }
        }
        private void AddToClosedStates(IState state)
        {
            int stateHash = state.GetHashCode();
            if (!ClosedStates.ContainsKey(stateHash))
            {
                ClosedStates.Add(stateHash, state);
            }
        }

        protected List<IState> DetermineAvailableStates(IState state)
        {
            List<IState> availableStates = new List<IState>();
            foreach (MoveDirections move in AllMoves)
            {
                if (state.IsChangeStateAvailable(move, ClosedStates))
                {
                    availableStates.Add(state.ChangeState(move, ClosedStates));
                }
            }
            return availableStates;
        }
        protected List<IState> FormResultStateSequence(IState state)
        {
            IState currentState = (IState)state.Clone();
            List<IState> way = new List<IState>() { state };

            while (currentState.Parent != null)
            {
                way.Add(currentState.Parent);
                currentState = currentState.Parent;
            }
            way.Reverse();
            return way;
        }
    }
}
