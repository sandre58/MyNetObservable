// -----------------------------------------------------------------------
// <copyright file="HasAnyItemsAttribute.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MyNet.Observable.Resources;

namespace MyNet.Observable.Attributes;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class HasAnyItemsAttribute : ValidationAttribute
{
    public HasAnyItemsAttribute()
    {
        ErrorMessageResourceName = nameof(ValidationResources.FieldXMustBeContainOneItemAtLeastError);
        ErrorMessageResourceType = typeof(ValidationResources);
    }

    public override bool IsValid(object? value) => value is IEnumerable<object> collection && collection.Any();
}
