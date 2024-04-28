// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using PropertyChanged;

namespace MyNet.Observable.Collections
{
    public class SortableObservableCollection<T> : ThreadSafeObservableCollection<T>
    {
        public Func<T, object>? SortSelector { get; set; }

        public ListSortDirection SortDirection { get; set; }

        public SortableObservableCollection() { }

        public SortableObservableCollection(List<T> list) : base(list) { }

        public SortableObservableCollection(IEnumerable<T> collection) : base(collection) { }

        public SortableObservableCollection(Func<T, object> sortSelector, ListSortDirection direction = ListSortDirection.Ascending) => (SortSelector, SortDirection) = (sortSelector, direction);

        [SuppressPropertyChangedWarnings]
        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e) => base.OnCollectionChanged(e);

        protected override void InvokeNotifyCollectionChanged(NotifyCollectionChangedEventHandler notifyEventHandler, NotifyCollectionChangedEventArgs e)
        {
            base.InvokeNotifyCollectionChanged(notifyEventHandler, e);

            if (SortSelector is null
                || e.Action == NotifyCollectionChangedAction.Remove
                || e.Action == NotifyCollectionChangedAction.Reset)
                return;

            Sort();
        }

        public void Sort()
        {
            if (SortSelector is null) return;

            ExecuteThreadSafe(() =>
            {
                var query = this.Select((x, index) => (Item: x, Index: index));

                query = SortDirection == ListSortDirection.Ascending ? query.OrderBy(x => SortSelector.Invoke(x.Item)) : query.OrderByDescending(x => SortSelector.Invoke(x.Item));

                var map = query.Select((x, index) => (OldIndex: x.Index, NewIndex: index)).Where(o => o.OldIndex != o.NewIndex);

                using var enumerator = map.GetEnumerator();
                if (enumerator.MoveNext())
                    Move(enumerator.Current.OldIndex, enumerator.Current.NewIndex);
            });
        }
    }
}
