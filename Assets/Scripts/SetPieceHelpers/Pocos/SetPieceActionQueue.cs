using System;
using System.Collections.Generic;
using Utilities;

namespace SetPieceHelpers.Pocos
{
    public class SetPieceActionQueue : PocoSingleton<SetPieceActionQueue>
    {
        private Queue<Action> _actionQueue;

        public SetPieceActionQueue()
        {
            _actionQueue = new();
        }

        public void Enqueue(Action action)
        {
            _actionQueue.Enqueue(action);
        }

        public void ExecuteNextAction()
        {
            _actionQueue.Dequeue()();
        }
    }
}
