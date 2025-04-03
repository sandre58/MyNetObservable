// -----------------------------------------------------------------------
// <copyright file="EnumClassTranslatable.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using MyNet.Humanizer;
using MyNet.Observable.Attributes;
using MyNet.Utilities;

namespace MyNet.Observable.Translatables;

public class EnumClassTranslatable(IEnumeration enumValue) : EnumClassTranslatable<IEnumeration>(enumValue);

public class EnumClassTranslatable<TEnum>(TEnum enumValue) : Translatable<TEnum>(() => enumValue)
    where TEnum : IEnumeration
{
    [UpdateOnCultureChanged]
    public string Display => Value?.Humanize() ?? string.Empty;

    public override string ToString() => Display;

    public override bool Equals(object? obj) => obj is EnumClassTranslatable<TEnum> result && (result.Value?.Equals(Value) ?? false);

    public override int GetHashCode() => Value?.GetHashCode() ?? 0;
}
