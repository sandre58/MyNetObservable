// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using DynamicData;
using DynamicData.Binding;
using MyNet.DynamicData.Extensions;
using MyNet.Observable.Collections.Extensions;
using MyNet.Observable.Collections.Filters;
using MyNet.Observable.Collections.Providers;
using MyNet.Observable.Collections.Sorting;
using MyNet.Utilities;
using MyNet.Utilities.Deferring;
using MyNet.Utilities.Providers;

namespace MyNet.Observable.Collections
{
    [Serializable]
    [System.Runtime.InteropServices.ComVisible(false)]
    [DebuggerDisplay("Count = {Count}/{SourceCount}")]
    public class ExtendedCollection<T> : ObservableObject, ICollection<T>, IReadOnlyList<T>, INotifyCollectionChanged where T : notnull
    {
        private readonly Deferrer _applyFilterDeferrer;
        private readonly Deferrer _applySortDeferrer;
        private readonly Subject<IList<CompositeFilter>> _filterSubject = new();
        private readonly Subject<Unit> _resortSubject = new();
        private readonly IObservable<IChangeSet<T>> _observableSource;
        private readonly IObservable<IChangeSet<T>> _observable;
        private readonly ReadOnlyObservableCollection<T> _items;
        private readonly ReadOnlyObservableCollection<T> _sortedSource;
        private readonly SourceList<T> _source;
        private readonly SortingComparer<T> _sortComparer;
        private readonly bool _readOnly;
        private readonly Action? _reload;

        public FiltersCollection Filters { get; } = [];

        public SortingPropertiesCollection SortingProperties { get; } = [];

        public ReadOnlyObservableCollection<T> Source => _sortedSource;

        public ReadOnlyObservableCollection<T> Items => _items;

        public int SourceCount => _source.Count;

        public ExtendedCollection(IScheduler? scheduler = null) : this(new SourceList<T>(), false, scheduler) { }

        public ExtendedCollection(ICollection<T> source, IScheduler? scheduler = null) : this(new SourceList<T>(), source.IsReadOnly, scheduler) => AddRange(source);

        public ExtendedCollection(IItemsProvider<T> source, IScheduler? scheduler = null) : this(new ItemsSourceProvider<T>(source), scheduler) { }

        public ExtendedCollection(ISourceProvider<T> source, IScheduler? scheduler = null) : this(source.Connect(), scheduler)
            => _reload = () => source.Reload();

        public ExtendedCollection(IObservable<IChangeSet<T>> source, IScheduler? scheduler = null) : this(new SourceList<T>(source), true, scheduler) { }

        protected ExtendedCollection(
            SourceList<T> sourceList,
            bool isReadOnly,
            IScheduler? scheduler = null)
        {
            _source = sourceList;
            _readOnly = isReadOnly;
            _applyFilterDeferrer = new Deferrer(() => _filterSubject.OnNext([.. Filters]));
            _applySortDeferrer = new Deferrer(() => _resortSubject.OnNext(Unit.Default));
            _observableSource = _source.Connect();
            _sortComparer = new SortingComparer<T>(SortingProperties);

            Disposables.AddRange(
            [
                System.Reactive.Linq.Observable.FromEventPattern(x => Filters.FiltersChanged += x, x => Filters.FiltersChanged -= x).Subscribe(_ => _applyFilterDeferrer.DeferOrExecute()),
                System.Reactive.Linq.Observable.FromEventPattern(x => SortingProperties.SortChanged += x, x => SortingProperties.SortChanged -= x).Subscribe(_ =>  _applySortDeferrer.DeferOrExecute()),

                ConnectSortedSource().ObserveOnOptional(scheduler)
                                     .Bind(out _sortedSource)
                                     .Subscribe(OnSourceRefreshed),
                ConnectSortedAndFilteredSource().ObserveOnOptional(scheduler)
                                                .Bind(out _items)
                                                .Subscribe(OnItemsRefreshed),
                _observableSource.SubscribeMany(x => System.Reactive.Linq.Observable.FromEventPattern<PropertyChangedEventHandler, PropertyChangedEventArgs>(
                                                        handler => x?.IfIs<INotifyPropertyChanged>(y => y.PropertyChanged += handler),
                                                        handler => x?.IfIs<INotifyPropertyChanged>(y => y.PropertyChanged -= handler)
                                                    ).Subscribe(x =>
                                                    {
                                                        if(SortingProperties.Select(y => y.PropertyName).Contains(x.EventArgs.PropertyName.OrEmpty()))
                                                            _applySortDeferrer.DeferOrExecute();

                                                         if(Filters.Select(y => y.Filter.PropertyName).Any(y => string.IsNullOrEmpty(y) || y == x.EventArgs.PropertyName.OrEmpty()))
                                                            _applyFilterDeferrer.DeferOrExecute();
                                                    })).Subscribe(),
                 System.Reactive.Linq.Observable.FromEventPattern<NotifyCollectionChangedEventHandler, NotifyCollectionChangedEventArgs>(x => ((INotifyCollectionChanged)_items).CollectionChanged += x, x => ((INotifyCollectionChanged)_items).CollectionChanged -= x).Subscribe(x =>  HandleCollectionChanged(x.EventArgs)),
                 System.Reactive.Linq.Observable.FromEventPattern<PropertyChangedEventHandler, PropertyChangedEventArgs>(x => ((INotifyPropertyChanged)_items).PropertyChanged += x, x => ((INotifyPropertyChanged)_items).PropertyChanged -= x).Subscribe(x =>  RaisePropertyChanged(x.EventArgs.PropertyName)),
                _filterSubject,
                _resortSubject,
                _source
            ]);

            _observable = _items.ToObservableChangeSet();

            Refresh();
        }

        protected IObservable<IChangeSet<T>> ConnectSortedSource() => _observableSource.Sort(_sortComparer, resort: _resortSubject);

        protected IObservable<IChangeSet<T>> ConnectSortedAndFilteredSource() => _observableSource.Filter(_filterSubject.Select(GetFilterFunc), ListFilterPolicy.CalculateDiff).Sort(_sortComparer, resort: _resortSubject);

        public IObservable<IChangeSet<T>> Connect() => _observable;

        public IObservable<IChangeSet<T>> ConnectSource() => _observableSource;

        protected virtual void OnItemsRefreshed(IChangeSet<T> changeSet) => RaisePropertyChanged(nameof(Count));

        protected virtual void OnSourceRefreshed(IChangeSet<T> changeSet) => RaisePropertyChanged(nameof(SourceCount));

        public IDisposable DeferRefresh() => new CompositeDisposable(_applySortDeferrer.Defer(), _applyFilterDeferrer.Defer());

        public IDisposable DeferSort() => _applySortDeferrer.Defer();

        public IDisposable DeferFilter() => _applyFilterDeferrer.Defer();

        public void Refresh()
        {
            RefreshFilter();
            RefreshSorting();
        }

        public void RefreshSorting() => _applySortDeferrer.Execute();

        public void RefreshFilter() => _applyFilterDeferrer.Execute();

        public void Reload() => _reload?.Invoke();

        private Func<T, bool> GetFilterFunc(IList<CompositeFilter> filters) => x => filters.Count == 0 || filters.Match(x);

        #region INotifyCollectionChanged

        event NotifyCollectionChangedEventHandler? INotifyCollectionChanged.CollectionChanged
        {
            add => CollectionChanged += value;
            remove => CollectionChanged -= value;
        }

        private event NotifyCollectionChangedEventHandler? CollectionChanged;

        private void HandleCollectionChanged(NotifyCollectionChangedEventArgs e) => CollectionChanged?.Invoke(this, e);

        #endregion INotifyCollectionChanged

        #region ICollection

        public int Count => _items.Count;

        public bool IsReadOnly => _readOnly;

        public T this[int index] => _items[index];

        public void Add(T item) => IsReadOnly.IfFalse(() => _source.Add(item));

        public void Clear() => IsReadOnly.IfFalse(() => _source.Clear());

        public bool Remove(T item) => !IsReadOnly && _source.Remove(item);

        public int IndexOf(T item) => _items.IndexOf(item);

        public bool Contains(T item) => _items.Contains(item);

        public void CopyTo(T[] array, int arrayIndex) => _items.CopyTo(array, arrayIndex);

        public IEnumerator<T> GetEnumerator() => _items.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _items.GetEnumerator();

        #endregion

        #region ICollection Extensions

        public void AddRange(IEnumerable<T> items) => IsReadOnly.IfFalse(() => _source.AddRange(items));

        public void RemoveMany(IEnumerable<T> itemsToRemove) => IsReadOnly.IfFalse(() => _source.RemoveMany(itemsToRemove));

        public void Set(IEnumerable<T> items) => IsReadOnly.IfFalse(() => _source.Edit(x =>
        {
            x.Clear();
            x.AddRange(items);
        }));

        #endregion
    }
}
