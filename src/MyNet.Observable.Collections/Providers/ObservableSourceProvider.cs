// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using DynamicData;
using DynamicData.Binding;
using MyNet.Utilities;
using MyNet.Utilities.Providers;

namespace MyNet.Observable.Collections.Providers
{
    public class ObservableSourceProvider<T> : ISourceProvider<T>, IDisposable, IItemsProvider<T>
        where T : notnull
    {
        private bool _disposedValue;
        private readonly ObservableCollectionExtended<T> _source = [];
        private readonly IObservable<IChangeSet<T>> _observable;

        protected IDisposable? SourceSubscription { get; set; }

        public ReadOnlyObservableCollection<T> Source { get; }

        public ObservableSourceProvider()
        {
            Source = new(_source);
            _observable = Source.ToObservableChangeSet();
        }

        public ObservableSourceProvider(IEnumerable<T> source) : this() => SetSource(source);

        public ObservableSourceProvider(ObservableCollection<T> source) : this() => SetSource(source.ToObservableChangeSet());

        public ObservableSourceProvider(ReadOnlyObservableCollection<T> source) : this() => SetSource(source.ToObservableChangeSet());

        public ObservableSourceProvider(IObservable<IChangeSet<T>> source) : this() => SetSource(source);

        public IObservable<IChangeSet<T>> Connect() => _observable;

        IEnumerable<T> IItemsProvider<T>.ProvideItems() => Source;

        protected void ClearSource()
        {
            SourceSubscription?.Dispose();

            _source.Clear();
        }

        protected void SetSource(IEnumerable<T> source)
        {
            ClearSource();
            _source.Load(source);
        }

        protected void SetSource(IObservable<IChangeSet<T>> source)
        {
            ClearSource();
            SourceSubscription = source.Bind(_source).Subscribe();
        }

        protected void SetSource<TKey>(IObservable<IChangeSet<T, TKey>> source)
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

    public class SourceProvider<T, TKey> : ObservableSourceProvider<T>
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
