// -----------------------------------------------------------------------
// <copyright file="ISourceProvider.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using DynamicData;
using MyNet.Utilities.Providers;

namespace MyNet.Observable.Collections.Providers;

public interface ISourceProvider<T> : IItemsProvider<T>
    where T : notnull
{
    ReadOnlyObservableCollection<T> Source { get; }

    IObservable<IChangeSet<T>> Connect();

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1033:Interface methods should be callable by child types", Justification = "Use Source")]
    IEnumerable<T> IItemsProvider<T>.ProvideItems() => Source;
}

public interface ISourceProvider<T, TKey> : ISourceProvider<T>
    where T : notnull
    where TKey : notnull
{
    IObservable<IChangeSet<T, TKey>> ConnectById();
}
