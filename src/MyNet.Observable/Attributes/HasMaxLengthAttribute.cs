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
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public sealed class HasMaxLengthAttribute : MaxLengthAttribute
    {
        #region Constructors

        /// <summary>
        /// Initialise a new instance of <see cref="HasMaxLengthAttribute"/>
        /// </summary>
        public HasMaxLengthAttribute(int length) : base(length)
        {
            ErrorMessageResourceName = nameof(ValidationResources.FieldXMustHaveMaxLengthYError);
            ErrorMessageResourceType = typeof(ValidationResources);
        }

        #endregion Constructors
    }
}
