using System;
using System.Diagnostics;

namespace Domain
{
    public class CrossDomainTraceHelper : MarshalByRefObject
    {
        private CrossDomainTraceHelper _parentDomain;

        public CrossDomainTraceHelper()
        {
        }

        public static void StartListening(AppDomain domain)
        {
            if (domain == null)
            {
                throw new ArgumentNullException(nameof(domain));
            }

            var listenerType = typeof(CrossDomainTraceHelper);

            // Create a remote instance
            var remoteHelper =
                (CrossDomainTraceHelper)domain.CreateInstanceAndUnwrap(
                    listenerType.Assembly.FullName,
                    listenerType.FullName);

            // Create a local instance
            var localHelper = new CrossDomainTraceHelper();

            // Register the local helper in the remote domain
            remoteHelper.Register(localHelper);
        }

        private void Register(CrossDomainTraceHelper parentDomain)
        {
            // Store the parent domain to pass messages to later
            _parentDomain = parentDomain;

            // Create and register the delegate trace listener
            var listener = new DelegateTraceListener(Write);

            Debug.Listeners.Add(listener);
        }

        private void Write(string message)
        {
            // Send the message to the parent domain
            _parentDomain.RemoteWrite(message);
        }

        private void RemoteWrite(string message)
        {
            Debug.Write(message);
        }
    }
}
