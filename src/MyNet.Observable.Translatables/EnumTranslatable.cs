// -----------------------------------------------------------------------
// <copyright file="EnumTranslatable.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using MyNet.Humanizer;
using MyNet.Observable.Attributes;

namespace MyNet.Observable.Translatables;

public class EnumTranslatable(Enum enumValue) : EnumTranslatable<Enum>(enumValue);

public class EnumTranslatable<TEnum>(TEnum enumValue) : Translatable<TEnum>(() => enumValue)
    where TEnum : Enum
{
    [UpdateOnCultureChanged]
    public string Description => Value?.ToDescription() ?? string.Empty;

    [UpdateOnCultureChanged]
    public string Display => Value?.Humanize() ?? string.Empty;

    public override string ToString() => Display;

    public override bool Equals(object? obj) => obj is EnumTranslatable result && (result.Value?.Equals(Value) ?? false);

    public override int GetHashCode() => Value?.GetHashCode() ?? 0;
}
