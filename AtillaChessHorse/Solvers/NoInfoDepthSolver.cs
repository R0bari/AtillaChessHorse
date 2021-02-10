using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtillaChessHorse.Solvers
{
    public class NoInfoDepthSolver : Solver
    {
        private readonly Stack<FieldState> openStates = new Stack<FieldState>();
        private readonly Dictionary<int, FieldState> closedStates = new Dictionary<int, FieldState>();
        public NoInfoDepthSolver()
        {
        }
        public override List<FieldState> Solve(FieldState initState)
        {
            IEnumerable<FieldState> availableStates = DetermineAvailableFieldStates(initState);
            if (availableStates.Count() == 0)
            {
                throw new Exception("No ways available at start");
            }
            AddToOpenStates(availableStates);

            do
            {
                if (openStates.Peek().IsResult())
                {
                    return FormResultWay(openStates.Peek());
                }
                AddToOpenStates(DetermineAvailableFieldStates(openStates.Pop()));
            } while (openStates.Peek() != null);
            
            return new List<FieldState>();
        }
        private IEnumerable<FieldState> DetermineAvailableFieldStates(FieldState state)
        {
            List<FieldState> availableStates = new List<FieldState>();
            foreach (FieldState.MoveDirections move in AllMoves)
            {
                if (state.IsMoveAvailable(move, closedStates))
                {
                    availableStates.Add(state.MoveHorse(move, closedStates));
                }
            }
            return availableStates;
        }
        private List<FieldState> FormResultWay(FieldState state)
        {
            FieldState currentState = (FieldState)state.Clone();
            List<FieldState> way = new List<FieldState>() {state};

            while (currentState.Parent != null)
            {
                way.Add(currentState.Parent);
                currentState = currentState.Parent;
            }
            way.Reverse();
            return way;
        }

        private void AddToOpenStates(IEnumerable<FieldState> states)
        {
            foreach (FieldState state in states)
            {
                openStates.Push(state);
            }
        }
    }
}
