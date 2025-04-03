// -----------------------------------------------------------------------
// <copyright file="SeverityValidationResult.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyNet.Observable.Validation;

public class SeverityValidationResult : ValidationResult
{
    public ValidationRuleSeverity Severity { get; }

    public SeverityValidationResult(string? errorMessage, ValidationRuleSeverity severity = ValidationRuleSeverity.Error)
        : base(errorMessage) => Severity = severity;

    public SeverityValidationResult(string? errorMessage, IEnumerable<string>? memberNames, ValidationRuleSeverity severity = ValidationRuleSeverity.Error)
        : base(errorMessage, memberNames) => Severity = severity;
}
