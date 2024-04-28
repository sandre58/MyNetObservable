// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using MyNet.Humanizer;
using MyNet.Observable.Attributes;
using MyNet.Utilities;

namespace MyNet.Observable.Translatables
{
    public class TranslatableEnumeration : TranslatableEnumeration<IEnumeration>
    {
        public TranslatableEnumeration(IEnumeration enumValue) : base(enumValue) { }
    }

    public class TranslatableEnumeration<TEnum> : TranslatableObject<TEnum>
        where TEnum : IEnumeration
    {
        [UpdateOnCultureChanged]
        public string Display => Value?.Humanize() ?? string.Empty;

        public TranslatableEnumeration(TEnum enumValue) : base(() => enumValue) { }

        public override string ToString() => Display;

        public override bool Equals(object? obj) => obj is TranslatableEnumeration<TEnum> result && (result.Value?.Equals(Value) ?? false);

        public override int GetHashCode() => Value?.GetHashCode() ?? 0;
    }
}
