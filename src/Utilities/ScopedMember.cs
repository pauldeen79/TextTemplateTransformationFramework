using System;
using System.Collections.Generic;
using Utilities.Extensions;

namespace Utilities
{
    public static class ScopedMember
    {
        public static T Run<T>(T member, params Action<T>[] actions) =>
            Run(member, (IEnumerable<Action<T>>)actions);

        public static T Run<T>(T member, IEnumerable<Action<T>> actions)
        {
            actions.ForEach(a => a(member));
            return member;
        }

        public static Func<T> Run<T>(Func<T> memberDelegate, params Action<Func<T>>[] actions) =>
            Run(memberDelegate, (IEnumerable<Action<Func<T>>>)actions);

        public static Func<T> Run<T>(Func<T> memberDelegate, IEnumerable<Action<Func<T>>> actions)
        {
            actions.ForEach(a => a(memberDelegate));
            return memberDelegate;
        }

        public static TResult Evaluate<T, TResult>(T member, Func<T, TResult> function)
        {
            if (function is null)
            {
                throw new ArgumentNullException(nameof(function));
            }

            return function(member);
        }

        public static TResult Evaluate<T, TResult>(Func<T> memberDelegate, Func<Func<T>, TResult> function)
        {
            if (function is null)
            {
                throw new ArgumentNullException(nameof(function));
            }

            return function(memberDelegate);
        }
    }
}
