using AtillaChessHorse.States;
using System;
using System.Collections.Generic;
using static AtillaChessHorse.States.FieldState;

namespace AtillaChessHorse.Solvers
{
    public abstract class NoInfoSearch : ISearch
    {
        protected IEnumerable<IState> OpenStates { get; set; }
        protected readonly Dictionary<int, IState> closedStates = new Dictionary<int, IState>();
        protected MoveDirections[] AllMoves = new MoveDirections[]
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

            do
            {
                if (PeekFromOpenStates().IsResult())
                {
                    return FormResultStateSequence(PeekFromOpenStates());
                }
                AddToOpenStates(DetermineAvailableStates(DeleteFromOpenStates()));
            } while (PeekFromOpenStates() != null);

            return new List<IState>();
        }
        protected abstract IState PeekFromOpenStates();
        protected abstract IState DeleteFromOpenStates();
        protected abstract void AddToOpenStates(IState state);
        protected void AddToOpenStates(IEnumerable<IState> states)
        {
            foreach(var state in states)
            {
                AddToOpenStates(state);
            }
        }

        protected List<IState> DetermineAvailableStates(IState state)
        {
            List<IState> availableStates = new List<IState>();
            foreach (MoveDirections move in AllMoves)
            {
                if (state.IsChangeStateAvailable(move, closedStates))
                {
                    availableStates.Add(state.ChangeState(move, closedStates));
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
