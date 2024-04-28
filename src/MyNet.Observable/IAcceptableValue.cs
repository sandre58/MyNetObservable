// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;

namespace MyNet.Observable
{
    public interface IAcceptableValue<T> : IProvideValue<T>, IResetable<T>, IComparable<IAcceptableValue<T>>, IComparable<T>, IComparable, IEquatable<T>, IConvertible, IEqualityComparer<T>, IEqualityComparer<IAcceptableValue<T>>
        where T : struct, IComparable<T>, IComparable, IConvertible, IEquatable<T>
    {
        new T? Value { get; set; }

        T? Min { get; set; }

        T? Max { get; set; }
    }
}
