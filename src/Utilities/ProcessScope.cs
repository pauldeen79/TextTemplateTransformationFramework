using System;

namespace Utilities
{
    public class ProcessScope
    {
        public bool Busy { get; private set; }

        public void Start()
        {
            if (Busy)
            {
                throw new InvalidOperationException("Already started");
            }

            Busy = true;
        }

        public void Stop()
        {
            if (!Busy)
            {
                throw new InvalidOperationException("Not started");
            }

            Busy = false;
        }

        public void Process(Action action)
        {
            if (action is null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            Start();

            try
            {
                action();
            }
            finally
            {
                Stop();
            }
        }

        public T Process<T>(Func<T> func)
        {
            if (func is null)
            {
                throw new ArgumentNullException(nameof(func));
            }

            Start();

            try
            {
                return func();
            }
            finally
            {
                Stop();
            }
        }
     }
}
