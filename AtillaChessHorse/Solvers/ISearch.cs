using AtillaChessHorse.States;
using System.Collections.Generic;


namespace AtillaChessHorse.Solvers
{
    public interface ISearch
    {
        List<IState> Search(IState initState);
    }
}
