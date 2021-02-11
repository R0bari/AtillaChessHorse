using AtillaChessHorse.States;
using System.Collections.Generic;


namespace AtillaChessHorse.Solvers
{
    public interface ISearch<T>
    {
        List<T> Search(IState<T> initState);
    }
}
