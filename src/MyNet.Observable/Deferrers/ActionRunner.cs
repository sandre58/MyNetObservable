// -----------------------------------------------------------------------
// <copyright file="ActionRunner.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using MyNet.Utilities;

namespace MyNet.Observable.Deferrers;

public sealed class ActionRunner : IDisposable
{
    private readonly Func<Subject<bool>, bool> _actionToRun;
    private readonly Dictionary<object, List<Func<IDisposable>>> _subscribers = [];
    private readonly Dictionary<object, List<Action>> _startActions = [];
    private readonly Dictionary<object, List<Action>> _endActions = [];
    private readonly Subject<bool> _runningSubject = new();
    private readonly Stopwatch _stopWatch = new();
    private readonly bool _useStopWatch;
    private readonly Subject<bool> _endSubject = new();
    private readonly IDisposable _disposable;
    private CompositeDisposable _disposables = [];

    public ActionRunner(Action action, bool useStopWatch = false)
        : this(_ =>
            {
                action();
                return true;
            },
              useStopWatch)
    { }

    public ActionRunner(Action<Subject<bool>> action, bool useStopWatch = false)
        : this(x =>
            {
                action(x);
                return false;
            },
              useStopWatch)
    { }

    private ActionRunner(Func<Subject<bool>, bool> action, bool useStopWatch = false)
    {
        _actionToRun = action;
        _useStopWatch = useStopWatch;
        _disposable = _endSubject.Subscribe(_ => End());
    }

    public bool IsRunning { get; private set; }

    public void Run()
    {
        IsRunning = true;
        _runningSubject.OnNext(IsRunning);
        _startActions.SelectMany(x => x.Value).ForEach(x => x.Invoke());
        _disposables = [.. _subscribers.SelectMany(x => x.Value).Select(x => x.Invoke()).ToList()];
        var continueWithEnd = true;

        try
        {
            if (_useStopWatch) _stopWatch.Start();
            continueWithEnd = _actionToRun(_endSubject);
        }
        finally
        {
            if (_useStopWatch) _stopWatch.Stop();

            if (continueWithEnd)
            {
                End();
            }
        }
    }

    private void End()
    {
        IsRunning = false;
        _runningSubject.OnNext(IsRunning);
        _endActions.SelectMany(x => x.Value).ForEach(x => x.Invoke());
        _disposables.Dispose();
    }

    public TimeSpan LastTimeElapsed => _stopWatch.Elapsed;

    public IObservable<bool> WhenStart() => _runningSubject.Where(x => x);

    public IObservable<bool> WhenEnd() => _runningSubject.Where(x => !x);

    public void Register(object subscriber, Func<IDisposable> createScope) => _subscribers.AddOrUpdate(subscriber, _subscribers.GetOrDefault(subscriber)?.Concat([createScope]).ToList() ?? [createScope]);

    public void Register(object subscriber, IEnumerable<Func<IDisposable>> createScopes)
    {
        var toList = createScopes.ToList();
        _subscribers.AddOrUpdate(subscriber, _subscribers.GetOrDefault(subscriber)?.Concat(toList).ToList() ?? [.. toList]);
    }

    public void RegisterOnStart(object subscriber, Action action) => _startActions.AddOrUpdate(subscriber, _startActions.GetOrDefault(subscriber)?.Concat([action]).ToList() ?? [action]);

    public void RegisterOnEnd(object subscriber, Action action) => _endActions.AddOrUpdate(subscriber, _endActions.GetOrDefault(subscriber)?.Concat([action]).ToList() ?? [action]);

    public void Unregister(object subscriber)
    {
        _subscribers.Remove(subscriber);
        _endActions.Remove(subscriber);
        _startActions.Remove(subscriber);
    }

    public void Dispose()
    {
        _disposable.Dispose();
        _endSubject.Dispose();
        _runningSubject.Dispose();
        if (!_disposables.IsDisposed) _disposables.Dispose();
    }
}

public sealed class ActionRunner<TIn, TOut> : IDisposable
{
    private readonly Func<TIn, Subject<TOut>, bool> _actionToRun;
    private readonly Dictionary<object, List<Func<IDisposable>>> _subscribers = [];
    private readonly Dictionary<object, List<Action<TOut>>> _startActions = [];
    private readonly Dictionary<object, List<Action<TOut>>> _endActions = [];
    private readonly Subject<TOut> _startSubject = new();
    private readonly Subject<TOut> _endSubject = new();
    private readonly Stopwatch _stopWatch = new();
    private readonly bool _useStopWatch;
    private readonly IDisposable _disposable;
    private readonly Subject<TOut> _forceEndSubject = new();
    private CompositeDisposable _disposables = [];

    public ActionRunner(Action<TIn> action, bool useStopWatch = false)
        : this((x, _) =>
                {
                    action(x);
                    return true;
                },
              useStopWatch)
    { }

    public ActionRunner(Action<TIn, Subject<TOut>> action, bool useStopWatch = false)
        : this((x, y) =>
                {
                    action(x, y);
                    return false;
                },
              useStopWatch)
    { }

    private ActionRunner(Func<TIn, Subject<TOut>, bool> action, bool useStopWatch = false)
    {
        _actionToRun = action;
        _useStopWatch = useStopWatch;
        _disposable = _forceEndSubject.Subscribe(End);
    }

    public bool IsRunning { get; private set; }

    public void Run(TIn obj, Func<TOut> result)
    {
        IsRunning = true;
        _startSubject.OnNext(result());
        _startActions.SelectMany(x => x.Value).ForEach(x => x.Invoke(result()));
        _disposables = [.. _subscribers.SelectMany(x => x.Value).Select(x => x.Invoke()).ToList()];
        var continueWithEnd = true;

        try
        {
            if (_useStopWatch) _stopWatch.Start();
            continueWithEnd = _actionToRun(obj, _forceEndSubject);
        }
        finally
        {
            if (_useStopWatch) _stopWatch.Stop();

            if (continueWithEnd)
            {
                End(result());
            }
        }
    }

    private void End(TOut obj)
    {
        IsRunning = false;
        _endSubject.OnNext(obj);
        _endActions.SelectMany(x => x.Value).ForEach(x => x.Invoke(obj));
        _disposables.Dispose();
    }

    public TimeSpan LastTimeElapsed => _stopWatch.Elapsed;

    public IObservable<TOut> WhenStart() => _startSubject;

    public IObservable<TOut> WhenEnd() => _endSubject;

    public void Register(object subscriber, Func<IDisposable> createScope) => _subscribers.AddOrUpdate(subscriber, _subscribers.GetOrDefault(subscriber)?.Concat([createScope]).ToList() ?? [createScope]);

    public void Register(object subscriber, IEnumerable<Func<IDisposable>> createScopes)
    {
        var toList = createScopes.ToList();
        _subscribers.AddOrUpdate(subscriber, _subscribers.GetOrDefault(subscriber)?.Concat(toList).ToList() ?? [.. toList]);
    }

    public void RegisterOnStart(object subscriber, Action<TOut> action) => _startActions.AddOrUpdate(subscriber, _startActions.GetOrDefault(subscriber)?.Concat([action]).ToList() ?? [action]);

    public void RegisterOnEnd(object subscriber, Action<TOut> action) => _endActions.AddOrUpdate(subscriber, _endActions.GetOrDefault(subscriber)?.Concat([action]).ToList() ?? [action]);

    public void Unregister(object subscriber)
    {
        _subscribers.Remove(subscriber);
        _endActions.Remove(subscriber);
        _startActions.Remove(subscriber);
    }

    public void Dispose()
    {
        _disposable.Dispose();
        _startSubject.Dispose();
        _endSubject.Dispose();
        _forceEndSubject.Dispose();
        if (!_disposables.IsDisposed) _disposables.Dispose();
    }
}
