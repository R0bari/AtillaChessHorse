using System;
using System.Collections.Generic;
using static AtillaChessHorse.FieldState;

namespace AtillaChessHorse.Solvers
{
    public abstract class Solver : ISolver
    {
        public MoveDirections[] AllMoves = new MoveDirections[]
        {
                MoveDirections.TopLeft,
                MoveDirections.TopRight,
                MoveDirections.RightTop,
                MoveDirections.RightBottom,
                MoveDirections.BottomRight,
                MoveDirections.BottomLeft,
                MoveDirections.LeftBottom,
                MoveDirections.LeftTop
        };

        public abstract List<FieldState> Solve(FieldState initState);
    }
}
