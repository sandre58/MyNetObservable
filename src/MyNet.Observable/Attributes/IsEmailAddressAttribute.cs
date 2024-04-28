// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;
using System.ComponentModel.DataAnnotations;
using MyNet.Observable.Resources;
using MyNet.Utilities.Extensions;

namespace MyNet.Observable.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class IsEmailAddressAttribute : DataTypeAttribute
    {
        private readonly bool _allowEmptyValue;

        public IsEmailAddressAttribute(bool allowEmptyValue = false)
            : base(DataType.EmailAddress)
        {
            _allowEmptyValue = allowEmptyValue;

            ErrorMessageResourceName = nameof(ValidationResources.FieldXMustBeValidEmailAddressError);
            ErrorMessageResourceType = typeof(ValidationResources);
        }

        public override bool IsValid(object? value)
            => value == null || _allowEmptyValue && string.IsNullOrEmpty(value.ToString()) || (value.ToString()?.IsEmailAddress() ?? false);
    }
}
