﻿// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;
using MyNet.Humanizer;
using MyNet.Utilities;
using MyNet.Utilities.Sequences;
using MyNet.Utilities.Units;

namespace MyNet.Observable.Translatables
{
    public class UnitValue<T, TUnit> : AcceptableValue<T>, IAcceptableValueWithUnit<T, TUnit>, IDisplayValueWithUnit
    where T : struct, IComparable<T>, IComparable, IConvertible, IEquatable<T>
    where TUnit : Enum
    {
        public TUnit Unit { get; }

        T IProvideValue<T>.Value => Value.GetValueOrDefault();

        public UnitValue(TUnit unit) : base() => Unit = unit;

        public UnitValue(TUnit unit, T? min, T? max) : base(min, max) => Unit = unit;

        public UnitValue(TUnit unit, AcceptableValueRange<T> acceptableValueRange) : base(acceptableValueRange) => Unit = unit;

        public virtual double? Convert(TUnit unit) => Value?.To(Unit, unit);

        public virtual IAcceptableValueWithUnit<double, TUnit> Simplify(TUnit? minUnit = default, TUnit? maxUnit = default)
        {
            var (newValue, newUnit) = Value.HasValue ? Value.Value.Simplify(Unit, minUnit, maxUnit) : ((double?)null, Unit);
            return new UnitValue<double, TUnit>(newUnit) { Value = newValue };
        }

        public virtual string? SimplifyToString(Enum? minUnit = default, Enum? maxUnit = default, bool abbreviation = true, string? format = null)
            => Value?.Humanize(Unit, minUnit, maxUnit, abbreviation, format);

        public virtual string? ToString(bool abbreviation, string? format = null)
            => Value?.Humanize(Unit, abbreviation, format);

        public override string? ToString() => ToString(true);
    }

    public class FileSize<T> : UnitValue<T, FileSizeUnit>
        where T : struct, IComparable<T>, IComparable, IConvertible, IEquatable<T>
    {
        public FileSize(FileSizeUnit unit = FileSizeUnit.Byte) : base(unit) { }
    }

    public class Metric<T> : UnitValue<T, MetricUnit>
        where T : struct, IComparable<T>, IComparable, IConvertible, IEquatable<T>
    {
        public Metric(MetricUnit unit = MetricUnit.None) : base(unit) { }

        public Metric(T? min, T? max, MetricUnit unit = MetricUnit.None) : base(unit, min, max) { }

        public Metric(AcceptableValueRange<T> acceptableValueRange, MetricUnit unit = MetricUnit.None) : base(unit, acceptableValueRange) { }
    }

    public class Length<T> : UnitValue<T, LengthUnit>
        where T : struct, IComparable<T>, IComparable, IConvertible, IEquatable<T>
    {
        public Length(LengthUnit unit = LengthUnit.Meter) : base(unit) { }

        public Length(T? min, T? max, LengthUnit unit = LengthUnit.Meter) : base(unit, min, max) { }

        public Length(AcceptableValueRange<T> acceptableValueRange, LengthUnit unit = LengthUnit.Meter) : base(unit, acceptableValueRange) { }
    }

    public class Mass<T> : UnitValue<T, MassUnit>
        where T : struct, IComparable<T>, IComparable, IConvertible, IEquatable<T>
    {
        public Mass(MassUnit unit = MassUnit.Gram) : base(unit) { }

        public Mass(T? min, T? max, MassUnit unit = MassUnit.Gram) : base(unit, min, max) { }

        public Mass(AcceptableValueRange<T> acceptableValueRange, MassUnit unit = MassUnit.Gram) : base(unit, acceptableValueRange) { }
    }
}
