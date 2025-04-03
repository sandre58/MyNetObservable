// -----------------------------------------------------------------------
// <copyright file="PredicateFilter.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;

namespace MyNet.Observable.Collections.Filters;

public sealed class PredicateFilter<T>(Func<T?, bool> predicate) : IFilter
{
    string IFilter.PropertyName => string.Empty;

    public bool IsMatch(object? target) => predicate.Invoke((T?)target);
}
