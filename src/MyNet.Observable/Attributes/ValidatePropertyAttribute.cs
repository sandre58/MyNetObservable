// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;

namespace MyNet.Observable.Attributes
{
    /// <summary>
    /// Indicates that the specified property must be validate in same time this property.
    /// </summary>
    /// <remarks>
    /// Initialise a new instance of <see cref="ValidatePropertyAttribute"/>
    /// </remarks>
    /// <param name="propertyName"></param>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public sealed class ValidatePropertyAttribute(string propertyName) : Attribute
    {
        /// <summary>
        /// Gets property name.
        /// </summary>
        public string PropertyName
        {
            get;
        } = propertyName;
    }
}
