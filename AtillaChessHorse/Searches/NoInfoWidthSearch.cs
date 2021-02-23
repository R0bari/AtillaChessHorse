using AtillaChessHorse.States;
using Priority_Queue;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AtillaChessHorse.Searches
{
    public class NoInfoWidthSearch : NoInfoSearch
    {
        public NoInfoWidthSearch()
        {
            OpenStates = new SimplePriorityQueue<IState>();
        }
        protected override IState DeleteFromOpen() => (OpenStates as SimplePriorityQueue<IState>).Dequeue();
        protected override void AddToOpenStates(IState state) => (OpenStates as SimplePriorityQueue<IState>)
            .Enqueue(state, (float)state.CurrentHeuristicValue);
        //  Sort asc because after adding to queue the state with 
        //  the best (least) heuristic will be dequeue first
        protected override IEnumerable<IState> OrderByHeuristic(IEnumerable<IState> states) =>
            states.OrderBy(state => state.CalculateHeuristic());
        protected override IState DeleteWorstStateFromOpen()
        {
            OpenStates = (OpenStates as SimplePriorityQueue<IState>).OrderByDescending(key => key.CurrentHeuristicValue);
            var deletedState = DeleteFromOpen();
            OpenStates = (OpenStates as SimplePriorityQueue<IState>).OrderBy(key => key.CurrentHeuristicValue);
            return deletedState;
        }
    }
}
