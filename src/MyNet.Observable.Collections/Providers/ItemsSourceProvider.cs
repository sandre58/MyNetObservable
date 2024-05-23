// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using DynamicData;
using DynamicData.Binding;
using MyNet.Utilities.Providers;

namespace MyNet.Observable.Collections.Providers
{
    public class ItemsSourceProvider<T> : ISourceProvider<T>
        where T : notnull
    {
        private readonly IItemsProvider<T> _provider;
        private readonly ObservableCollectionExtended<T> _source = [];
        private readonly IObservable<IChangeSet<T>> _observable;

        public ReadOnlyObservableCollection<T> Source { get; }

        public ItemsSourceProvider(IEnumerable<T> source) : this(new ItemsProvider<T>(source)) { }

        public ItemsSourceProvider(IItemsProvider<T> provider)
        {
            Source = new(_source);
            _observable = Source.ToObservableChangeSet();
            _provider = provider;
        }

        public IObservable<IChangeSet<T>> Connect() => _observable;

        public void Reload() => _source.Load(_provider.ProvideItems());
    }
}
