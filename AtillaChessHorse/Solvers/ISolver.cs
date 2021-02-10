using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtillaChessHorse.Solvers
{
    public interface ISolver
    {
        List<FieldState> Solve(FieldState initState);
    }
}
