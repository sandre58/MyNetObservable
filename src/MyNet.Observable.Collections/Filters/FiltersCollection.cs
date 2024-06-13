// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using MyNet.Utilities.Comparaison;
using MyNet.Utilities.Deferring;
using PropertyChanged;

namespace MyNet.Observable.Collections.Filters
{
    public class FiltersCollection : ObservableCollection<CompositeFilter>
    {
        private readonly Deferrer _filtersChangedDeferrer;

        public event EventHandler? FiltersChanged;

        public FiltersCollection() => _filtersChangedDeferrer = new Deferrer(OnFiltersChanged);

        [SuppressPropertyChangedWarnings]
        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnCollectionChanged(e);
            _filtersChangedDeferrer.DeferOrExecute();
        }

        public FiltersCollection And(IFilter filter)
        {
            Add(new CompositeFilter(filter, LogicalOperator.And));

            return this;
        }

        public FiltersCollection Or(IFilter filter)
        {
            Add(new CompositeFilter(filter, LogicalOperator.Or));

            return this;
        }

        public void Set(IEnumerable<CompositeFilter> filters)
        {
            using (_filtersChangedDeferrer.Defer())
            {
                Clear();
                AddRange(filters);
            }
        }

        public FiltersCollection AddRange(IEnumerable<CompositeFilter> filters)
        {
            using (_filtersChangedDeferrer.Defer())
                filters.ToList().ForEach(Add);

            return this;
        }

        public FiltersCollection AndRange(IEnumerable<IFilter> filters)
        {
            using (_filtersChangedDeferrer.Defer())
                filters.ToList().ForEach(x => And(x));

            return this;
        }

        public FiltersCollection OrRange(IEnumerable<IFilter> filters)
        {
            using (_filtersChangedDeferrer.Defer())
                filters.ToList().ForEach(x => Or(x));

            return this;
        }

        [SuppressPropertyChangedWarnings]
        private void OnFiltersChanged() => FiltersChanged?.Invoke(this, EventArgs.Empty);
    }
}
