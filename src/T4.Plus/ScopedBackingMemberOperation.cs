using System;

namespace TextTemplateTransformationFramework.T4.Plus
{
    public static class ScopedBackingMemberOperation
    {
        public static TReturn InvokeWhenNotNull<TRef, TReturn>(ref TRef referenceMember, TRef newMemberValue, Func<TReturn> runDelegate)
            where TReturn : class =>
            Invoke
            (
                ref referenceMember,
                newMemberValue,
                v => !System.Collections.Generic.EqualityComparer<TRef>.Default.Equals(v, default),
                runDelegate
            );

        public static TReturn InvokeAlways<TRef, TReturn>(ref TRef referenceMember, TRef newMemberValue, Func<TReturn> runDelegate)
            where TReturn : class =>
            Invoke
            (
                ref referenceMember,
                newMemberValue,
                _ => true,
                runDelegate
            );

        public static TReturn Invoke<TRef, TReturn>(ref TRef referenceMember, TRef newMemberValue, Func<TRef, bool> copyToBackingMemberCondition, Func<TReturn> runDelegate)
            where TReturn : class
        {
            if (copyToBackingMemberCondition == null)
            {
                throw new ArgumentNullException(nameof(copyToBackingMemberCondition));
            }

            if (runDelegate == null)
            {
                throw new ArgumentNullException(nameof(runDelegate));
            }

            var previousValue = referenceMember;

            var shouldPerformCopyingToBackingMember = copyToBackingMemberCondition(newMemberValue);
            if (shouldPerformCopyingToBackingMember)
            {
                referenceMember = newMemberValue;
            }

            try
            {
                return runDelegate();
            }
            finally
            {
                if (shouldPerformCopyingToBackingMember)
                {
                    referenceMember = previousValue;
                }
            }
        }
    }
}
