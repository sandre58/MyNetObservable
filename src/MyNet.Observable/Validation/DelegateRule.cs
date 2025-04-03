// -----------------------------------------------------------------------
// <copyright file="DelegateRule.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Linq.Expressions;

namespace MyNet.Observable.Validation;

/// <summary>
/// Determines whether or not an object satisfies a rule and
/// provides an error if it does not.
/// </summary>
public sealed class DelegateRule<TObject, TProperty> : ValidationRule<TObject, TProperty>
{
    private readonly Func<TProperty?, bool> _rule;

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="DelegateRule{TObject, TProperty}"/> class.
    /// </summary>
    /// <param name="propertyAccessor">>The name of the property the rules applies to.</param>
    /// <param name="error">The error if the rules fails.</param>
    /// <param name="rule">The rule to execute.</param>
    /// <param name="severity">Severity of rule.</param>
    public DelegateRule(Expression<Func<TObject, TProperty>> propertyAccessor, Func<string> error, Func<TProperty?, bool> rule, ValidationRuleSeverity severity = ValidationRuleSeverity.Error)
        : base(propertyAccessor, error, severity) => _rule = rule ?? throw new ArgumentNullException(nameof(rule));

    public DelegateRule(Expression<Func<TObject, TProperty>> propertyAccessor, string error, Func<TProperty?, bool> rule, ValidationRuleSeverity severity = ValidationRuleSeverity.Error)
        : base(propertyAccessor, () => error, severity) => _rule = rule ?? throw new ArgumentNullException(nameof(rule));

    #endregion Constructors

    #region Rule<T> Members

    /// <inheritdoc />
    /// <summary>
    /// Applies the rule to the specified object.
    /// </summary>
    /// <param name="item">The object to apply the rule to.</param>
    /// <returns>
    /// <c>true</c> if the object satisfies the rule, otherwise <c>false</c>.
    /// </returns>
    protected override bool ApplyOnProperty(TProperty item) => _rule.Invoke(item);

    #endregion Rule<T> Members
}
