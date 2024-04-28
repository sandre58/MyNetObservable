// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Linq;

namespace MyNet.Observable.Validation
{
    public sealed class ValidationRuleCollection : Collection<IValidationRule>
    {
        public void Add<T, TProperty>(Expression<Func<T, TProperty>> propertyAccessor, Func<string> error, Func<TProperty?, bool> rule, ValidationRuleSeverity severity = ValidationRuleSeverity.Error)
            => Add(new DelegateRule<T, TProperty>(propertyAccessor, error, rule, severity));

        public void Add<T, TProperty>(Expression<Func<T, TProperty>> propertyAccessor, string error, Func<TProperty?, bool> rule, ValidationRuleSeverity severity = ValidationRuleSeverity.Error)
            => Add(propertyAccessor, () => error, rule, severity);

        public void AddNotNull<T, TProperty>(Expression<Func<T, TProperty>> propertyAccessor, Func<string> error, Func<TProperty, bool> rule, ValidationRuleSeverity severity = ValidationRuleSeverity.Error)
            => Add(propertyAccessor, error, new Func<TProperty?, bool>(x => x is not null && rule.Invoke(x)), severity);

        public void AddNotNull<T, TProperty>(Expression<Func<T, TProperty>> propertyAccessor, string error, Func<TProperty, bool> rule, ValidationRuleSeverity severity = ValidationRuleSeverity.Error)
            => Add(propertyAccessor, () => error, new Func<TProperty?, bool>(x => x is not null && rule.Invoke(x)), severity);

        public IEnumerable<IValidationRule> Apply<T>(T item, string propertyName)
            => (from rule in this where string.IsNullOrEmpty(propertyName) || (rule.PropertyName?.Equals(propertyName, StringComparison.OrdinalIgnoreCase) ?? false) where !rule.Apply(item) select rule).ToList();
    }
}
