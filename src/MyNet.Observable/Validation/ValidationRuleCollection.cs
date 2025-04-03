// -----------------------------------------------------------------------
// <copyright file="ValidationRuleCollection.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;

namespace MyNet.Observable.Validation;

public sealed class ValidationRuleCollection : Collection<IValidationRule>
{
    public void Add<T, TProperty>(Expression<Func<T, TProperty>> propertyAccessor, Func<string> error, Func<TProperty?, bool> rule, ValidationRuleSeverity severity = ValidationRuleSeverity.Error)
        => Add(new DelegateRule<T, TProperty>(propertyAccessor, error, rule, severity));

    public void Add<T, TProperty>(Expression<Func<T, TProperty>> propertyAccessor, string error, Func<TProperty?, bool> rule, ValidationRuleSeverity severity = ValidationRuleSeverity.Error)
        => Add(propertyAccessor, () => error, rule, severity);

    public void AddNotNull<T, TProperty>(Expression<Func<T, TProperty>> propertyAccessor, Func<string> error, Func<TProperty, bool> rule, ValidationRuleSeverity severity = ValidationRuleSeverity.Error)
        => Add(propertyAccessor, error, x => x is not null && rule.Invoke(x), severity);

    public void AddNotNull<T, TProperty>(Expression<Func<T, TProperty>> propertyAccessor, string error, Func<TProperty, bool> rule, ValidationRuleSeverity severity = ValidationRuleSeverity.Error)
        => Add(propertyAccessor, () => error, x => x is not null && rule.Invoke(x), severity);

    public IEnumerable<IValidationRule> Apply<T>(T item, string propertyName)
        => [.. from rule in this where string.IsNullOrEmpty(propertyName) || (rule.PropertyName?.Equals(propertyName, StringComparison.OrdinalIgnoreCase) ?? false) where !rule.Apply(item) select rule];
}
