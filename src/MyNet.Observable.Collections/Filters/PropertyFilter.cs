// -----------------------------------------------------------------------
// <copyright file="PropertyFilter.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MyNet.Observable.Collections.Filters;

public class PropertyFilter(string propertyName, Func<object?, bool> predicate) : IFilter
{
    private static readonly string[] Separator = ["."];

    public string PropertyName { get; } = propertyName;

    public bool IsMatch(object? target) => IsMatchInternal(target, PropertyName.Split(Separator, StringSplitOptions.RemoveEmptyEntries));

    private bool IsMatchInternal(object? target, IList<string> propertyNames)
    {
        if (target == null) return false;

        var toCompare = target;

        if (!propertyNames.Any())
            return toCompare is IList enumerable ? IsMatchPropertyList(enumerable.Cast<object>()) : IsMatchProperty(toCompare);

        var newPropertyNames = propertyNames.ToList();

        foreach (var propertyName in propertyNames)
        {
            var propertyInfo = toCompare?.GetType().GetProperty(propertyName);
            if (propertyInfo == null)
                return false;

            toCompare = propertyInfo.GetValue(toCompare, null);

            _ = newPropertyNames.Remove(propertyName);

            if (newPropertyNames.Count > 0 && toCompare is IList toCompareEnumerableRecursive)
                return toCompareEnumerableRecursive.Cast<object>().Any(x => IsMatchInternal(x, newPropertyNames));
        }

        return toCompare is IList toCompareEnumerable ? IsMatchPropertyList(toCompareEnumerable.Cast<object>()) : IsMatchProperty(toCompare!);
    }

    private bool IsMatchPropertyList(IEnumerable<object> toCompareEnumerable) => toCompareEnumerable.Any(IsMatchProperty);

    private bool IsMatchProperty(object? toCompareProperty) => predicate.Invoke(toCompareProperty);

    public override bool Equals(object? obj) => obj is PropertyFilter o && GetType() == obj.GetType() && PropertyName == o.PropertyName;

    public override int GetHashCode() => PropertyName.GetHashCode(StringComparison.OrdinalIgnoreCase);
}
