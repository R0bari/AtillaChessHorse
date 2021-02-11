using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AtillaChessHorse.States.FieldState;

namespace AtillaChessHorse.States
{
    public interface IState<T>
    {
        T Parent { get; set; }
        T ChangeState(MoveDirections move, Dictionary<int, T> closedStates);
        bool IsChangeStateAvailable(MoveDirections move, Dictionary<int, T> closedStates);
    }
}
