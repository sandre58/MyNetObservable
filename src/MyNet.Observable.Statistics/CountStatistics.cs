// -----------------------------------------------------------------------
// <copyright file="CountStatistics.cs" company="Stéphane ANDRE">
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

public class CountStatistics<T> : ObservableObject
    where T : notnull
{
    public CountStatistics() { }

    public CountStatistics(ReadOnlyObservableCollection<T> list, Func<T, bool> filterPredicate, Func<T, IObservable<object>>? reevaluator = null)
    {
        var obs = list.ToObservableChangeSet();

        if (reevaluator is not null)
            obs = obs.AutoRefreshOnObservable(reevaluator);

        Disposables.Add(obs.Subscribe(_ => Update(list, filterPredicate)));
    }

    public CountStatistics(ObservableCollection<T> list, Func<T, bool> filterPredicate, Func<T, IObservable<object>>? reevaluator = null)
    {
        var obs = list.ToObservableChangeSet();

        if (reevaluator is not null)
            obs = obs.AutoRefreshOnObservable(reevaluator);

        Disposables.Add(obs.Subscribe(_ => Update(list, filterPredicate)));
    }

    public void Update(IEnumerable<T> list, Func<T, bool> filterPredicate)
    {
        var toList = list.ToList();
        var subCount = toList.Count(filterPredicate);
        Count = subCount;
        Percentage = toList.Count != 0 ? (double)subCount / toList.Count : 0;
    }

    public double Percentage { get; private set; }

    public double Count { get; private set; }
}
