// -----------------------------------------------------------------------
// <copyright file="IsRequiredAttribute.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.ComponentModel.DataAnnotations;
using MyNet.Observable.Resources;

namespace MyNet.Observable.Attributes;

/// <summary>
/// Indicates that the specified property must be validate in same time this property.
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public sealed class IsRequiredAttribute : RequiredAttribute
{
    public IsRequiredAttribute()
    {
        ErrorMessageResourceName = nameof(ValidationResources.FieldXIsRequiredError);
        ErrorMessageResourceType = typeof(ValidationResources);
    }

    public override bool IsValid(object? value) => value switch
    {
        TimeSpan ts when ts == TimeSpan.MinValue || ts == TimeSpan.MaxValue => false,
        DateTime dt when dt == DateTime.MinValue || dt == DateTime.MaxValue => false,
        _ => base.IsValid(value)
    };
}
