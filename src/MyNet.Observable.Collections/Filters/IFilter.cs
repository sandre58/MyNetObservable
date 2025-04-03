// -----------------------------------------------------------------------
// <copyright file="IFilter.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace MyNet.Observable.Collections.Filters;

public interface IFilter
{
    string PropertyName { get; }

    bool IsMatch(object? target);
}
