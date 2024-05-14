// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq.Expressions;

namespace MyNet.Observable.Validation
{
    /// <summary>
    /// A named rule containing an error to be used if the rule fails.
    /// </summary>
    public abstract class ValidationRule<TObject, TProperty> : IValidationRule
    {
        private readonly Func<string> _error;

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Rule"/> class.
        /// </summary>
        /// <param name="propertyAccessor">The name of the property this instance applies to.</param>
        /// <param name="error">The error message if the rules fails.</param>
        /// <param name="severity"></param>
        protected ValidationRule(Expression<Func<TObject, TProperty>> propertyAccessor, Func<string> error, ValidationRuleSeverity severity = ValidationRuleSeverity.Error)
        {
            Severity = severity;
            PropertyExpression = propertyAccessor;
            PropertyName = (propertyAccessor.Body as MemberExpression ?? ((UnaryExpression)propertyAccessor.Body).Operand as MemberExpression)?.Member.Name;
            _error = error ?? throw new ArgumentNullException(nameof(error));
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets the name of the property this instance applies to.
        /// </summary>
        /// <value>The name of the property this instance applies to.</value>
        public string? PropertyName { get; }

        /// <summary>
        /// Gets the name of the property this instance applies to.
        /// </summary>
        /// <value>The name of the property this instance applies to.</value>
        protected Expression<Func<TObject, TProperty>> PropertyExpression { get; }

        /// <summary>
        /// Gets the error message if the rules fails.
        /// </summary>
        /// <value>The error message if the rules fails.</value>
        public string Error => _error.Invoke();

        public ValidationRuleSeverity Severity { get; }

        #endregion Properties

        #region Apply

        /// <summary>
        /// Applies the rule to the specified object.
        /// </summary>
        /// <param name="item">The object to apply the rule to.</param>
        /// <returns>
        /// <c>true</c> if the object satisfies the rule, otherwise <c>false</c>.
        /// </returns>
        protected abstract bool ApplyOnProperty(TProperty item);

        /// <summary>
        /// Applies the rule to the specified object.
        /// </summary>
        /// <param name="item">The object to apply the rule to.</param>
        /// <returns>
        /// <c>true</c> if the object satisfies the rule, otherwise <c>false</c>.
        /// </returns>
        public bool Apply(TObject item) => ApplyOnProperty(PropertyExpression.Compile().Invoke(item));

        bool IValidationRule.Apply<T>(T item) => item is TObject obj && Apply(obj);

        #endregion Apply
    }
}
