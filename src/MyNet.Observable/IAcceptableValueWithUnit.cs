// -----------------------------------------------------------------------
// <copyright file="IAcceptableValueWithUnit.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;

namespace MyNet.Observable;

public interface IAcceptableValueWithUnit<T, TUnit> : IAcceptableValue<T>
where T : struct, IComparable<T>, IComparable, IConvertible, IEquatable<T>
where TUnit : Enum
{
    TUnit Unit { get; }

    double? Convert(TUnit unit);

    IAcceptableValueWithUnit<double, TUnit> Simplify(TUnit? minUnit = default, TUnit? maxUnit = default);
}
