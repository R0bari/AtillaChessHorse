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
        protected override void AddToOpen(IState state) => (OpenStates as SimplePriorityQueue<IState>)
            .Enqueue(state, (float)state.CurrentHeuristicValue);
        //  Sort asc because after adding to queue the state with 
        //  the best (least) heuristic will be dequeue first
        protected override IEnumerable<IState> OrderByHeuristic(IEnumerable<IState> states) =>
            states.OrderBy(state => state.CalculateHeuristic());
        protected override IState DeleteWorstStateFromOpen()
        {
            var queueElements = OpenStates.OrderByDescending(key => key.CurrentHeuristicValue).ToList();
            var deletedState = queueElements[0];
            queueElements.Remove(deletedState);
            queueElements = queueElements.OrderBy(key => key.CurrentHeuristicValue).ToList();
            while (OpenStates.Count() > 0)
            {
                DeleteFromOpen();
            }
            foreach (var state in queueElements)
            {
                AddToOpen(state);
            }

            return deletedState;
        }
    }
}
