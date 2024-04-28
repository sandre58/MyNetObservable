// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;
using System.ComponentModel;
using MyNet.Utilities;

namespace MyNet.Observable
{
    public class EditableWrapper<T> : EditableObject, ICloneable, ISettable, IIdentifiable<Guid>, IWrapper<T>
    {
        public Guid Id { get; } = Guid.NewGuid();

        public T Item { get; protected set; }

        public EditableWrapper(T item) => Item = item;

        protected virtual void OnItemChanged()
        {
            if (Item is INotifyPropertyChanged notifyPropertyChanged)
            {
                notifyPropertyChanged.PropertyChanged += Item_PropertyChanged;
            }
        }

        protected virtual void OnItemChanging()
        {
            if (Item is INotifyPropertyChanged notifyPropertyChanged)
            {
                notifyPropertyChanged.PropertyChanged -= Item_PropertyChanged;
            }

            if (Item is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }

        private void Item_PropertyChanged(object? sender, PropertyChangedEventArgs e) => RaisePropertyChanged(e.PropertyName);

        public virtual object Clone()
        {
            var item = Item is ICloneable clonable ? (T)clonable.Clone() : Item;
            return CreateCloneInstance(item);
        }

        protected virtual EditableWrapper<T> CreateCloneInstance(T item) => new(item);

        public void SetFrom(object? from)
        {
            if (Item is ISettable settable)
            {
                if (from is T newItem)
                {
                    settable.SetFrom(newItem);
                }
                else if (from is EditableWrapper<T> newWrapper)
                {
                    settable.SetFrom(newWrapper.Item);
                }
            }
            else
            {
                if (from is T newItem)
                {
                    Item?.DeepSet(newItem);
                }
                else if (from is EditableWrapper<T> newWrapper)
                {
                    Item?.DeepSet(newWrapper.Item);
                }
            }
        }

        public override bool Equals(object? obj) => obj != null && Equals(GetType(), obj.GetType()) && ReferenceEquals(Item, ((EditableWrapper<T>)obj).Item);

        public override int GetHashCode() => Item?.GetHashCode() ?? 0;

        protected override void Cleanup()
        {
            if (Item is INotifyPropertyChanged notifyPropertyChanged)
            {
                notifyPropertyChanged.PropertyChanged -= Item_PropertyChanged;
            }

            base.Cleanup();
        }

    }
}
