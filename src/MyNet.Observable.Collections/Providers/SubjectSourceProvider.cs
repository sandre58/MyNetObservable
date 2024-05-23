// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;
using System.ComponentModel;
using System.Reactive.Subjects;
using DynamicData;
using MyNet.Utilities;

namespace MyNet.Observable.Collections.Providers
{
    internal class SubjectSourceProvider<T, TItem> : ObservableSourceProvider<T>
        where T : IIdentifiable<Guid>, INotifyPropertyChanged
    {
        private readonly IDisposable _disposable;

        public SubjectSourceProvider(Subject<TItem?> subject, Func<TItem, IObservable<IChangeSet<T, Guid>>> provideNewObservableSource)
            => _disposable = subject.Subscribe(x =>
            {
                if (x is not null)
                    SetSource(provideNewObservableSource.Invoke(x));
                else
                    ClearSource();
            });

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            _disposable.Dispose();
        }
    }
}
