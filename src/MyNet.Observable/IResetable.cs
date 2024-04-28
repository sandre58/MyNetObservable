// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

namespace MyNet.Observable
{
    public interface IResetable<out T>
    {
        T? DefaultValue { get; }

        void Reset();
    }
}
