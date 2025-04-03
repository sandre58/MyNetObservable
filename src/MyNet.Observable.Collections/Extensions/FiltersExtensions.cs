// -----------------------------------------------------------------------
// <copyright file="FiltersExtensions.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using MyNet.Observable.Collections.Filters;
using MyNet.Utilities.Comparison;

namespace MyNet.Observable.Collections.Extensions;

public static class FiltersExtensions
{
    public static bool Match<T>(this IList<CompositeFilter> filters, T item)
    {
        if (!filters.Any()) return true;

        var result = filters[0].Filter.IsMatch(item);

        if (filters.Count <= 1) return result;

        for (var i = 1; i < filters.Count; i++)
        {
            var wrapper = filters[i];

            result = wrapper.Operator switch
            {
                LogicalOperator.Or => result || wrapper.Filter.IsMatch(item),
                LogicalOperator.And => result && wrapper.Filter.IsMatch(item),
                _ => throw new NotImplementedException()
            };
        }

        return result;
    }
}
