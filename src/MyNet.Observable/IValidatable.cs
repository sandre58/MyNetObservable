// -----------------------------------------------------------------------
// <copyright file="IValidatable.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using MyNet.Observable.Validation;

namespace MyNet.Observable;

public interface IValidatable
{
    ValidationRuleCollection ValidationRules { get; }

    bool ValidateProperties();

    IEnumerable<string> GetValidationErrors();
}
