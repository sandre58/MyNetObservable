﻿// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using DynamicData;
using DynamicData.Binding;
using MyNet.Utilities;

namespace MyNet.Observable.Statistics
{
    public class TimeSpanRangeStatistics<T> : ObservableObject where T : notnull
    {
        public TimeSpanRangeStatistics() { }

        public TimeSpanRangeStatistics(ObservableCollection<T> list, Func<T, bool> filterPredicate, Func<T, TimeSpan> valuePredicate, Func<T, IObservable<object>>? reevaluator = null)
        {
            var obs = list.ToObservableChangeSet();

            if (reevaluator is not null)
                obs = obs.AutoRefreshOnObservable(reevaluator);

            Disposables.Add(obs.Subscribe(_ => Update(list, filterPredicate, valuePredicate)));
        }

        public void Update(IEnumerable<T> list, Func<T, bool> filterPredicate, Func<T, TimeSpan> valuePredicate)
        {
            var items = list.Where(filterPredicate).ToList();
            Average = items.Count != 0 ? new TimeSpan((long)items.Average(x => valuePredicate.Invoke(x).Ticks)) : TimeSpan.Zero;
            Sum = items.Count != 0 ? new TimeSpan(items.Sum(x => valuePredicate.Invoke(x).Ticks)) : TimeSpan.Zero;
            Min = items.Count != 0 ? new TimeSpan(items.Min(x => valuePredicate.Invoke(x).Ticks)) : TimeSpan.Zero;
            Max = items.Count != 0 ? new TimeSpan(items.Max(x => valuePredicate.Invoke(x).Ticks)) : TimeSpan.Zero;
        }

        public TimeSpan Average { get; private set; }

        public TimeSpan Sum { get; private set; }

        public TimeSpan Min { get; private set; }

        public TimeSpan Max { get; private set; }
    }
}
