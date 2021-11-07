using System;
using System.Diagnostics;

namespace Domain
{
    public class DelegateTraceListener : TraceListener
    {
        private readonly Action<string> _write;

        public DelegateTraceListener(Action<string> write)
            => _write = write;

        public override void Write(string message)
            => _write(message);

        public override void WriteLine(string message)
            => Write(message + Environment.NewLine);
    }
}
