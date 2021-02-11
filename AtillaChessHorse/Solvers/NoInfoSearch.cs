using System;
using System.Collections.Generic;
using static AtillaChessHorse.FieldState;

namespace AtillaChessHorse.Solvers
{
    public abstract class NoInfoSearch : ISearch
    {
        protected IEnumerable<FieldState> OpenStates { get; set; }
        protected readonly Dictionary<int, FieldState> closedStates = new Dictionary<int, FieldState>();
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

        public List<FieldState> Search(FieldState initState)
        {
            List<FieldState> availableStates = DetermineAvailableFieldStates(initState);
            if (availableStates.Count == 0)
            {
                throw new Exception("No ways available at start");
            }
            AddToOpenStates(availableStates);

            do
            {
                if (PeekFromOpenStates().IsResult())
                {
                    return FormResultWay(PeekFromOpenStates());
                }
                AddToOpenStates(DetermineAvailableFieldStates(DeleteFromOpenStates()));
            } while (PeekFromOpenStates() != null);

            return new List<FieldState>();
        }
        protected abstract FieldState PeekFromOpenStates();
        protected abstract FieldState DeleteFromOpenStates();
        protected abstract void AddToOpenStates(FieldState state);
        protected void AddToOpenStates(IEnumerable<FieldState> states)
        {
            foreach(var state in states)
            {
                AddToOpenStates(state);
            }
        }

        protected List<FieldState> DetermineAvailableFieldStates(FieldState state)
        {
            List<FieldState> availableStates = new List<FieldState>();
            foreach (MoveDirections move in AllMoves)
            {
                if (state.IsMoveAvailable(move, closedStates))
                {
                    availableStates.Add(state.MoveHorse(move, closedStates));
                }
            }
            return availableStates;
        }
        protected List<FieldState> FormResultWay(FieldState state)
        {
            FieldState currentState = (FieldState)state.Clone();
            List<FieldState> way = new List<FieldState>() { state };

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
