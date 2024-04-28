// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using MyNet.Utilities.Suspending;

namespace MyNet.Observable.Suspenders
{
    public static class PropertyChangedSuspender
    {
        public static Suspender Default { get; } = new();
    }
}
