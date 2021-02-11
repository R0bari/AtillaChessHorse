using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtillaChessHorse.Solvers
{
    public class NoInfoDepthSearch : NoInfoSearch
    {
        public NoInfoDepthSearch()
        {
            OpenStates = new Stack<FieldState>();
        }
        protected override FieldState PeekFromOpenStates() => (OpenStates as Stack<FieldState>).Peek();
        protected override FieldState DeleteFromOpenStates() => (OpenStates as Stack<FieldState>).Pop();
        protected override void AddToOpenStates(FieldState state) => (OpenStates as Stack<FieldState>).Push(state);
    }
}
