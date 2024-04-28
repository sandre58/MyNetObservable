// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;

namespace MyNet.Observable
{
    public interface IDisplayValueWithUnit
    {
        string? SimplifyToString(Enum? minUnit = default, Enum? maxUnit = default, bool abbreviation = true, string? format = null);

        string? ToString(bool abbreviation, string? format = null);
    }

    public interface IAcceptableValueWithUnit<T, TUnit> : IAcceptableValue<T>
    where T : struct, IComparable<T>, IComparable, IConvertible, IEquatable<T>
    where TUnit : Enum
    {
        TUnit Unit { get; }

        double? Convert(TUnit unit);

        IAcceptableValueWithUnit<double, TUnit> Simplify(TUnit? minUnit = default, TUnit? maxUnit = default);
    }
}
