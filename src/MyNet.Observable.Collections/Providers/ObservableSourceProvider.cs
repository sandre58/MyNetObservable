// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.ObjectModel;
using DynamicData;
using DynamicData.Binding;
using MyNet.Utilities;

namespace MyNet.Observable.Collections.Providers
{
    public class ObservableSourceProvider<T> : ISourceProvider<T>, IDisposable
        where T : notnull
    {
        private bool _disposedValue;
        private readonly ObservableCollectionExtended<T> _source = [];
        private readonly IObservable<IChangeSet<T>> _observable;
        private readonly IDisposable? _sourceSubscription;

        public ObservableSourceProvider(ObservableCollection<T> source) : this(source.ToObservableChangeSet()) { }

        public ObservableSourceProvider(ReadOnlyObservableCollection<T> source) : this(source.ToObservableChangeSet()) { }

        public ObservableSourceProvider(IObservable<IChangeSet<T>> source)
        {
            Source = new(_source);
            _observable = Source.ToObservableChangeSet();
            _sourceSubscription = source.Bind(_source).Subscribe();
        }

        public ReadOnlyObservableCollection<T> Source { get; }

        public IObservable<IChangeSet<T>> Connect() => _observable;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _sourceSubscription?.Dispose();
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

    public class ObservableSourceProvider<T, TKey> : ObservableSourceProvider<T>, ISourceProvider<T, TKey>
    where T : IIdentifiable<TKey>
        where TKey : notnull
    {
        private readonly IObservable<IChangeSet<T, TKey>> _observableById;

        public ObservableSourceProvider(ObservableCollection<T> source) : base(source) => _observableById = Source.ToObservableChangeSet(x => x.Id);

        public ObservableSourceProvider(ReadOnlyObservableCollection<T> source) : base(source) => _observableById = Source.ToObservableChangeSet(x => x.Id);

        public ObservableSourceProvider(IObservable<IChangeSet<T>> source) : base(source) => _observableById = Source.ToObservableChangeSet(x => x.Id);

        public IObservable<IChangeSet<T, TKey>> ConnectById() => _observableById;
    }
}
