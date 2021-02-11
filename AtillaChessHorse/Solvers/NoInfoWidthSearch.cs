using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtillaChessHorse.Solvers
{
    public class NoInfoWidthSearch : NoInfoSearch
    {
        public NoInfoWidthSearch()
        {
            OpenStates = new Queue<FieldState>();
        }
        protected override FieldState PeekFromOpenStates() => (OpenStates as Queue<FieldState>).Peek();
        protected override FieldState DeleteFromOpenStates() => (OpenStates as Queue<FieldState>).Dequeue();
        protected override void AddToOpenStates(FieldState state) => (OpenStates as Queue<FieldState>).Enqueue(state);
    }
}
