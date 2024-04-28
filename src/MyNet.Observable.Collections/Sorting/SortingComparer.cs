// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MyNet.Utilities.Comparers;

namespace MyNet.Observable.Collections.Sorting
{
    public class SortingComparer<T>(SortingPropertiesCollection sortCollection) : IComparer, IComparer<T>
    {
        private readonly SortingPropertiesCollection _sortCollection = sortCollection;

        public int Compare(T? x, T? y) => new ReflectionComparer<T>(_sortCollection.Select(x => new ReflectionSortDescription(x.PropertyName, x.Direction)).ToList()).Compare(x, y);

        public int Compare(object? x, object? y) => new ReflectionComparer<T>(_sortCollection.Select(x => new ReflectionSortDescription(x.PropertyName, x.Direction)).ToList()).Compare(x, y);
    }
}
