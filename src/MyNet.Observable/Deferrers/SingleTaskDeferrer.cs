﻿// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading;
using MyNet.Utilities.Threading;

namespace MyNet.Observable.Deferrers
{
    public class SingleTaskDeferrer : IDisposable
    {
        private readonly RefreshDeferrer _refreshDeferrer = new();
        private readonly SingleTaskRunner _task;
        private bool _disposedValue;
        private bool _musteBeReRun = false;

        public SingleTaskDeferrer(Action<CancellationToken> action, Action<bool>? onRunningChanged = null, Action? onCancelled = null, int throttle = 0)
        {
            _task = new SingleTaskRunner(action, x =>
            {
                onRunningChanged?.Invoke(x);

                if (!x && _musteBeReRun)
                {
                    _musteBeReRun = false;
                    _task?.Run();
                }
            }, onCancelled);

            _refreshDeferrer.Subscribe(this, () =>
            {
                if (_task.IsRunning)
                {
                    _musteBeReRun = true;
                    _task.Cancel();
                }
                else
                    _task.Run();
            }, throttle);
        }
        public virtual bool IsRunning() => _task.IsRunning;

        public virtual void Cancel() => _task.Cancel();

        public virtual IDisposable Defer() => _refreshDeferrer.Defer();

        public virtual IDisposable Suspend() => _refreshDeferrer.Suspend();

        public virtual void AskRefresh() => _refreshDeferrer.AskRefresh();

        public virtual bool IsDeferred() => _refreshDeferrer.IsDeferred();

        public virtual bool IsSuspended() => _refreshDeferrer.IsSuspended();

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _refreshDeferrer.Dispose();
                    _task.Dispose();
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
