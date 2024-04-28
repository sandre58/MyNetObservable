// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;
using System.ComponentModel.DataAnnotations;
using MyNet.Observable.Resources;

namespace MyNet.Observable.Attributes
{
    /// <summary>
    /// Indicates that the specified property must be validate in same time this property.
    /// </summary>
    public sealed class IsRequiredAttribute : RequiredAttribute
    {
        #region Constructors

        /// <summary>
        /// Initialise a new instance of <see cref="IsRequiredAttribute"/>
        /// </summary>
        /// <param name="propertyName"></param>
        public IsRequiredAttribute()
        {
            ErrorMessageResourceName = nameof(ValidationResources.FieldXIsRequiredError);
            ErrorMessageResourceType = typeof(ValidationResources);
        }

        #endregion Constructors

        public override bool IsValid(object? value) => value switch
        {
            TimeSpan ts when ts == TimeSpan.MinValue || ts == TimeSpan.MaxValue => false,
            DateTime dt when dt == DateTime.MinValue || dt == DateTime.MaxValue => false,
            _ => base.IsValid(value)
        };
    }
}
