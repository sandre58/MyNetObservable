// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace MyNet.Observable.Collections
{
    public abstract class ObservableKeyedCollection<TKey, T> : SortableObservableCollection<T>
        where TKey : notnull
    {
        private Dictionary<TKey, T>? _dict;

        protected ObservableKeyedCollection(IEnumerable<T> list) : this(list, null) { }

        protected ObservableKeyedCollection() : this([], null) { }

        protected ObservableKeyedCollection(IEqualityComparer<TKey> comparer) : this([], comparer) { }

        protected ObservableKeyedCollection(Func<T, object> sortSelector, ListSortDirection direction = ListSortDirection.Ascending) : base(sortSelector, direction) => Comparer = EqualityComparer<TKey>.Default;

        protected ObservableKeyedCollection(IEnumerable<T> list, IEqualityComparer<TKey>? comparer) : base(list)
        {
            comparer ??= EqualityComparer<TKey>.Default;

            Comparer = comparer;
        }

        public IEqualityComparer<TKey> Comparer { get; }

        public T? this[TKey key]
            => key switch
            {
                null => throw new ArgumentNullException(nameof(key)),
                _ => _dict is not null && _dict.TryGetValue(key, out var value) ? value : Items.FirstOrDefault(x => Comparer.Equals(GetKeyForItem(x), key))
            };

        public bool Contains(TKey key)
            => key switch
            {
                null => throw new ArgumentNullException(nameof(key)),
                _ => _dict is not null ? _dict.ContainsKey(key) : Items.Any(x => Comparer.Equals(GetKeyForItem(x), key))
            };

        private bool ContainsItem(T item)
        {
            TKey key;
            if (_dict is null || (key = GetKeyForItem(item)) is null)
            {
                return Items.Contains(item);
            }

            var exist = _dict.TryGetValue(key, out var itemInDict);
            return exist && EqualityComparer<T>.Default.Equals(itemInDict, item);
        }

        public bool TryAdd(T item)
        {
            var key = GetKeyForItem(item);
            if (key is null || _dict is null || _dict.ContainsKey(key)) return false;

            Add(item);

            return true;
        }

        public bool Remove(TKey key)
        {
            if (key is null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (_dict is not null)
            {
                return _dict.ContainsKey(key) && Remove(_dict[key]);
            }

            for (var i = 0; i < Items.Count; i++)
            {
                if (Comparer.Equals(GetKeyForItem(Items[i]), key))
                {
                    RemoveItem(i);
                    return true;
                }
            }
            return false;
        }

        protected IDictionary<TKey, T>? Dictionary => _dict;

        protected void ChangeItemKey(T item, TKey newKey)
        {
            // check if the item exists in the collection
            if (!ContainsItem(item))
            {
                return;
            }

            var oldKey = GetKeyForItem(item);
            if (!Comparer.Equals(oldKey, newKey))
            {
                if (newKey is not null)
                {
                    AddKey(newKey, item);
                }

                if (oldKey is not null)
                {
                    RemoveKey(oldKey);
                }
            }
        }

        protected override void ClearItems()
        {
            _dict?.Clear();

            base.ClearItems();
        }

        protected abstract TKey GetKeyForItem(T item);

        protected override void InsertItem(int index, T item)
        {
            var key = GetKeyForItem(item);
            if (key is not null)
            {
                AddKey(key, item);
            }
            base.InsertItem(index, item);
        }

        protected void InsertItemInItems(int index, T item) => base.InsertItem(index, item);

        protected override void RemoveItem(int index)
        {
            var key = GetKeyForItem(Items[index]);
            if (key is not null)
            {
                RemoveKey(key);
            }
            base.RemoveItem(index);
        }

        protected override void SetItem(int index, T item)
        => ExecuteThreadSafe(() =>
        {
            var newKey = GetKeyForItem(item);
            var oldKey = GetKeyForItem(Items[index]);

            if (Comparer.Equals(oldKey, newKey))
            {
                if (newKey is not null && _dict is not null)
                {
                    _dict[newKey] = item;
                }
            }
            else
            {
                if (newKey is not null)
                {
                    AddKey(newKey, item);
                }

                if (oldKey is not null)
                {
                    RemoveKey(oldKey);
                }
            }
            base.SetItem(index, item);
        });

        private void AddKey(TKey key, T item)
            => ExecuteThreadSafe(() =>
            {
                if (_dict is null)
                {
                    CreateDictionary();
                }
                _dict?.Add(key, item);
            });

        private void CreateDictionary()
        {
            _dict = new Dictionary<TKey, T>(Comparer);
            foreach (var item in Items)
            {
                var key = GetKeyForItem(item);
                if (key is not null)
                {
                    _dict.Add(key, item);
                }
            }
        }

        private void RemoveKey(TKey key)
            => ExecuteThreadSafe(() =>
            {
                if (_dict is not null)
                {
                    _ = _dict.Remove(key);
                }
            });

    }
}
