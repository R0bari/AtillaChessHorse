using System;
using System.Collections.Generic;
using static AtillaChessHorse.FieldState;

namespace AtillaChessHorse.Solvers
{
    public abstract class Solver : ISolver
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

        public abstract List<FieldState> Solve(FieldState initState);
        protected abstract void AddToOpenStates(IEnumerable<FieldState> states);

        protected IEnumerable<FieldState> DetermineAvailableFieldStates(FieldState state)
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
