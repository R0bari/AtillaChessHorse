using System;
using System.Collections.Generic;
using System.Text;

namespace AtillaChessHorse.States
{
    public class FieldState : IState
    {
        public CellTypes[][] Cells { get; set; }
        public int Size { get; set; }
        public int ResultHorseX { get; private set; }
        public int ResultHorseY { get; private set; }
        public int HorseX { get; set; }
        public int HorseY { get; set; }
        public int KingX { get; set; }
        public int KingY { get; set; }
        public IState Parent { get; set; }
        public Heuristics CurrentHeuristicType { get; set; } = Heuristics.ManhattanDistance;
        public int CurrentHeuristicValue { get; set; }
        public bool IsKingAlreadyReached { get; set; } = false;
        public FieldState(CellTypes[][] cells, int resultHorseX, int resultHorseY)
        {
            Size = cells.Length;
            Cells = new CellTypes[cells.Length][];
            for (int i = 0; i < Cells.Length; ++i)
            {
                Cells[i] = new CellTypes[cells.Length];
                for (int j = 0; j < Cells.Length; ++j)
                {
                    Cells[i][j] = cells[i][j];
                }
            }
            var horseCoords = DeterminePosition(CellTypes.H);
            var kingCoords = DeterminePosition(CellTypes.K);

            HorseX = horseCoords.Item1;
            HorseY = horseCoords.Item2;
            KingX = kingCoords.Item1;
            KingY = kingCoords.Item2;
            ResultHorseX = resultHorseX;
            ResultHorseY = resultHorseY;
        }

        public IState ChangeState(MoveDirections direction, Dictionary<int, IState> closedFields)
        {
            FieldState cloneField = (FieldState)this.Clone();
            if (IsChangeStateAvailable(direction, closedFields))
            {
                cloneField.ChangeHorseCoords(direction);
            }
            cloneField.Parent = this;
            if (!cloneField.IsKingAlreadyReached)
            {
                cloneField.IsKingAlreadyReached = cloneField.DetermineReachingKingStatus();
            }
            cloneField.CalculateHeuristic();
            return cloneField;
        }
        public bool IsChangeStateAvailable(MoveDirections direction, Dictionary<int, IState> closedFields)
        {
            if (HorseX + GetMoveOffsetX(direction) >= Size || HorseX + GetMoveOffsetX(direction) < 0
                    || HorseY + GetMoveOffsetY(direction) >= Size || HorseY + GetMoveOffsetY(direction) < 0)
            {
                return false;
            }

            if (Cells[HorseY + GetMoveOffsetY(direction)][HorseX + GetMoveOffsetX(direction)] == CellTypes.D)
            {
                return false;
            }
            if (HorseX + GetMoveOffsetX(direction) == ResultHorseX && HorseY + GetMoveOffsetY(direction) == ResultHorseY
                    && !IsKingAlreadyReached)
            {
                return false;
            }

            FieldState cloneField = (FieldState)this.Clone();
            cloneField.ChangeHorseCoords(direction);
            if (closedFields.ContainsKey(cloneField.GetHashCode()))
            {
                return false;
            }

            return true;
        }
        public int CalculateHeuristic()
        {
            Tuple<int, int> aim = IsKingAlreadyReached ? Tuple.Create(ResultHorseX, ResultHorseY) : Tuple.Create(KingX, KingY);
            switch (CurrentHeuristicType)
            {
                case Heuristics.ManhattanDistance:
                    CurrentHeuristicValue = Math.Abs(HorseX - aim.Item1) + Math.Abs(HorseY - aim.Item2);
                    return CurrentHeuristicValue;
                default: return -1;
            }
        }
        private void ChangeHorseCoords(MoveDirections direction)
        {
            try
            {
                //  Если положение не стартовое, блокируем клетку
                Cells[HorseY][HorseX] = IsHorseInStartPosition() ? CellTypes.A : CellTypes.D;
                //  Изменяем положение коня
                HorseX += GetMoveOffsetX(direction);
                HorseY += GetMoveOffsetY(direction);
                //  Изменяем состояние новой клетки
                Cells[HorseY][HorseX] = CellTypes.H;
            }
            catch
            {
                throw new Exception($"{direction} is impossible move.");
            }
        }
        private int GetMoveOffsetX(MoveDirections direction)
        {
            if (direction == MoveDirections.TopLeft || direction == MoveDirections.BottomLeft)
            {
                return -1;
            }
            if (direction == MoveDirections.LeftTop || direction == MoveDirections.LeftBottom)
            {
                return -2;
            }
            if (direction == MoveDirections.TopRight || direction == MoveDirections.BottomRight)
            {
                return +1;
            }
            if (direction == MoveDirections.RightTop || direction == MoveDirections.RightBottom)
            {
                return +2;
            }
            throw new Exception("Wrong kind of move.");
        }
        private int GetMoveOffsetY(MoveDirections direction)
        {
            if (direction == MoveDirections.TopLeft || direction == MoveDirections.TopRight)
            {
                return -2;
            }
            if (direction == MoveDirections.LeftTop || direction == MoveDirections.RightTop)
            {
                return -1;
            }
            if (direction == MoveDirections.LeftBottom || direction == MoveDirections.RightBottom)
            {
                return +1;
            }
            if (direction == MoveDirections.BottomLeft || direction == MoveDirections.BottomRight)
            {
                return +2;
            }
            throw new Exception("Wrong kind of move.");
        }

        private Tuple<int, int> DeterminePosition(CellTypes cellTypes)
        {
            for (int i = 0; i < Size; ++i)
            {
                for (int j = 0; j < Size; ++j)
                {
                    if (Cells[i][j] == cellTypes)
                    {
                        return Tuple.Create(j, i);
                    }
                }
            }
            return Tuple.Create(-1, -1);
        }
        private bool DetermineReachingKingStatus() => KingX == HorseX && KingY == HorseY;
        public bool IsResult() => (IsHorseInStartPosition() && IsKingAlreadyReached);
        public bool IsHorseInStartPosition() => HorseX == ResultHorseX && HorseY == ResultHorseY;

        public int CompareTo(object other)
        {
            if (other == null || !(other is FieldState otherState))
            {
                return 1;
            }

            int currentHeuristic = this.CalculateHeuristic();
            int otherHeuristic = otherState.CalculateHeuristic();
            if (currentHeuristic < otherHeuristic)
                return 1;
            else if (currentHeuristic > otherHeuristic)
                return -1;
            else
                return 0;
        }

        public override int GetHashCode()
        {
            //  77232917 - просто число Мерсенна
            long hash = IsKingAlreadyReached ? 77232917 : 0, count = 0;
            hash += (int)Math.Pow(CurrentHeuristicValue, 13);
            for (int i = 0; i < Size; ++i)
            {
                for (int j = 0; j < Size; ++j)
                {
                    hash += (int)Math.Pow((int)Cells[i][j], count);
                    ++count;
                }
            }
            return (int)hash;
        }
        public override string ToString()
        {
            StringBuilder chessFieldString = new StringBuilder();
            for (int i = 0; i < Size; ++i)
            {
                for (int j = 0; j < Size; ++j)
                {
                    chessFieldString.Append($"{Cells[i][j]}\t");
                }
                chessFieldString.Append(Environment.NewLine);
            }
            return chessFieldString.ToString();
        }
        public override bool Equals(object obj)
        {
            if (!(obj is FieldState))
            {
                return false;
            }
            FieldState anotherField = (FieldState)obj;
            return anotherField.GetHashCode() == this.GetHashCode();
        }
        public object Clone()
        {
            FieldState cloneField = new FieldState(cells: Cells, ResultHorseX, ResultHorseY)
            {
                Size = this.Size,
                HorseX = this.HorseX,
                HorseY = this.HorseY,
                IsKingAlreadyReached = this.IsKingAlreadyReached,
                CurrentHeuristicValue = this.CalculateHeuristic(),
                Parent = this.Parent
            };

            for (int i = 0; i < this.Size; ++i)
            {
                for (int j = 0; j < this.Size; ++j)
                {
                    cloneField.Cells[i][j] = this.Cells[i][j];
                }
            }

            return cloneField;
        }

        public enum CellTypes
        {
            A = 0,
            D = 1,
            H = 2,
            K = 3
        }
        public enum MoveDirections
        {
            TopLeft = 0,
            TopRight = 1,
            RightTop = 2,
            RightBottom = 3,
            BottomRight = 4,
            BottomLeft = 5,
            LeftBottom = 6,
            LeftTop = 7
        }
        public enum Heuristics 
        {
            ManhattanDistance = 0
        }
    }
}
