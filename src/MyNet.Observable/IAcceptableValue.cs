// -----------------------------------------------------------------------
// <copyright file="IAcceptableValue.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace MyNet.Observable;

public interface IAcceptableValue<T> : IProvideValue<T>, IResetable<T>, IComparable<IAcceptableValue<T>>, IComparable<T>, IComparable, IEquatable<T>, IConvertible, IEqualityComparer<T>, IEqualityComparer<IAcceptableValue<T>>
    where T : struct, IComparable<T>, IComparable, IConvertible, IEquatable<T>
{
    new T? Value { get; set; }

    T? Min { get; set; }

    T? Max { get; set; }
}
