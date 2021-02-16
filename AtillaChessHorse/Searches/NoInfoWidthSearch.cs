using AtillaChessHorse.States;
using System.Collections.Generic;
using System.Linq;

namespace AtillaChessHorse.Searches
{
    public class NoInfoWidthSearch : NoInfoSearch
    {
        public NoInfoWidthSearch()
        {
            OpenStates = new Queue<IState>();
        }
        protected override IState DeleteFromOpenStates() => (OpenStates as Queue<IState>).Dequeue();
        protected override void AddToOpenStates(IState state) => (OpenStates as Queue<IState>).Enqueue(state);
        //  Sort asc because after adding to queue the state with 
        //  the best (least) heuristic will be dequeue first
        protected override IEnumerable<IState> OrderByHeuristic(IEnumerable<IState> states) =>
            states.OrderBy(state => state.CalculateHeuristic());
    }
}
