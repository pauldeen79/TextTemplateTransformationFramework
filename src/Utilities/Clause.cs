using System;

namespace Utilities
{
    public static class Clause
    {
        public static Clause<T, TResult> Create<T, TResult>(Func<bool> predicate, Func<TResult> function)
            => Create(new Func<T, bool>(_ => predicate()), _ => function());

        public static Clause<T, TResult> Create<T, TResult>(Func<T, bool> predicate, Func<T, TResult> function)
            => new Clause<T, TResult>(predicate, function);

        public static Clause<T, TResult> Always<T, TResult>(Func<T, TResult> function)
            => new Clause<T, TResult>(_ => true, function);

        public static Clause<T, TResult> Always<T, TResult>(Func<TResult> function)
            => new Clause<T, TResult>(_ => true, _ => function());

        public static Clause<T, TResult> When<T, TResult>(Func<T, bool> predicate)
            => new Clause<T, TResult>(predicate, _ => default);

        public static Clause<T, TResult> When<T, TResult>(Func<bool> predicate)
            => new Clause<T, TResult>(new Func<T, bool>(_ => predicate()), _ => default);
    }

    public sealed class Clause<T, TResult>
    {
        public Func<T, bool> Predicate { get; }
        public Func<T, TResult> Function { get; }

        internal Clause(Func<T, bool> predicate, Func<T, TResult> function)
        {
            Predicate = predicate;
            Function = function;
        }

        public Clause<T, TResult> Then(Func<TResult> function) =>
            new Clause<T, TResult>(Predicate, _ => function());

        public Clause<T, TResult> Then(Func<T, TResult> function) =>
            new Clause<T, TResult>(Predicate, function);
    }
}
