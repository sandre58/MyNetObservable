// -----------------------------------------------------------------------
// <copyright file="ExpressionFilter.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Linq.Expressions;
using MyNet.Utilities;

namespace MyNet.Observable.Collections.Filters;

public class ExpressionFilter<T, TProperty>(Expression<Func<T, TProperty>> expression, Func<TProperty?, bool> predicate) : IFilter
{
    public string PropertyName { get; } = expression.GetPropertyName().OrEmpty();

    public bool IsMatch(object? target)
    {
        if (target is not T t) return false;

        var func = expression.Compile();

        return predicate.Invoke(func.Invoke(t));
    }
}
