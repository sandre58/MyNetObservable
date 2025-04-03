// -----------------------------------------------------------------------
// <copyright file="IsEmailAddressAttribute.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.ComponentModel.DataAnnotations;
using MyNet.Observable.Resources;
using MyNet.Utilities;

namespace MyNet.Observable.Attributes;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class IsEmailAddressAttribute : DataTypeAttribute
{
    public IsEmailAddressAttribute(bool allowEmptyValue = false)
        : base(DataType.EmailAddress)
    {
        AllowEmptyValue = allowEmptyValue;

        ErrorMessageResourceName = nameof(ValidationResources.FieldXMustBeValidEmailAddressError);
        ErrorMessageResourceType = typeof(ValidationResources);
    }

    public bool AllowEmptyValue { get; }

    public override bool IsValid(object? value)
        => value == null || (AllowEmptyValue && string.IsNullOrEmpty(value.ToString())) || (value.ToString()?.IsEmailAddress() ?? false);
}
