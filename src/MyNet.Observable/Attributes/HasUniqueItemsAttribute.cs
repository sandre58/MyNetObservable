// -----------------------------------------------------------------------
// <copyright file="HasUniqueItemsAttribute.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using MyNet.Observable.Resources;
using MyNet.Utilities;

namespace MyNet.Observable.Attributes;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class HasUniqueItemsAttribute : ValidationAttribute
{
    public HasUniqueItemsAttribute()
    {
        ErrorMessageResourceName = nameof(ValidationResources.FieldXMustHaveUniqueItemsError);
        ErrorMessageResourceType = typeof(ValidationResources);
    }

    public override bool IsValid(object? value)
    {
        if (value is not IEnumerable<object> collection) return true;

        var list = collection.ToList();
        var hashSet = new HashSet<object>(list, new SimilarComparer());
        return hashSet.Count == list.Count;
    }

    private sealed class SimilarComparer : IEqualityComparer<object>
    {
        bool IEqualityComparer<object>.Equals(object? x, object? y) => x is ISimilar a ? a.IsSimilar(y) : x?.Equals(y) ?? false;

        public int GetHashCode(object obj) => RuntimeHelpers.GetHashCode(this);
    }
}
