// -----------------------------------------------------------------------
// <copyright file="SortingComparer.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MyNet.Utilities.Comparers;

namespace MyNet.Observable.Collections.Sorting;

[Serializable]
public class SortingComparer<T>(SortingPropertiesCollection sortCollection) : IComparer, IComparer<T>
{
    public int Compare(T? x, T? y) => new ReflectionComparer<T>([.. sortCollection.Select(z => new ReflectionSortDescription(z.PropertyName, z.Direction))]).Compare(x, y);

    public int Compare(object? x, object? y) => new ReflectionComparer<T>([.. sortCollection.Select(z => new ReflectionSortDescription(z.PropertyName, z.Direction))]).Compare(x, y);
}
