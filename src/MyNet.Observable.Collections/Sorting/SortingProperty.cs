// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System.ComponentModel;

namespace MyNet.Observable.Collections.Sorting
{
    public class SortingProperty
    {
        public SortingProperty(string propertyName, ListSortDirection direction = ListSortDirection.Ascending)
        {
            PropertyName = propertyName;
            Direction = direction;
        }

        public string PropertyName { get; }

        public ListSortDirection Direction { get; }
    }
}
