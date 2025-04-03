// -----------------------------------------------------------------------
// <copyright file="DisplayWrapper.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace MyNet.Observable.Translatables;

public class DisplayWrapper<T>(T item, IProvideValue<string> displayName) : Wrapper<T>(item)
{
    public IProvideValue<string> DisplayName { get; set; } = displayName;

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope", Justification = "No dissposable")]
    public DisplayWrapper(T item, string resourceKey)
        : this(item, new StringTranslatable(resourceKey)) { }

    protected override Wrapper<T> CreateCloneInstance(T item) => new DisplayWrapper<T>(item, DisplayName);

    public override string? ToString() => DisplayName.Value;
}
