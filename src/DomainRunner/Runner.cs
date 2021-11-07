using System;

namespace Domain
{
    /// <summary>
    /// Helper class for running code in a separate AppDomain.
    /// </summary>
    public static class Runner
    {
        /// <summary>
        /// Runs code in a specified AppDomain.
        /// </summary>
        /// <param name="delg">Code to execute (delegate)</param>
        /// <param name="targetDomain">AppDomain to run the code in</param>
        /// <param name="args">Arguments to use on the delegate</param>
        /// <returns>Return value of the code (delegate)</returns>
        public static object RunInAppDomain(Delegate delg, AppDomain targetDomain, params object[] args)
        {
            if (targetDomain == null)
            {
                throw new ArgumentNullException(nameof(targetDomain));
            }

            if (delg == null)
            {
                throw new ArgumentNullException(nameof(delg));
            }

            var runner = new DomainDomainRunner(delg, args, delg.GetHashCode());
            targetDomain.DoCallBack(new CrossAppDomainDelegate(runner.Invoke));

            return targetDomain.GetData("appDomainResult" + delg.GetHashCode());
        }

        /// <summary>
        /// Runs code in a new AppDomain. The AppDomain will be created and destroyed on the fly.
        /// </summary>
        /// <param name="delg">Code to execute (delegate)</param>
        /// <param name="args">Arguments to use on the delegate</param>
        /// <returns>Return value of the code (delegate)</returns>
        public static object RunInAppDomain(Delegate delg, params object[] args)
        {
            if (delg == null)
            {
                throw new ArgumentNullException(nameof(delg));
            }

            var setup = new AppDomainSetup
            {
                ApplicationBase = AppDomain.CurrentDomain.SetupInformation.ApplicationBase
            };
            var tempDomain = AppDomain.CreateDomain("domain_RunInAppDomain" + delg.GetHashCode(), AppDomain.CurrentDomain.Evidence, setup);
            CrossDomainTraceHelper.StartListening(tempDomain);
            object returnValue = null;
            try
            {
                returnValue = RunInAppDomain(delg, tempDomain, args);
            }
            finally
            {
                AppDomain.Unload(tempDomain);
            }
            return returnValue;
        }

        [Serializable]
        internal class DomainDomainRunner
        {
            private readonly Delegate _delegate;
            private readonly object[] _arguments;
            public int _hash;

            public DomainDomainRunner(Delegate delg, object[] args, int hash)
            {
                _delegate = delg;
                _arguments = args;
                _hash = hash;
            }

            public void Invoke()
            {
                if (_delegate != null)
                {
                    AppDomain.CurrentDomain.SetData("appDomainResult" + _hash, _delegate.DynamicInvoke(_arguments));
                }
            }
        }
    }
}
