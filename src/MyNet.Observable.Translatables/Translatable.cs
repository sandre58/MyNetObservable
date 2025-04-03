// -----------------------------------------------------------------------
// <copyright file="Translatable.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using MyNet.Observable.Attributes;

namespace MyNet.Observable.Translatables;

public class Translatable<T>(Func<T?> provideValue) : LocalizableObject, IProvideValue<T>
{
    [UpdateOnCultureChanged]
    public virtual T? Value => provideValue.Invoke();

    public override string? ToString() => Value?.ToString();
}
