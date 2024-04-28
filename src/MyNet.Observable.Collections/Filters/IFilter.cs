// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

namespace MyNet.Observable.Collections.Filters
{
    public interface IFilter
    {
        string PropertyName { get; }

        bool IsMatch(object? target);
    }
}
