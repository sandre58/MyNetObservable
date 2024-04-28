// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq.Expressions;
using MyNet.Utilities;

namespace MyNet.Observable.Collections.Filters
{
    public class ExpressionFilter<T, TProperty> : IFilter
    {
        private readonly Expression<Func<T, TProperty>> _expression;
        private readonly Func<TProperty?, bool> _predicate;

        public ExpressionFilter(Expression<Func<T, TProperty>> expression, Func<TProperty?, bool> predicate)
        {
            _expression = expression;
            _predicate = predicate;
            PropertyName = expression.GetPropertyName().OrEmpty();
        }

        public string PropertyName { get; }

        public bool IsMatch(object? target)
        {
            if (target is not T t) return false;

            var func = _expression.Compile();

            return _predicate.Invoke(func.Invoke(t));
        }
    }
}
