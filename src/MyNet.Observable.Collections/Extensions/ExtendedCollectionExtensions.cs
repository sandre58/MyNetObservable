// -----------------------------------------------------------------------
// <copyright file="ExtendedCollectionExtensions.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using DynamicData;
using DynamicData.Binding;

namespace MyNet.Observable.Collections.Extensions;

public static class ExtendedCollectionExtensions
{
    public static IObservable<IChangeSet<T>> ToObservableChangeSet<T>(this ExtendedCollection<T> source)
        where T : notnull
        => source.ToObservableChangeSet<ExtendedCollection<T>, T>();
}
