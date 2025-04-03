﻿// -----------------------------------------------------------------------
// <copyright file="CanBeValidatedAttribute.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;

namespace MyNet.Observable.Attributes;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Class | AttributeTargets.Interface)]
public sealed class CanBeValidatedAttribute(bool value = true) : Attribute
{
    public bool Value { get; } = value;
}
