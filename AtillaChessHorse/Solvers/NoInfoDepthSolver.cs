﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtillaChessHorse.Solvers
{
    public class NoInfoDepthSolver : Solver
    {
        public NoInfoDepthSolver()
        {
            OpenStates = new Queue<FieldState>();
        }
        public override List<FieldState> Solve(FieldState initState)
        {
            IEnumerable<FieldState> availableStates = DetermineAvailableFieldStates(initState);
            if (availableStates.Count() == 0)
            {
                throw new Exception("No ways available at start");
            }
            AddToOpenStates(availableStates);

            do
            {
                if ((OpenStates as Queue<FieldState>).Peek().IsResult())
                {
                    return FormResultWay((OpenStates as Queue<FieldState>).Peek());
                }
                AddToOpenStates(DetermineAvailableFieldStates((OpenStates as Queue<FieldState>).Dequeue()));
            } while ((OpenStates as Queue<FieldState>).Peek() != null);
            
            return new List<FieldState>();
        }
        
        protected override void AddToOpenStates(IEnumerable<FieldState> states)
        {
            foreach (FieldState state in states)
            {
                (OpenStates as Queue<FieldState>).Enqueue(state);
            }
        }
    }
}
