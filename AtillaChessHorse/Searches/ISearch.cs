using AtillaChessHorse.States;
using System.Collections.Generic;


namespace AtillaChessHorse.Searches
{
    public interface ISearch
    {
           int OpenStatesMaxSize { get; }
           List<IState> Search(IState initState);
    }
}
