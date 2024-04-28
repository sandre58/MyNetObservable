// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyNet.Observable.Validation
{
    public class SeverityValidationResult : ValidationResult
    {
        public ValidationRuleSeverity Severity { get; }

        public SeverityValidationResult(string? errorMessage, ValidationRuleSeverity severity = ValidationRuleSeverity.Error) : base(errorMessage) => Severity = severity;

        public SeverityValidationResult(string? errorMessage, IEnumerable<string>? memberNames, ValidationRuleSeverity severity = ValidationRuleSeverity.Error) : base(errorMessage, memberNames) => Severity = severity;
    }
}
