using System;
using System.Collections.Generic;
using static AtillaChessHorse.States.FieldState;

namespace AtillaChessHorse.States
{
    public interface IState : ICloneable
    {
        IState Parent { get; set; }
        IState ChangeState(MoveDirections move, Dictionary<int, IState> closedStates);
        bool IsChangeStateAvailable(MoveDirections move, Dictionary<int, IState> closedStates);
        bool IsResult();
    }
}
