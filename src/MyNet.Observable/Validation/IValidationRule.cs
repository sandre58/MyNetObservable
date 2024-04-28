// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

namespace MyNet.Observable.Validation
{
    public interface IValidationRule
    {
        string? PropertyName { get; }

        string Error { get; }

        ValidationRuleSeverity Severity { get; }

        bool Apply<T>(T item);
    }
}
