using System;
using System.Collections.Generic;
using static AtillaChessHorse.States.FieldState;

namespace AtillaChessHorse.States
{
    public interface IState : ICloneable
    {
        Heuristics CurrentHeuristicType { get; set; }
        int CurrentHeuristicValue { get; set; }
        IState Parent { get; set; }
        IState ChangeState(MoveDirections move, Dictionary<int, IState> closedStates);
        bool IsChangeStateAvailable(MoveDirections move, Dictionary<int, IState> closedStates);
        int CalculateHeuristic();
        bool IsResult();
    }
}
