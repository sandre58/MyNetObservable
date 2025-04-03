// -----------------------------------------------------------------------
// <copyright file="RangeStatistics.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using DynamicData;
using DynamicData.Binding;

namespace MyNet.Observable.Statistics;

public class RangeStatistics<T> : ObservableObject
    where T : notnull
{
    public RangeStatistics() { }

    public RangeStatistics(ReadOnlyObservableCollection<T> list, Func<T, bool> filterPredicate, Func<T, double> valuePredicate, Func<T, IObservable<object>>? reevaluator = null)
    {
        var obs = list.ToObservableChangeSet();

        if (reevaluator is not null)
            obs = obs.AutoRefreshOnObservable(reevaluator);

        Disposables.Add(obs.Subscribe(_ => Update(list, filterPredicate, valuePredicate)));
    }

    public RangeStatistics(ObservableCollection<T> list, Func<T, bool> filterPredicate, Func<T, double> valuePredicate, Func<T, IObservable<object>>? reevaluator = null)
    {
        var obs = list.ToObservableChangeSet();

        if (reevaluator is not null)
            obs = obs.AutoRefreshOnObservable(reevaluator);

        Disposables.Add(obs.Subscribe(_ => Update(list, filterPredicate, valuePredicate)));
    }

    public void Update(IEnumerable<T> list, Func<T, bool> filterPredicate, Func<T, double> valuePredicate)
    {
        var items = list.Where(filterPredicate).ToList();
        Average = items.Count != 0 ? items.Average(valuePredicate) : double.NaN;
        Sum = items.Count != 0 ? items.Sum(valuePredicate) : 0;
        Min = items.Count != 0 ? items.Min(valuePredicate) : double.NaN;
        Max = items.Count != 0 ? items.Max(valuePredicate) : double.NaN;
    }

    public double Average { get; private set; }

    public double Sum { get; private set; }

    public double Min { get; private set; }

    public double Max { get; private set; }
}
