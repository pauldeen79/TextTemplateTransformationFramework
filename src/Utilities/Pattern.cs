using System;
using System.Collections.Generic;
using System.Linq;

namespace Utilities
{
    public static class Pattern
    {
        public static Pattern<T, TResult> Match<T, TResult>(Func<T, TResult> defaultValueDelegate, IEnumerable<Clause<T, TResult>> clauses) =>
        new Pattern<T, TResult>(defaultValueDelegate, clauses);

        public static Pattern<T, TResult> Match<T, TResult>(Func<T, TResult> defaultValueDelegate, params Clause<T, TResult>[] clauses) =>
            new Pattern<T, TResult>(defaultValueDelegate, clauses);

        public static Pattern<T, TResult> Match<T, TResult>(params Clause<T, TResult>[] clauses) =>
            new Pattern<T, TResult>(_ => default, clauses);
    }

    public sealed class Pattern<T, TResult>
    {
        private readonly Func<T, TResult> _defaultValueClause;
        private readonly IEnumerable<Clause<T, TResult>> _clauses;

        internal Pattern(Func<T, TResult> defaultValueDelegate, IEnumerable<Clause<T, TResult>> clauses)
        {
            _defaultValueClause = defaultValueDelegate;
            _clauses = clauses;
        }

        public TResult Evaluate() =>
            Evaluate(default);

        public TResult Evaluate(T value)
        {
            var clause = _clauses.FirstOrDefault(c => c.Predicate(value));

            return clause == null
                ? _defaultValueClause(value)
                : clause.Function(value);
        }

        public IEnumerable<TResult> EvaluateAll(T value)
        {
            foreach (var clause in _clauses)
            {
                if (clause.Predicate(value))
                {
                    yield return clause.Function(value);
                }
            }
        }

        public Pattern<T, TResult> Default(Func<T, TResult> defaultValueDelegate) =>
            new Pattern<T, TResult>(defaultValueDelegate, _clauses);

        public Pattern<T, TResult> Default(Func<TResult> defaultValueDelegate) =>
            new Pattern<T, TResult>(_ => defaultValueDelegate(), _clauses);
    }
}
