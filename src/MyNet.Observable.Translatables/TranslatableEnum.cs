// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;
using MyNet.Humanizer;
using MyNet.Observable.Attributes;

namespace MyNet.Observable.Translatables
{
    public class TranslatableEnum : TranslatableEnum<Enum>
    {
        public TranslatableEnum(Enum enumValue) : base(enumValue) { }
    }

    public class TranslatableEnum<TEnum> : TranslatableObject<TEnum>
        where TEnum : Enum
    {
        [UpdateOnCultureChanged]
        public string Description => Value?.ToDescription() ?? string.Empty;

        [UpdateOnCultureChanged]
        public string Display => Value?.Humanize() ?? string.Empty;

        public TranslatableEnum(TEnum enumValue) : base(() => enumValue) { }

        public override string ToString() => Display;

        public override bool Equals(object? obj) => obj is TranslatableEnum result && (result.Value?.Equals(Value) ?? false);

        public override int GetHashCode() => Value?.GetHashCode() ?? 0;
    }
}
