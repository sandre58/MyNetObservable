// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

namespace MyNet.Observable.Translatables
{
    public class DisplayWrapper<T> : Wrapper<T>
    {
        public IProvideValue<string> DisplayName { get; set; }

        public DisplayWrapper(T item, IProvideValue<string> displayName) : base(item) => DisplayName = displayName;

        public DisplayWrapper(T item, string resourceKey) : this(item, new TranslatableString(resourceKey)) { }

        protected override Wrapper<T> CreateCloneInstance(T item) => new DisplayWrapper<T>(item, DisplayName);

        public override string? ToString() => DisplayName.Value;
    }
}
