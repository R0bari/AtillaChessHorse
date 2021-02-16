﻿using AtillaChessHorse.States;
using System.Collections.Generic;


namespace AtillaChessHorse.Searches
{
    public interface ISearch
    {
        List<IState> Search(IState initState);
    }
}