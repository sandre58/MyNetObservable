// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using MyNet.Humanizer;
using MyNet.Utilities.Extensions;

namespace MyNet.Observable.Translatables
{
    public class TranslatableString : TranslatableObject<string>
    {
        public string Key { get; }

        public TranslatableString(string key, LetterCasing casing = LetterCasing.Normal, string? filename = "")
            : base(() => string.IsNullOrEmpty(key) ? string.Empty : string.IsNullOrEmpty(filename) ? key.Translate()?.ApplyCase(casing) : key.Translate(filename)?.ApplyCase(casing)) => Key = key;

        public override bool Equals(object? obj) => obj is TranslatableString o && Key.Equals(o.Key);

        public override int GetHashCode() => Key.GetHashCode();
    }
}
