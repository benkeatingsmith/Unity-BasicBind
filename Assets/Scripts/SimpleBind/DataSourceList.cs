using System;
using System.Collections;
using System.Collections.Generic;

namespace SimpleBind
{
    public interface IDataSourceList : IDataSource, IEnumerable
    {
        event EventHandler<CollectionChangedEventArgs> CollectionChanged;

		Type ElementType { get; }
        
        #if UNITY_EDITOR
        void NotifyChanges(IList<object> oldValues);
        #endif
    }
    
    [Serializable]
	public class DataSourceList<TElement> : DataSource<List<TElement>>, IList<TElement>, IDataSourceList
	{
        public int Count => value.Count;
        public bool IsReadOnly => false;

        public Type ElementType => typeof(TElement);

        public event EventHandler<CollectionChangedEventArgs> CollectionChanged;
        
        public IEnumerator<TElement> GetEnumerator() => value.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => value.GetEnumerator();
        
        public DataSourceList() : this(new List<TElement>())
        {
        }

        public DataSourceList(List<TElement> value)
        {
            this.value = value;
        }
        
        public TElement this[int key]
        {
            get => value[key];
            set
            {
                if (EqualityComparer<TElement>.Default.Equals(this.value[key], value)) return;
                this.value[key] = value;
                InvokeChanged(CollectionChangedEventArgs.ItemChanged(value, key));
            }
        }

        public int IndexOf(TElement item)
        {
            return value.IndexOf(item);
        }

        public void Add(TElement item)
        {
            value.Add(item);
            InvokeChanged(CollectionChangedEventArgs.ItemChanged(item, value.Count - 1));
        }

        public void Insert(int index, TElement item)
        {
            value.Insert(index, item);
            InvokeChanged(CollectionChangedEventArgs.ItemChanged(item, index));
        }

        public bool Remove(TElement item)
        {
            var index = value.IndexOf(item);
            if (index == -1) return false;
            value.RemoveAt(index);
            InvokeChanged(CollectionChangedEventArgs.ItemChanged(item, index));
            return true;
        }

        public void RemoveAt(int index)
        {
            var item = value[index];
            value.RemoveAt(index);
            InvokeChanged(CollectionChangedEventArgs.ItemChanged(item, index));
        }

        public bool Contains(TElement item)
        {
            return value.Contains(item);
        }

        public void CopyTo(TElement[] array, int arrayIndex)
        {
            value.CopyTo(array, arrayIndex);
        }

        public void Clear()
        {
            value.Clear();
            InvokeChanged(CollectionChangedEventArgs.Cleared());
        }

        private void InvokeChanged(CollectionChangedEventArgs args)
        {
            CollectionChanged?.Invoke(this, args);
        }
        
        #if UNITY_EDITOR
        public void NotifyChanges(IList<object> oldValues)
        {
            var itemWasAdded = value.Count > oldValues.Count;
            var itemWasRemoved = value.Count < oldValues.Count;
            
            var maxItems = Math.Max(value.Count, oldValues.Count);
            for (var i = 0; i < maxItems; ++i)
            {
                var oldElement = i < oldValues.Count ? oldValues[i] : null;
                var newElement = i < value.Count ? (object)value[i] : null;
                var elementsMatch = newElement == oldElement;
                
                // Item was removed
                if (itemWasRemoved && !elementsMatch)
                {
                    InvokeChanged(CollectionChangedEventArgs.ItemRemoved(oldElement, i));
                    break;
                }

                // Item was added
                if (itemWasAdded && !elementsMatch)
                {
                    InvokeChanged(CollectionChangedEventArgs.ItemAdded(newElement, i));
                    break;
                }
                
                // Items was changed in-place
                if (!elementsMatch)
                {
                    InvokeChanged(CollectionChangedEventArgs.ItemChanged(newElement, i));
                }
            }
        }
        #endif
    }
}