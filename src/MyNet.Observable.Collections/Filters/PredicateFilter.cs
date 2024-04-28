// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;

namespace MyNet.Observable.Collections.Filters
{
    public class PredicateFilter<T> : IFilter
    {
        private readonly Func<T?, bool> _predicate;

        public PredicateFilter(Func<T?, bool> predicate) => _predicate = predicate;

        string IFilter.PropertyName => string.Empty;

        public bool IsMatch(object? target) => _predicate.Invoke((T?)target);
    }
}
