using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtillaChessHorse
{
    public class ChessField : ICloneable
    {
        public static MoveDirections[] AllMoves = new MoveDirections[]
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
        private static Dictionary<int, ChessField> _previousFields { get; set; } = new Dictionary<int, ChessField>();
        public CellTypes[][] Cells { get; set; }
        public int Size { get; set; }
        public int HorseX { get; set; }
        public int HorseY { get; set; }
        public int KingX { get; set; }
        public int KingY { get; set; }
        private ChessField(int size = 8)
        {
            Cells = new CellTypes[size][];
            for (int i = 0; i < size; ++i)
            {
                Cells[i] = new CellTypes[size];
            }
        }
        public ChessField(CellTypes[][] cells)
        {
            Size = cells.Length;
            Cells = cells;
            Tuple<int, int> horsePosition = DeterminePosition(CellTypes.H);
            Tuple<int, int> kingPosition = DeterminePosition(CellTypes.K);
            HorseX = horsePosition.Item1;
            HorseY = horsePosition.Item2;
            KingX = kingPosition.Item1;
            KingY = kingPosition.Item2;
        }

        public static string FindWay(ChessField chessField)
        {
            Dictionary<int, ChessField> wrongFields = new Dictionary<int, ChessField>();

            FindOptimalWay(chessField, wrongFields, out string way);
            return way;
        }
        public static bool FindOptimalWay(ChessField chessField, 
            Dictionary<int, ChessField> wrongFields, out string way)
        {
            way = "";
            //  Путь находится. TODO: нахождение кратчайшего пути, вывод пути 
            List<MoveDirections> moveSequence = new List<MoveDirections>();
            Console.WriteLine(chessField.ToString());
            
            if (chessField.IsResult())
            {
                return true;
            }

            for (int i = 0; i < AllMoves.Length; ++i)
            {
                MoveDirections currentMove = AllMoves[i];
                if (chessField.IsMoveAvailable(currentMove, wrongFields))
                {
                    if (ChessField.FindOptimalWay(chessField.MoveHorse(currentMove, wrongFields), wrongFields, out way))
                    {
                        way = currentMove.ToString() + "\n" + way;
                        return true;
                    }
                    i = 0;
                }
            }

            ChessField.AddFieldToDictionary(wrongFields, chessField);
            return false;
        }
        public ChessField MoveHorse(MoveDirections direction, Dictionary<int, ChessField> wrongFields)
        {
            if (IsMoveAvailable(direction, wrongFields))
            {
                ChangeHorseCoords(direction);
            }
            return (ChessField)this.Clone();
        }
        public bool IsMoveAvailable(MoveDirections direction, Dictionary<int, ChessField> wrongFields)
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

            ChessField cloneField = (ChessField)this.Clone();
            cloneField.ChangeHorseCoords(direction);
            if (wrongFields.ContainsKey(cloneField.GetHashCode()))
            {
                return false;
            }

            return true;
        }
        private void ChangeHorseCoords(MoveDirections direction)
        {
            try
            {
                Cells[HorseY][HorseX] = CellTypes.D;
                //  Изменяем положение коня
                HorseX = HorseX + GetMoveOffsetX(direction);
                HorseY = HorseY + GetMoveOffsetY(direction);
                //  Изменяем состояние новой клетки
                Cells[HorseY][HorseX] = CellTypes.H;
            }
            catch
            {
                throw new Exception($"{direction} is impossible move.");
            }
        }
        public bool IsResult() => (KingX == HorseX && KingY == HorseY);

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

        private Tuple<int, int> DeterminePosition(CellTypes cellType)
        {
            for (int i = 0; i < Size; ++i)
            {
                for (int j = 0; j < Size; ++j)
                {
                    if (Cells[i][j] == cellType)
                    {
                        return Tuple.Create(j, i);
                    }
                }
            }
            throw new Exception($"{cellType} not found");
        }

        private static void AddFieldToDictionary(Dictionary<int, ChessField> wrongFields, ChessField field)
        {
            if (!wrongFields.ContainsKey(field.GetHashCode()))
            {
                wrongFields.Add(field.GetHashCode(), field);
            }
            if (ChessField._previousFields.ContainsKey(field.GetHashCode()))
            {
                ChessField._previousFields.Add(field.GetHashCode(), field);
            }
        }


        public override int GetHashCode()
        {
            long hash = 0, count = 0;
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
            if (!(obj is ChessField))
            {
                return false;
            }
            ChessField anotherField = (ChessField)obj;
            return anotherField.GetHashCode() == this.GetHashCode();
        }
        public object Clone()
        {
            ChessField cloneField = new ChessField
            {
                Size = this.Size,
                HorseX = this.HorseX,
                HorseY = this.HorseY,
                KingX = this.KingX,
                KingY = this.KingY
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
    }
}
