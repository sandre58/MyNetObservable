// -----------------------------------------------------------------------
// <copyright file="IsFilePathAttribute.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using MyNet.Observable.Resources;

namespace MyNet.Observable.Attributes;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class IsFilePathAttribute : ValidationAttribute
{
    public IsFilePathAttribute(bool allowEmpty = true)
    {
        AllowEmpty = allowEmpty;
        ErrorMessageResourceName = nameof(ValidationResources.FieldXMustBeAnValidFilePathError);
        ErrorMessageResourceType = typeof(ValidationResources);
    }

    public bool AllowEmpty { get; }

    public override bool IsValid(object? value) => (AllowEmpty && string.IsNullOrEmpty(value?.ToString())) || (!string.IsNullOrEmpty(value?.ToString()) && value is string filepath && Path.IsPathRooted(filepath));
}
