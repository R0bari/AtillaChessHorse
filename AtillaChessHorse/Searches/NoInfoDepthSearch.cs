﻿using AtillaChessHorse.States;
using System.Collections.Generic;

namespace AtillaChessHorse.Searches
{
    public class NoInfoDepthSearch : NoInfoSearch
    {
        public NoInfoDepthSearch()
        {
            OpenStates = new Stack<IState>();
        }
        protected override IState DeleteFromOpenStates() => (OpenStates as Stack<IState>).Pop();
        protected override void AddToOpenStates(IState state) => (OpenStates as Stack<IState>).Push(state);
    }
}
