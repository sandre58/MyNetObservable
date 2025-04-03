// -----------------------------------------------------------------------
// <copyright file="IsInPastAttribute.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.ComponentModel.DataAnnotations;
using MyNet.Observable.Resources;
using MyNet.Utilities;

namespace MyNet.Observable.Attributes;

/// <summary>
/// Indicates that the specified property must be validate in same time this property.
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class IsInPastAttribute : DataTypeAttribute
{
    public IsInPastAttribute(bool allowEmptyValue = false)
        : base(DataType.DateTime)
    {
        AllowEmptyValue = allowEmptyValue;

        ErrorMessageResourceName = nameof(ValidationResources.FieldXMustBeInPastError);
        ErrorMessageResourceType = typeof(ValidationResources);
    }

    public bool AllowEmptyValue { get; }

    public override bool IsValid(object? value)
        => (AllowEmptyValue && (value == null || (value is DateTime d && d == DateTime.MinValue))) || (value is DateTime date && date.IsInPast());
}
