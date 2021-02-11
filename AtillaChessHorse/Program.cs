using AtillaChessHorse.Solvers;
using AtillaChessHorse.States;
using System;
using static AtillaChessHorse.States.FieldState;

namespace AtillaChessHorse
{
    public class Program
    {
        static CellTypes[][] GetRealField()
        {
            int size = 8;
            CellTypes[][] fieldCells = new CellTypes[size][];

            fieldCells[0] = new CellTypes[] { CellTypes.A, CellTypes.D, CellTypes.D, CellTypes.D, CellTypes.A, CellTypes.A, CellTypes.D, CellTypes.D };
            fieldCells[1] = new CellTypes[] { CellTypes.D, CellTypes.D, CellTypes.D, CellTypes.D, CellTypes.A, CellTypes.A, CellTypes.A, CellTypes.D };
            fieldCells[2] = new CellTypes[] { CellTypes.A, CellTypes.A, CellTypes.A, CellTypes.A, CellTypes.A, CellTypes.A, CellTypes.A, CellTypes.A };
            fieldCells[3] = new CellTypes[] { CellTypes.A, CellTypes.A, CellTypes.D, CellTypes.D, CellTypes.D, CellTypes.D, CellTypes.D, CellTypes.D };
            fieldCells[4] = new CellTypes[] { CellTypes.A, CellTypes.A, CellTypes.D, CellTypes.D, CellTypes.D, CellTypes.D, CellTypes.H, CellTypes.D };
            fieldCells[5] = new CellTypes[] { CellTypes.A, CellTypes.K, CellTypes.D, CellTypes.D, CellTypes.A, CellTypes.D, CellTypes.A, CellTypes.A };
            fieldCells[6] = new CellTypes[] { CellTypes.A, CellTypes.A, CellTypes.D, CellTypes.A, CellTypes.A, CellTypes.A, CellTypes.A, CellTypes.D };
            fieldCells[7] = new CellTypes[] { CellTypes.D, CellTypes.A, CellTypes.D, CellTypes.A, CellTypes.A, CellTypes.D, CellTypes.A, CellTypes.A };
            return fieldCells;
        }

        static void Main(string[] args)
        {
            IState<FieldState> initState = new FieldState(GetRealField(), 6, 4);
            ISearch<FieldState> search = null;
            char choise = '0';

            while (search == null)
            {
                Console.Write("\"No info\" search. Choose the type:\n1 - Depth;\n2 - Width.\n->");
                choise = Console.ReadKey().KeyChar;
                if (choise == '1')
                {
                    search = new NoInfoDepthSearch();
                }
                else if (choise == '2')
                {
                    search = new NoInfoWidthSearch();
                }
                Console.Clear();
            }

            var way = search.Search(initState);
            int count = 1;
            Console.WriteLine($"-------------------------------\n{(choise == '1' ? "Depth" : "Width")}:\n");
            way.ForEach(cell => 
                { 
                    Console.WriteLine($"{count}.____________\n" + cell.ToString()); 
                    count++; 
                });

            Console.ReadKey();
        }
    }
}
