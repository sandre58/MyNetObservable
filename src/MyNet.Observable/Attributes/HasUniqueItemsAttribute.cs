// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using MyNet.Observable.Resources;
using MyNet.Utilities;

namespace MyNet.Observable.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
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

            var hashSet = new HashSet<object>(collection, new SimilarComparer());
            return hashSet.Count == collection.Count();
        }


        private sealed class SimilarComparer : IEqualityComparer<object>
        {
            public new bool Equals(object? x, object? y) => x is ISimilar a ? a.IsSimilar(y) : x?.Equals(y) ?? false;

            public int GetHashCode(object obj) => RuntimeHelpers.GetHashCode(this);
        }
    }
}
