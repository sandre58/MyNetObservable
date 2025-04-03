// -----------------------------------------------------------------------
// <copyright file="CanSetIsModifiedAttribute.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;

namespace MyNet.Observable.Attributes;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Class | AttributeTargets.Interface)]
public sealed class CanSetIsModifiedAttribute(bool value = true) : Attribute
{
    public bool Value { get; } = value;
}
