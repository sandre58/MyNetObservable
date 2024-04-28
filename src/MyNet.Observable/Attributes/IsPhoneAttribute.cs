// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;
using System.ComponentModel.DataAnnotations;
using MyNet.Observable.Resources;
using MyNet.Utilities.Extensions;

namespace MyNet.Observable.Attributes
{
    /// <summary>
    /// Indicates that the specified property must be validate in same time this property.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class IsPhoneAttribute : DataTypeAttribute
    {
        private readonly bool _allowEmptyValue;

        public IsPhoneAttribute(bool allowEmptyValue = false)
            : base(DataType.PhoneNumber)
        {
            _allowEmptyValue = allowEmptyValue;

            ErrorMessageResourceName = nameof(ValidationResources.FieldXMustBeValidPhoneNumberError);
            ErrorMessageResourceType = typeof(ValidationResources);
        }

        public override bool IsValid(object? value)
            => value == null || _allowEmptyValue && string.IsNullOrEmpty(value.ToString()) || (value.ToString()?.IsPhoneNumber() ?? false);
    }
}
