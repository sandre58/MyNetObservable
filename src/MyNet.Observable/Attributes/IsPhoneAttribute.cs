// -----------------------------------------------------------------------
// <copyright file="IsPhoneAttribute.cs" company="Stéphane ANDRE">
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
public sealed class IsPhoneAttribute : DataTypeAttribute
{
    public IsPhoneAttribute(bool allowEmptyValue = false)
        : base(DataType.PhoneNumber)
    {
        AllowEmptyValue = allowEmptyValue;

        ErrorMessageResourceName = nameof(ValidationResources.FieldXMustBeValidPhoneNumberError);
        ErrorMessageResourceType = typeof(ValidationResources);
    }

    public bool AllowEmptyValue { get; }

    public override bool IsValid(object? value)
        => value == null || (AllowEmptyValue && string.IsNullOrEmpty(value.ToString())) || (value.ToString()?.IsPhoneNumber() ?? false);
}
