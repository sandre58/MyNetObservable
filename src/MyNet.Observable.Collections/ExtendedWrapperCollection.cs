// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using DynamicData;
using DynamicData.Binding;
using MyNet.DynamicData.Extensions;
using MyNet.Observable.Collections.Providers;
using MyNet.Utilities;
using MyNet.Utilities.Providers;

namespace MyNet.Observable.Collections
{
    public class ExtendedWrapperCollection<T, TWrapper> : ExtendedCollection<T>
        where TWrapper : IWrapper<T>
        where T : notnull
    {
        private readonly Func<T, TWrapper> _createWrapper;
        private readonly ReadOnlyObservableCollection<TWrapper> _wrappersSource;
        private readonly ReadOnlyObservableCollection<TWrapper> _wrappers;
        private readonly IObservable<IChangeSet<TWrapper>> _observableWrapperSource;
        private readonly IObservable<IChangeSet<TWrapper>> _observableWrappers;
        private readonly Dictionary<T, TWrapper> _cache = [];

        public ReadOnlyObservableCollection<TWrapper> Wrappers => _wrappers;

        public ReadOnlyObservableCollection<TWrapper> WrappersSource => _wrappersSource;

        public ExtendedWrapperCollection(ICollection<T> source, IScheduler? scheduler = null, Func<T, TWrapper>? createWrapper = null)
            : this(new SourceList<T>(), source.IsReadOnly, scheduler, createWrapper) => AddRange(source);

        public ExtendedWrapperCollection(IItemsProvider<T> source, IScheduler? scheduler = null, Func<T, TWrapper>? createWrapper = null)
            : this(new ItemsSourceProvider<T>(source), scheduler) { }

        public ExtendedWrapperCollection(ISourceProvider<T> source, IScheduler? scheduler = null, Func<T, TWrapper>? createWrapper = null)
            : this(source.Connect(), scheduler, createWrapper) { }

        public ExtendedWrapperCollection(IObservable<IChangeSet<T>> source, IScheduler? scheduler = null, Func<T, TWrapper>? createWrapper = null)
            : this(new SourceList<T>(source), true, scheduler, createWrapper) { }

        public ExtendedWrapperCollection(IScheduler? scheduler = null, Func<T, TWrapper>? createWrapper = null)
            : this(new SourceList<T>(), false, scheduler, createWrapper) { }

        protected ExtendedWrapperCollection(
            SourceList<T> sourceList,
            bool isReadOnly,
            IScheduler? scheduler = null,
            Func<T, TWrapper>? createWrapper = null)
            : base(sourceList, isReadOnly, scheduler)
        {
            _createWrapper = createWrapper ?? new Func<T, TWrapper>(x => (TWrapper)Activator.CreateInstance(typeof(TWrapper), x)!);

            Disposables.AddRange(
            [
                ConnectSortedSource().Transform(GetOrCreate)
                                     .ObserveOnOptional(scheduler)
                                     .Bind(out _wrappersSource)
                                     .DisposeMany()
                                     .Subscribe(),
                ConnectSortedAndFilteredSource().Transform(GetOrCreate)
                                                .ObserveOnOptional(scheduler)
                                                .Bind(out _wrappers)
                                                .Subscribe()
            ]);

            _observableWrappers = _wrappers.ToObservableChangeSet();
            _observableWrapperSource = _wrappersSource.ToObservableChangeSet();

            Refresh();
        }

        protected TWrapper GetOrCreate(T item)
        {
            if (!_cache.ContainsKey(item))
                _cache.Add(item, _createWrapper(item));

            return _cache[item];
        }

        public IObservable<IChangeSet<TWrapper>> ConnectWrappers() => _observableWrappers;

        public IObservable<IChangeSet<TWrapper>> ConnectWrappersSource() => _observableWrapperSource;
    }
}
