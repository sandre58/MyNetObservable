// -----------------------------------------------------------------------
// <copyright file="ItemsSourceProvider.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using DynamicData;
using DynamicData.Binding;
using MyNet.Utilities.Providers;

namespace MyNet.Observable.Collections.Providers;

public class ItemsSourceProvider<T> : ISourceProvider<T>
    where T : notnull
{
    private readonly IItemsProvider<T> _provider;
    private readonly ObservableCollectionExtended<T> _source = [];
    private readonly IObservable<IChangeSet<T>> _observable;

    public ReadOnlyObservableCollection<T> Source { get; }

    public ItemsSourceProvider(IEnumerable<T> source, bool loadItems = true)
        : this(new ItemsProvider<T>(source), loadItems) { }

    public ItemsSourceProvider(IItemsProvider<T> provider, bool loadItems = true)
    {
        Source = new(_source);
        _observable = Source.ToObservableChangeSet();
        _provider = provider;

        if (loadItems)
            _source.Load(_provider.ProvideItems());
    }

    public IObservable<IChangeSet<T>> Connect() => _observable;

    public void Clear() => _source.Clear();

    public virtual void Reload() => _source.Load(_provider.ProvideItems());
}
