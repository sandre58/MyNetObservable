// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using DynamicData;
using MyNet.Utilities.Providers;

namespace MyNet.Observable.Collections.Providers
{
    public interface ISourceProvider<T> : IItemsProvider<T> where T : notnull
    {
        ReadOnlyObservableCollection<T> Source { get; }

        IObservable<IChangeSet<T>> Connect();

        IEnumerable<T> IItemsProvider<T>.ProvideItems() => Source;
    }

    public interface ISourceProvider<T, TKey> : ISourceProvider<T> where T : notnull where TKey : notnull
    {
        IObservable<IChangeSet<T, TKey>> ConnectById();
    }
}
