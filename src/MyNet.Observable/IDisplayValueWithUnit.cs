// -----------------------------------------------------------------------
// <copyright file="IDisplayValueWithUnit.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;

namespace MyNet.Observable;

public interface IDisplayValueWithUnit
{
    string? SimplifyToString(Enum? minUnit = null, Enum? maxUnit = null, bool abbreviation = true, string? format = null);

    string? ToString(bool abbreviation, string? format = null);
}
