﻿// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using MyNet.Utilities;
using MyNet.Utilities.Deferring;
using MyNet.Utilities.Suspending;

namespace MyNet.Observable.Deferrers
{
    public class RefreshDeferrer : IDisposable
    {
        private readonly Subject<bool> _refreshSubject = new();
        private readonly Deferrer _deferrer;
        private readonly Suspender _suspender = new();
        private bool _disposedValue;
        private readonly Dictionary<object, CompositeDisposable> _disposables = [];

        public RefreshDeferrer() => _deferrer = new(() =>
        {
            if (_suspender.IsSuspended) return;

            _refreshSubject.OnNext(true);
        });

        public virtual void Subscribe(object obj, Action action, int throttle = 0)
        {
            IObservable<bool> obs = _refreshSubject;

            if (throttle > 0)
                obs = _refreshSubject.Throttle(TimeSpan.FromMilliseconds(throttle));

            var disposable = obs.Subscribe(_ => action());

            if (_disposables.TryGetValue(obj, out var value))
                value.Add(disposable);
            else
                _disposables.Add(obj, [disposable]);
        }

        public virtual void Unsubscribe(object obj)
        {
            if (_disposables.TryGetValue(obj, out var value))
            {
                value.Dispose();
                _disposables.Remove(obj);
            }
        }

        public virtual IDisposable Defer() => _deferrer.Defer();

        public virtual IDisposable Suspend() => _suspender.Suspend();

        public virtual void AskRefresh() => _deferrer.DeferOrExecute();

        public virtual bool IsDeferred() => _deferrer.IsDeferred;

        public virtual bool IsSuspended() => _suspender.IsSuspended;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _disposables.Values.ForEach(x => x.Dispose());
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
}
