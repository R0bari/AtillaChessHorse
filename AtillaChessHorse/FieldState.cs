﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace AtillaChessHorse
{
    public class FieldState
    {
        public CellTypes[][] Cells { get; set; }
        public int Size { get; set; }
        public int ResultHorseX { get; private set; }
        public int ResultHorseY { get; private set; }
        public int HorseX { get; set; }
        public int HorseY { get; set; }
        public int KingX { get; set; }
        public int KingY { get; set; }
        public FieldState Parent { get; set; }
        public bool IsKingAlreadyReached { get; set; } = false;
        public FieldState(CellTypes[][] cells, int resultHorseX, int resultHorseY, int kingX, int kingY)
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
            
            HorseX = resultHorseX;
            HorseY = resultHorseY;
            KingX = kingX;
            KingY = kingY;
            ResultHorseX = resultHorseX;
            ResultHorseY = resultHorseY;
        }

        public FieldState MoveHorse(MoveDirections direction, Dictionary<int, FieldState> closedFields)
        {
            FieldState cloneField = (FieldState)this.Clone();
            if (IsMoveAvailable(direction, closedFields))
            {
                cloneField.ChangeHorseCoords(direction);
            }
            cloneField.Parent = this;
            if (!cloneField.IsKingAlreadyReached)
            {
                cloneField.IsKingAlreadyReached = cloneField.DetermineReachingKingStatus();
            }
            return cloneField;
        }
        public bool IsMoveAvailable(MoveDirections direction, Dictionary<int, FieldState> closedFields)
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

        private bool DetermineReachingKingStatus() => KingX == HorseX && KingY == HorseY;
        private void ReachKing() => IsKingAlreadyReached = true;
        public bool IsResult() => (IsHorseInStartPosition() && IsKingAlreadyReached);
        public bool IsHorseInStartPosition() => HorseX == ResultHorseX && HorseY == ResultHorseY;
        public override int GetHashCode()
        {
            //  77232917 - просто число Мерсенна
            long hash = IsKingAlreadyReached ? 77232917 : 0, count = 0;
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
            FieldState cloneField = new FieldState(cells: Cells, ResultHorseX, ResultHorseY, KingX, KingY)
            {
                Size = this.Size,
                HorseX = this.HorseX,
                HorseY = this.HorseY,
                IsKingAlreadyReached = this.IsKingAlreadyReached,
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
            [Description("Вверх-влево")]
            TopLeft = 0,
            [Description("Вверх-вправо")]
            TopRight = 1,
            [Description("Вправо-вверх")]
            RightTop = 2,
            [Description("Вправо-вниз")]
            RightBottom = 3,
            [Description("Вниз-право")]
            BottomRight = 4,
            [Description("Вниз-влево")]
            BottomLeft = 5,
            [Description("Влево-вниз")]
            LeftBottom = 6,
            [Description("Влево-вверх")]
            LeftTop = 7
        }
    }
}