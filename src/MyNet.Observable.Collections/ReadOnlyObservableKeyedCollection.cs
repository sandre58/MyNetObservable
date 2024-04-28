// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System.Collections.ObjectModel;

namespace MyNet.Observable.Collections
{
    public class ReadOnlyObservableKeyedCollection<TKey, T>(ObservableKeyedCollection<TKey, T> list) : ReadOnlyObservableCollection<T>(list)
        where TKey : notnull
    {
        public T? this[TKey key] => ((ObservableKeyedCollection<TKey, T>)Items)[key];
    }
}
