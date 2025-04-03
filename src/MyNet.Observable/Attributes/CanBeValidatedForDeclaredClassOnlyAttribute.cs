// -----------------------------------------------------------------------
// <copyright file="CanBeValidatedForDeclaredClassOnlyAttribute.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;

namespace MyNet.Observable.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, Inherited = false)]
public sealed class CanBeValidatedForDeclaredClassOnlyAttribute(bool value = true) : Attribute
{
    public bool Value { get; } = value;
}
