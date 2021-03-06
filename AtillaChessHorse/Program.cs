﻿using AtillaChessHorse.Searches;
using AtillaChessHorse.States;
using System;
using static AtillaChessHorse.States.FieldState;
using static AtillaChessHorse.States.FieldState.CellTypes;

namespace AtillaChessHorse
{
    public class Program
    {
        static CellTypes[][] GetFirstField()
        {
            int size = 8;
            CellTypes[][] fieldCells = new CellTypes[size][];

            fieldCells[0] = new CellTypes[] { A, D, D, D, A, A, D, D };
            fieldCells[1] = new CellTypes[] { D, D, D, D, A, A, A, D };
            fieldCells[2] = new CellTypes[] { A, A, A, A, A, A, A, A };
            fieldCells[3] = new CellTypes[] { A, A, D, D, D, D, D, D };
            fieldCells[4] = new CellTypes[] { A, A, D, D, D, D, H, D };
            fieldCells[5] = new CellTypes[] { A, K, D, D, A, D, A, A };
            fieldCells[6] = new CellTypes[] { A, A, D, A, A, A, A, D };
            fieldCells[7] = new CellTypes[] { D, A, D, A, A, D, A, A };
            return fieldCells;
        }

        static CellTypes[][] GetSecondField()
        {
            int size = 8;
            CellTypes[][] fieldCells = new CellTypes[size][];

            fieldCells[0] = new CellTypes[] { A, D, D, D, A, A, D, D };
            fieldCells[1] = new CellTypes[] { D, D, D, D, A, A, A, D };
            fieldCells[2] = new CellTypes[] { A, A, A, A, A, A, A, A };
            fieldCells[3] = new CellTypes[] { A, A, D, D, D, D, D, D };
            fieldCells[4] = new CellTypes[] { A, A, D, D, D, D, A, D };
            fieldCells[5] = new CellTypes[] { A, K, D, D, A, D, A, A };
            fieldCells[6] = new CellTypes[] { A, A, D, A, A, A, A, H };
            fieldCells[7] = new CellTypes[] { D, A, D, A, A, A, A, A };
            return fieldCells;
        }

        static void Main(string[] args)
        {
            IState initState = new FieldState(GetFirstField(), 6, 4);
            ISearch search = null;
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

            try
            {
                var start = DateTime.Now;
                var way = search.Search(initState, out int stepCount, out int maxStatesCount);
                int count = 1;
                Console.WriteLine($"-------------------------------\n{(choise == '1' ? "Depth" : "Width")}:\n");
                way.ForEach(cell =>
                {
                    Console.WriteLine($"{count}.____________\n" + cell.ToString());
                    count++;
                });
                var totalDuration = (DateTime.Now - start).TotalMilliseconds;
                Console.WriteLine($"Total steps:{stepCount}");
                Console.WriteLine($"Total time:{totalDuration}");
                Console.WriteLine($"Memory:{maxStatesCount}");
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }            
            Console.ReadKey();
        }
    }
}
