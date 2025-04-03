// -----------------------------------------------------------------------
// <copyright file="IValidationRule.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace MyNet.Observable.Validation;

public interface IValidationRule
{
    string? PropertyName { get; }

    string Error { get; }

    ValidationRuleSeverity Severity { get; }

    bool Apply<T>(T item);
}
