// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;
using System.ComponentModel.DataAnnotations;
using MyNet.Observable.Resources;
using MyNet.Utilities;

namespace MyNet.Observable.Attributes
{
    /// <summary>
    /// Indicates that the specified property must be validate in same time this property.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class IsInPastAttribute : DataTypeAttribute
    {
        private readonly bool _allowEmptyValue;

        public IsInPastAttribute(bool allowEmptyValue = false)
            : base(DataType.DateTime)
        {
            _allowEmptyValue = allowEmptyValue;

            ErrorMessageResourceName = nameof(ValidationResources.FieldXMustBeInPastError);
            ErrorMessageResourceType = typeof(ValidationResources);
        }

        public override bool IsValid(object? value)
            => (_allowEmptyValue && (value == null || value is DateTime d && d == DateTime.MinValue)) || value is DateTime date && date.IsInPast();
    }
}
