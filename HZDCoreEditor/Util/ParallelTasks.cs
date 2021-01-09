using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HZDCoreEditor.Util
{
    public class ParallelTasks<T>
    {
        public Action<T> Action { get; }
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
            foreach (var worker in Workers)
                worker.Start();
        }

        public void AddItem(T item)
        {
            Queue.Add(item);
        }

        private void Worker()
        {
            foreach (var item in Queue.GetConsumingEnumerable())
                Action(item);
        }
        
        public void CompleteAdding()
        {
            Queue.CompleteAdding();
        }

        public void WaitForComplete()
        {
            Task.WaitAll(Workers);
        }
    }
}
