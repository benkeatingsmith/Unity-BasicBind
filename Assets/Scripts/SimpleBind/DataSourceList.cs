using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleBind
{
    public interface IDataSourceList : IDataSource
    {
        event EventHandler<CollectionChangedEventArgs> CollectionChanged;

		Type ElementType { get; }
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
                CollectionChanged?.Invoke(this, CollectionChangedEventArgs.ItemChanged(value, key));
            }
        }

        public int IndexOf(TElement item)
        {
            return value.IndexOf(item);
        }

        public void Add(TElement item)
        {
            value.Add(item);
            CollectionChanged?.Invoke(this, CollectionChangedEventArgs.ItemChanged(item, value.Count - 1));
        }

        public void Insert(int index, TElement item)
        {
            value.Insert(index, item);
            CollectionChanged?.Invoke(this, CollectionChangedEventArgs.ItemChanged(item, index));
        }

        public bool Remove(TElement item)
        {
            var index = value.IndexOf(item);
            if (index == -1) return false;
            value.RemoveAt(index);
            CollectionChanged?.Invoke(this, CollectionChangedEventArgs.ItemChanged(item, index));
            return true;
        }

        public void RemoveAt(int index)
        {
            var item = value[index];
            value.RemoveAt(index);
            CollectionChanged?.Invoke(this, CollectionChangedEventArgs.ItemChanged(item, index));
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
            CollectionChanged?.Invoke(this, CollectionChangedEventArgs.Cleared());
        }
    }
}