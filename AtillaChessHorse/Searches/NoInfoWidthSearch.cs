using AtillaChessHorse.States;
using System.Collections.Generic;

namespace AtillaChessHorse.Searches
{
    public class NoInfoWidthSearch : NoInfoSearch
    {
        public NoInfoWidthSearch()
        {
            OpenStates = new Queue<IState>();
        }
        protected override IState PeekFromOpenStates() => (OpenStates as Queue<IState>).Peek();
        protected override IState DeleteFromOpenStates() => (OpenStates as Queue<IState>).Dequeue();
        protected override void AddToOpenStates(IState state) => (OpenStates as Queue<IState>).Enqueue(state);
    }
}
