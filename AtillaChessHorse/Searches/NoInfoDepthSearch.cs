using AtillaChessHorse.States;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AtillaChessHorse.Searches
{
    public class NoInfoDepthSearch : NoInfoSearch
    {
        public NoInfoDepthSearch()
        {
            OpenStates = new Stack<IState>();
        }
        protected override IState DeleteFromOpen() => (OpenStates as Stack<IState>).Pop();
        protected override void AddToOpen(IState state) => (OpenStates as Stack<IState>).Push(state);
        //  Sort desc because after adding to stack the state with 
        //  the best (least) heuristic will be pop first
        protected override IEnumerable<IState> OrderByHeuristic(IEnumerable<IState> states) =>
            states.OrderByDescending(state => state.CalculateHeuristic());
        protected override IState DeleteWorstStateFromOpen()
        {
            throw new NotImplementedException();
        }
    }
}
