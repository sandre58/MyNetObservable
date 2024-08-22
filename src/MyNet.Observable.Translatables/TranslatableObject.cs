// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;
using MyNet.Observable.Attributes;

namespace MyNet.Observable.Translatables
{
    public class TranslatableObject<T> : LocalizableObject, IProvideValue<T>
    {
        private readonly Func<T?> _provideValue;

        public TranslatableObject(Func<T?> provideValue) => _provideValue = provideValue;

        [UpdateOnCultureChanged]
        public virtual T? Value => _provideValue != null ? _provideValue.Invoke() : default;

        public override string? ToString() => Value?.ToString();
    }
}
