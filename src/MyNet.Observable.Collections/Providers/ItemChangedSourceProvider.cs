// -----------------------------------------------------------------------
// <copyright file="ItemChangedSourceProvider.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.ObjectModel;
using System.Reactive.Subjects;
using DynamicData;
using DynamicData.Binding;
using MyNet.DynamicData.Extensions;

namespace MyNet.Observable.Collections.Providers;

public class ItemChangedSourceProvider<T, TItem> : ISourceProvider<T>, IDisposable
    where T : notnull
{
    private readonly ExtendedObservableCollection<T> _source = [];
    private readonly IObservable<IChangeSet<T>> _observable;
    private readonly IDisposable _subjectSubscription;
    private IDisposable? _sourceSubscription;
    private bool _disposedValue;

    public ItemChangedSourceProvider(Subject<TItem?> subject, Func<TItem, IObservable<IChangeSet<T>>> provideNewObservableSource)
    {
        Source = new(_source);
        _observable = Source.ToObservableChangeSet();

        _subjectSubscription = subject.Subscribe(x =>
        {
            if (x is not null)
                SetSource(provideNewObservableSource.Invoke(x));
            else
                ClearSource();
        });
    }

    public ReadOnlyObservableCollection<T> Source { get; }

    public IObservable<IChangeSet<T>> Connect() => _observable;

    private void SetSource(IObservable<IChangeSet<T>> source)
    {
        ClearSource();
        _sourceSubscription = source.Bind(_source).Subscribe();
    }

    protected void ClearSource()
    {
        _sourceSubscription?.Dispose();
        _source.Clear();
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposedValue) return;

        if (disposing)
        {
            _sourceSubscription?.Dispose();
            _subjectSubscription.Dispose();
        }

        _disposedValue = true;
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
