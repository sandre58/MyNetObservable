// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using MyNet.Utilities.Comparaison;

namespace MyNet.Observable.Collections.Filters
{
    public class CompositeFilter
    {
        public CompositeFilter(IFilter filter, LogicalOperator @operator = LogicalOperator.And)
        {
            Operator = @operator;
            Filter = filter;
        }

        public LogicalOperator Operator { get; }

        public IFilter Filter { get; }
    }
}
