// -----------------------------------------------------------------------
// <copyright file="HasMaxLengthAttribute.cs" company="Stéphane ANDRE">
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
public sealed class HasMaxLengthAttribute : MaxLengthAttribute
{
    public HasMaxLengthAttribute(int length)
        : base(length)
    {
        ErrorMessageResourceName = nameof(ValidationResources.FieldXMustHaveMaxLengthYError);
        ErrorMessageResourceType = typeof(ValidationResources);
    }
}
