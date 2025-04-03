// -----------------------------------------------------------------------
// <copyright file="SortingProperty.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.ComponentModel;

namespace MyNet.Observable.Collections.Sorting;

public class SortingProperty(string propertyName, ListSortDirection direction = ListSortDirection.Ascending)
{
    public string PropertyName { get; } = propertyName;

    public ListSortDirection Direction { get; } = direction;
}
