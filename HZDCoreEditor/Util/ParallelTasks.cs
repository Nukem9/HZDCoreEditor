using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HZDCoreEditor.Util
{
    public class ParallelTasks<T>
    {
        private CancellationTokenSource _tokens;
        private Exception _error;

        private Action<T> Action { get; }
        private BlockingCollection<T> Queue { get; }
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

            foreach (var worker in Workers)
                worker.Start();
        }
        
        public void AddItems(IEnumerable<T> items)
        {
            foreach (var item in items)
                Queue.Add(item);
        }
        public void AddItem(T item)
        {
            Queue.Add(item);
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

            if (_error != null)
                throw new Exception("Parallel tasks failed: " + _error.Message, _error);
        }
    }
}
