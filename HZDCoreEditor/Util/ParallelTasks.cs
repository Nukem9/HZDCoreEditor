using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace HZDCoreEditor.Util
{
    public class ParallelTasks<T>
    {
        private CancellationTokenSource _tokens;
        private Exception _error;
        private bool _stopping;

        private Action<T> Action { get; }
        private BlockingCollection<T> Queue { get; set; }
        private Task[] Workers { get; }
        

        public ParallelTasks(int threads, Action<T> action)
        {
            Action = action;
            Queue = new BlockingCollection<T>();

            Workers = new Task[threads];
            for (int i = 0; i < threads; i++)
                Workers[i] = new Task(Worker);
        }

        public void Start()
        {
            _tokens = new CancellationTokenSource();
            _error = null;
            _stopping = false;

            foreach (var worker in Workers)
                worker.Start();
        }
        
        public bool AddItems(IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                if (!AddItem(item))
                    return false;
            }
            return true;
        }
        public bool AddItem(T item)
        {
            if (_stopping) return false;
            Queue.Add(item);
            return true;
        }

        private void Worker()
        {
            try
            {
                while (Queue.TryTake(out var item, -1, _tokens.Token))
                    Action(item);
            }
            catch (Exception ex)
            {
                _error = ex;
                Stop();
            }
        }

        public void Stop()
        {
            _stopping = true;
            _tokens.Cancel();
        }

        public void CompleteAdding()
        {
            Queue.CompleteAdding();
        }

        public void WaitForComplete()
        {
            if (!Queue.IsAddingCompleted)
                Queue.CompleteAdding();

            Task.WaitAll(Workers);

            if (_error != null && !(_error is OperationCanceledException))
                throw new Exception("Parallel tasks failed: " + _error.Message, _error);
        }
    }
}
