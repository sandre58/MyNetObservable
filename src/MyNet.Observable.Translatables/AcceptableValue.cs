// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;
using MyNet.Observable.Attributes;
using MyNet.Observable.Resources;
using MyNet.Utilities;
using MyNet.Utilities.Extensions;
using MyNet.Utilities.Sequences;

namespace MyNet.Observable.Translatables
{
    public class AcceptableValue<T> : EditableObject, IAcceptableValue<T>
        where T : struct, IComparable<T>, IComparable, IConvertible, IEquatable<T>
    {
        private AcceptableValueRange<T> _acceptableRange;

        public T? Value { get; set; }

        public T? DefaultValue { get; }

        public bool HasValue => Value.HasValue;

        [ValidateProperty(nameof(Value))]
        public T? Min
        {
            get => _acceptableRange.Min;
            set
            {
                if (!Equals(value, Min))
                {
                    _acceptableRange = new(value, Max);
                    RaisePropertyChanged(nameof(Min));
                }
            }
        }

        [ValidateProperty(nameof(Value))]
        public T? Max
        {
            get => _acceptableRange.Max;
            set
            {
                if (!Equals(value, Max))
                {
                    _acceptableRange = new(Min, value);
                    RaisePropertyChanged(nameof(Max));
                }
            }
        }

        T IProvideValue<T>.Value => Value.GetValueOrDefault();

        T IResetable<T>.DefaultValue => DefaultValue.GetValueOrDefault();

        public AcceptableValue() : this(new(null, null)) { }

        public AcceptableValue(T? min, T? max) : this(new(min, max)) { }

        public AcceptableValue(AcceptableValueRange<T> acceptableValueRange, T? defaultValue = default)
        {
            _acceptableRange = acceptableValueRange;
            DefaultValue = defaultValue;

            ValidationRules.Add<IAcceptableValue<T>, T?>(x => Value, () =>
            {
                if (Min.HasValue && Max.HasValue)
                    return ValidationResources.FieldXMustBeBetweenYAndZError.FormatWith(nameof(Value).Translate()!, Min.Value, Max.Value);
                else if (Min.HasValue)
                    return ValidationResources.FieldXMustBeUpperOrEqualsThanYError.FormatWith(nameof(Value).Translate()!, Min.Value);
                else if (Max.HasValue)
                    return ValidationResources.FieldXMustBeLowerOrEqualsThanYError.FormatWith(nameof(Value).Translate()!, Max.Value);

                return string.Empty;
            }, _acceptableRange.IsValid);
            DefaultValue = defaultValue;
        }

        public void Reset() => Value = DefaultValue;

        public override string? ToString() => Value?.ToString();

        public TypeCode GetTypeCode() => Value?.GetTypeCode() ?? TypeCode.Empty;
        public bool ToBoolean(IFormatProvider? provider) => Value?.ToBoolean(provider) ?? default;
        public byte ToByte(IFormatProvider? provider) => Value?.ToByte(provider) ?? default;
        public char ToChar(IFormatProvider? provider) => Value?.ToChar(provider) ?? default;
        public DateTime ToDateTime(IFormatProvider? provider) => Value?.ToDateTime(provider) ?? default;
        public decimal ToDecimal(IFormatProvider? provider) => Value?.ToDecimal(provider) ?? default;
        public double ToDouble(IFormatProvider? provider) => Value?.ToDouble(provider) ?? default;
        public short ToInt16(IFormatProvider? provider) => Value?.ToInt16(provider) ?? default;
        public int ToInt32(IFormatProvider? provider) => Value?.ToInt32(provider) ?? default;
        public long ToInt64(IFormatProvider? provider) => Value?.ToInt64(provider) ?? default;
        public sbyte ToSByte(IFormatProvider? provider) => Value?.ToSByte(provider) ?? default;
        public float ToSingle(IFormatProvider? provider) => Value?.ToSingle(provider) ?? default;
        public string ToString(IFormatProvider? provider) => Value?.ToString(provider) ?? string.Empty;
        public object ToType(Type conversionType, IFormatProvider? provider) => Value?.ToBoolean(provider) ?? default;
        public ushort ToUInt16(IFormatProvider? provider) => Value?.ToUInt16(provider) ?? default;
        public uint ToUInt32(IFormatProvider? provider) => Value?.ToUInt32(provider) ?? default;
        public ulong ToUInt64(IFormatProvider? provider) => Value?.ToUInt64(provider) ?? default;

        public override bool Equals(object? obj) => obj is IAcceptableValue<T> val && Value.Equals(val.Value) || Value.Equals(obj);
        public virtual bool Equals(T other) => Value.Equals(other);
        public override int GetHashCode() => Value.GetHashCode();

        public int CompareTo(T? other) => Value.CompareTo(other);
        public int CompareTo(T other) => Value.CompareTo(other);
        public int CompareTo(object? obj)
            => obj switch
            {
                T obj1 => Value.CompareTo(obj1),
                IAcceptableValue<T> obj2 => Value.CompareTo(obj2.Value),
                _ => Value?.CompareTo(obj) ?? (obj is null ? 0 : 1)
            };
        public int CompareTo(IAcceptableValue<T>? other) => CompareTo(other?.Value);

        public static implicit operator T?(AcceptableValue<T> value) => value.Value;

        public bool Equals(T x, T y) => x.Equals(y);

        public int GetHashCode(T obj) => obj.GetHashCode();

        public bool Equals(IAcceptableValue<T>? x, IAcceptableValue<T>? y) => x is not null && x.Equals(y);

        public int GetHashCode(IAcceptableValue<T> obj) => obj.GetHashCode();

        public static bool operator ==(AcceptableValue<T> left, AcceptableValue<T> right) => left is null ? right is null : left.Value.Equals(right.Value);
        public static bool operator !=(AcceptableValue<T> left, AcceptableValue<T> right) => !(left == right);
        public static bool operator <(AcceptableValue<T> left, AcceptableValue<T> right) => left is null ? right is not null : left.Value.CompareTo(right.Value) < 0;
        public static bool operator <=(AcceptableValue<T> left, AcceptableValue<T> right) => left is null || left.Value.CompareTo(right.Value) <= 0;
        public static bool operator >(AcceptableValue<T> left, AcceptableValue<T> right) => left is not null && left.Value.CompareTo(right.Value) > 0;
        public static bool operator >=(AcceptableValue<T> left, AcceptableValue<T> right) => left is null ? right is null : left.Value.CompareTo(right.Value) >= 0;
    }
}
