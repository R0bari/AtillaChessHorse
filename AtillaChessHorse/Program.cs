using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AtillaChessHorse.ChessField;

namespace AtillaChessHorse
{
    public class Program
    {
        static CellTypes[][] GetRealField()
        {
            int size = 8;
            CellTypes[][] fieldCells = new CellTypes[size][];

            fieldCells[0] = new CellTypes[] { CellTypes.A, CellTypes.D, CellTypes.D, CellTypes.D,
                                CellTypes.A, CellTypes.A, CellTypes.D, CellTypes.D };
            fieldCells[1] = new CellTypes[] { CellTypes.D, CellTypes.D, CellTypes.D, CellTypes.D,
                                CellTypes.A, CellTypes.A, CellTypes.A, CellTypes.D };
            fieldCells[2] = new CellTypes[] { CellTypes.A, CellTypes.A, CellTypes.A, CellTypes.A,
                                CellTypes.A, CellTypes.A, CellTypes.A, CellTypes.A };
            fieldCells[3] = new CellTypes[] { CellTypes.A, CellTypes.A, CellTypes.D, CellTypes.D,
                                CellTypes.D, CellTypes.D, CellTypes.D, CellTypes.D };
            fieldCells[4] = new CellTypes[] { CellTypes.A, CellTypes.A, CellTypes.D, CellTypes.D,
                                CellTypes.D, CellTypes.D, CellTypes.H, CellTypes.D };
            fieldCells[5] = new CellTypes[] { CellTypes.A, CellTypes.K, CellTypes.D, CellTypes.D,
                                CellTypes.A, CellTypes.D, CellTypes.A, CellTypes.A };
            fieldCells[6] = new CellTypes[] { CellTypes.A, CellTypes.A, CellTypes.D, CellTypes.A,
                                CellTypes.A, CellTypes.A, CellTypes.A, CellTypes.D };
            fieldCells[7] = new CellTypes[] { CellTypes.D, CellTypes.A, CellTypes.D, CellTypes.A,
                                CellTypes.A, CellTypes.D, CellTypes.A, CellTypes.A };
            return fieldCells;
        }

        static void Main(string[] args)
        {
            ChessField field = new ChessField(GetRealField());

            string way = ChessField.FindWay(field);

            Console.WriteLine(way);

            Console.ReadKey();
        }
    }
}
