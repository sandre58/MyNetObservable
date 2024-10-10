// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using MyNet.Utilities.Collections;
using MyNet.Utilities.Deferring;
using PropertyChanged;

namespace MyNet.Observable.Collections.Sorting
{
    public class SortingPropertiesCollection : OptimizedObservableCollection<SortingProperty>
    {
        private readonly Deferrer _sortChangedDeferrer;

        public event EventHandler? SortChanged;

        public SortingPropertiesCollection() => _sortChangedDeferrer = new Deferrer(OnSortChanged);

        [SuppressPropertyChangedWarnings]
        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnCollectionChanged(e);
            _sortChangedDeferrer.DeferOrExecute();
        }

        public void Set(IEnumerable<SortingProperty> properties)
        {
            using (_sortChangedDeferrer.Defer())
            {
                Clear();
                AddRange(properties);
            }
        }

        public new SortingPropertiesCollection AddRange(IEnumerable<SortingProperty> sort)
        {
            using (_sortChangedDeferrer.Defer())
                base.AddRange(sort);

            return this;
        }

        public SortingPropertiesCollection Add(string propertyName, ListSortDirection sortDirection = ListSortDirection.Ascending)
        {
            Add(new SortingProperty(propertyName, sortDirection));

            return this;
        }

        public SortingPropertiesCollection Ascending(string propertyName) => Add(propertyName);

        public SortingPropertiesCollection Descending(string propertyName) => Add(propertyName);

        public SortingPropertiesCollection AscendingRange(IEnumerable<string> propertyNames)
        {
            using (_sortChangedDeferrer.Defer())
                propertyNames.ToList().ForEach(x => Ascending(x));

            return this;
        }

        public SortingPropertiesCollection DescendingRange(IEnumerable<string> propertyNames)
        {
            using (_sortChangedDeferrer.Defer())
                propertyNames.ToList().ForEach(x => Descending(x));

            return this;
        }

        [SuppressPropertyChangedWarnings]
        public void OnSortChanged() => SortChanged?.Invoke(this, EventArgs.Empty);
    }
}
