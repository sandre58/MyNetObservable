// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reactive.Concurrency;
using System.Threading.Tasks;
using DynamicData.Binding;
using PropertyChanged;

namespace MyNet.Observable.Collections
{
    public class ThreadSafeObservableCollection<T> : ObservableCollectionExtended<T>
    {
        private readonly object _localLock = new();

        public ThreadSafeObservableCollection() { }

        public ThreadSafeObservableCollection(List<T> list) : base(list) { }

        public ThreadSafeObservableCollection(IEnumerable<T> collection) : base(collection) { }

        public override event NotifyCollectionChangedEventHandler? CollectionChanged;

        protected override void InsertItem(int index, T item) => ExecuteThreadSafe(() => base.InsertItem(index, item));

        protected override void MoveItem(int oldIndex, int newIndex) => ExecuteThreadSafe(() => base.MoveItem(oldIndex, newIndex));

        protected override void RemoveItem(int index) => ExecuteThreadSafe(() => base.RemoveItem(index));

        protected override void SetItem(int index, T item) => ExecuteThreadSafe(() => base.SetItem(index, item));

        protected override void ClearItems() => ExecuteThreadSafe(base.ClearItems);

        [SuppressPropertyChangedWarnings]
        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            using (BlockReentrancy())
            {
                var collectionChanged = CollectionChanged;
                if (collectionChanged != null)
                    NotifyCollectionChanged(e, collectionChanged);
            }
        }

        private void NotifyCollectionChanged(NotifyCollectionChangedEventArgs e, NotifyCollectionChangedEventHandler collectionChanged)
        {
            foreach (var notifyEventHandler in collectionChanged.GetInvocationList().OfType<NotifyCollectionChangedEventHandler>())
            {
                try
                {
                    Schedule(() => InvokeNotifyCollectionChanged(notifyEventHandler, e));
                }
                catch (TaskCanceledException)
                {
                    // Opeation has canceled by the system
                }
            }
        }

        protected virtual void InvokeNotifyCollectionChanged(NotifyCollectionChangedEventHandler notifyEventHandler, NotifyCollectionChangedEventArgs e) => notifyEventHandler.Invoke(this, e);

        protected void ExecuteThreadSafe(Action action)
        {
            lock (_localLock)
            {
                action();
            }
        }

        protected virtual void Schedule(Action action) => _ = Threading.Scheduler.GetUIOrCurrent().Schedule(action);
    }

}
