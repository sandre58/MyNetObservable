// -----------------------------------------------------------------------
// <copyright file="ValidatePropertyAttribute.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;

namespace MyNet.Observable.Attributes;

/// <summary>
/// Indicates that the specified property must be validate in same time this property.
/// </summary>
/// <remarks>
/// Initialise a new instance of <see cref="ValidatePropertyAttribute"/>.
/// </remarks>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
public sealed class ValidatePropertyAttribute(string propertyName) : Attribute
{
    /// <summary>
    /// Gets property name.
    /// </summary>
    public string PropertyName { get; } = propertyName;
}
