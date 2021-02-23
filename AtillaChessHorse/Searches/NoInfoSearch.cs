using AtillaChessHorse.States;
using System;
using System.Collections.Generic;
using System.Linq;
using static AtillaChessHorse.States.FieldState;

namespace AtillaChessHorse.Searches
{
    public abstract class NoInfoSearch : ISearch
    {
        public int OpenStatesMaxSize { get; private set; } = 7;
        protected IEnumerable<IState> OpenStates { get; set; }
        private readonly Dictionary<int, IState> ClosedStates = new Dictionary<int, IState>();
        private readonly MoveDirections[] AllMoves = new MoveDirections[]
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

        public List<IState> Search(IState initState)
        {
            List<IState> availableStates = DetermineAvailableStates(initState);
            if (availableStates.Count == 0)
            {
                throw new Exception("No states available at start");
            }
            AddToOpen(availableStates);

            IState currentState;
            do
            {
                //  Проверка достижения цели
                if ((currentState = DeleteFromOpen()).IsResult())
                {
                    return FormResultStateSequence(currentState);
                }
                AddToClosed(currentState);
                if (currentState.Child != null)
                {
                    AddToOpen(currentState.Child);
                    currentState.Child = null;
                }
                //  Определение следующих состояний
                var newStates = DetermineAvailableStates(currentState);
                //  Проверка на переполнение списка открытых состояний
                while (!IsAvailabaleToAddToOpen(newStates))
                {
                    CorrectOpen();
                }
                AddToOpen(newStates);

            } while (currentState != null);
            throw new Exception("Way not found");
        }
        protected abstract IState DeleteFromOpen();
        protected abstract IState DeleteWorstStateFromOpen();
        /// <summary> 
        ///  Удаляем состояние c наихудшей эвристикой из списка открытых состояний и заносим его в 
        ///  новое худшее состояние, чтобы при необходимости его можно было восстановить
        /// </summary>
        private void CorrectOpen()
        {
            var deletedNode = DeleteWorstStateFromOpen();
            var secondWorstNode = DeleteWorstStateFromOpen();
            secondWorstNode.Child = deletedNode;
            AddToOpen(secondWorstNode);
        }
        protected abstract void AddToOpen(IState state);
        protected abstract IEnumerable<IState> OrderByHeuristic(IEnumerable<IState> states);
        private bool IsAvailabaleToAddToOpen(IEnumerable<IState> newStates) =>
            OpenStates.Count() + newStates.Count() <= OpenStatesMaxSize;
        private void AddToOpen(IEnumerable<IState> states)
        {
            foreach(var state in states)
            {
                AddToOpen(state);
            }
        }
        private void AddToClosed(IState state)
        {
            int stateHash = state.GetHashCode();
            if (!ClosedStates.ContainsKey(stateHash))
            {
                ClosedStates.Add(stateHash, state);
            }
        }

        private List<IState> DetermineAvailableStates(IState state)
        {
            List<IState> availableStates = new List<IState>();
            foreach (MoveDirections move in AllMoves)
            {
                if (state.IsChangeStateAvailable(move, ClosedStates))
                {
                    availableStates.Add(state.ChangeState(move, ClosedStates));
                }
            }
            return availableStates;
        }
        private List<IState> FormResultStateSequence(IState state)
        {
            IState currentState = (IState)state.Clone();
            List<IState> way = new List<IState>() { state };

            while (currentState.Parent != null)
            {
                way.Add(currentState.Parent);
                currentState = currentState.Parent;
            }
            way.Reverse();
            return way;
        }
    }
}
