using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtillaChessHorse.Solvers
{
    public class NoInfoWidthSolver : Solver
    {
        public NoInfoWidthSolver()
        {
            OpenStates = new Stack<FieldState>();
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
                if ((OpenStates as Stack<FieldState>).Peek().IsResult())
                {
                    return FormResultWay((OpenStates as Stack<FieldState>).Peek());
                }
                AddToOpenStates(DetermineAvailableFieldStates((OpenStates as Stack<FieldState>).Pop()));
            } while ((OpenStates as Stack<FieldState>).Peek() != null);

            return new List<FieldState>();
        }

        protected override void AddToOpenStates(IEnumerable<FieldState> states)
        {
            foreach (FieldState state in states)
            {
                (OpenStates as Stack<FieldState>).Push(state);
            }
        }
    }
}
