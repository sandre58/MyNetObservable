// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MyNet.Observable.Resources;

namespace MyNet.Observable.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class HasAnyItemsAttribute : ValidationAttribute
    {
        public HasAnyItemsAttribute()
        {
            ErrorMessageResourceName = nameof(ValidationResources.FieldXMustBeContainOneItemAtLeastError);
            ErrorMessageResourceType = typeof(ValidationResources);
        }

        public override bool IsValid(object? value) => value is IEnumerable<object> collection && collection.Any();
    }
}
