// -----------------------------------------------------------------------
// <copyright file="UpdateOnTimeZoneChangedAttribute.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;

namespace MyNet.Observable.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public sealed class UpdateOnTimeZoneChangedAttribute : Attribute;
