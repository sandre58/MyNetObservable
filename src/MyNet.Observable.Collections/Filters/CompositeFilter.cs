// -----------------------------------------------------------------------
// <copyright file="CompositeFilter.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using MyNet.Utilities.Comparison;

namespace MyNet.Observable.Collections.Filters;

public class CompositeFilter(IFilter filter, LogicalOperator @operator = LogicalOperator.And)
{
    public LogicalOperator Operator { get; } = @operator;

    public IFilter Filter { get; } = filter;
}
