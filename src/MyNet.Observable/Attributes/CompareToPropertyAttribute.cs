// -----------------------------------------------------------------------
// <copyright file="CompareToPropertyAttribute.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.ComponentModel.DataAnnotations;
using MyNet.Utilities;
using MyNet.Utilities.Comparison;

namespace MyNet.Observable.Attributes;

/// <summary>
/// Initialise a new instance of <see cref="ValidatePropertyAttribute"/>.
/// </summary>
[AttributeUsage(AttributeTargets.Property |
                AttributeTargets.Field)]
public sealed class CompareToPropertyAttribute(string propertyName, ComparableOperator @operator) : ValidationAttribute
{
    /// <summary>
    /// Gets property name.
    /// </summary>
    public string PropertyName { get; } = propertyName;

    /// <summary>
    /// Gets property name.
    /// </summary>
    public ComparableOperator Operator { get; } = @operator;

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var property = validationContext.ObjectType.GetProperty(PropertyName);

        return property == null
            ? null
            : property.GetValue(validationContext.ObjectInstance) is not IComparable otherValue || value is not IComparable firstValue
            ? null
            : !firstValue.Compare(otherValue, Operator)
            ? new ValidationResult(FormatErrorMessage(ErrorMessage ?? string.Empty))
            : null;
    }
}
