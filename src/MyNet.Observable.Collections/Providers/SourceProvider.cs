// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using DynamicData;
using DynamicData.Binding;
using MyNet.Utilities;

namespace MyNet.Observable.Collections.Providers
{
    public class SourceProvider<T> : ISourceProvider<T>, IDisposable, IItemsProvider<T>
        where T : notnull
    {
        private bool _disposedValue;
        private readonly ObservableCollectionExtended<T> _source = [];
        private readonly IObservable<IChangeSet<T>> _observable;

        protected IDisposable? SourceSubscription { get; set; }

        public ReadOnlyObservableCollection<T> Source { get; }

        public SourceProvider()
        {
            Source = new(_source);
            _observable = Source.ToObservableChangeSet();
        }

        public SourceProvider(IEnumerable<T> source) : this() => SetSource(source);

        public SourceProvider(ObservableCollection<T> source) : this() => SetSource(source.ToObservableChangeSet());

        public SourceProvider(ReadOnlyObservableCollection<T> source) : this() => SetSource(source.ToObservableChangeSet());

        public SourceProvider(IObservable<IChangeSet<T>> source) : this() => SetSource(source);

        public IObservable<IChangeSet<T>> Connect() => _observable;

        IEnumerable<T> IItemsProvider<T>.ProvideItems() => Source;

        public void ClearSource()
        {
            SourceSubscription?.Dispose();

            _source.Clear();
        }

        public void SetSource(IEnumerable<T> source)
        {
            ClearSource();
            _source.Load(source);
        }

        public void SetSource(IObservable<IChangeSet<T>> source)
        {
            ClearSource();
            SourceSubscription = source.Bind(_source).Subscribe();
        }

        public void SetSource<TKey>(IObservable<IChangeSet<T, TKey>> source)
            where TKey : notnull
        {
            ClearSource();
            SourceSubscription = source.Bind(_source).Subscribe();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    SourceSubscription?.Dispose();
                }

                _disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }

    public class SourceProvider<T, TKey> : SourceProvider<T>
    where T : IIdentifiable<TKey>
        where TKey : notnull
    {
        private readonly IObservable<IChangeSet<T, TKey>> _observableById;

        public SourceProvider() => _observableById = Source.ToObservableChangeSet(x => x.Id);

        public SourceProvider(IEnumerable<T> source) : this() => SetSource(source);

        public SourceProvider(ObservableCollection<T> source) : this() => SetSource(source.ToObservableChangeSet());

        public SourceProvider(ReadOnlyObservableCollection<T> source) : this() => SetSource(source.ToObservableChangeSet());

        public SourceProvider(IObservable<IChangeSet<T>> source) : this() => SetSource(source);

        public IObservable<IChangeSet<T, TKey>> ConnectById() => _observableById;
    }
}
