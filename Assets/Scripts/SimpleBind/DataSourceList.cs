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
			var newValues = value;
			
			// Handle removals
			for (var i = oldValues.Count - 1; i >= newValues.Count; --i)
			{
				InvokeChanged(CollectionChangedEventArgs.ItemRemoved(oldValues[i], i));
			}
			
			// Handle additions
			for (var i = oldValues.Count; i < newValues.Count; ++i)
			{
				InvokeChanged(CollectionChangedEventArgs.ItemAdded(newValues[i], i));
			}
			
			// Handle changes
			for (var i = 0; i < Math.Min(oldValues.Count, newValues.Count); ++i)
			{
				var oldElement = oldValues[i];
				var newElement = newValues[i];
				var elementsMatch = newElement.Equals(oldElement);
				if (!elementsMatch)
				{
					InvokeChanged(CollectionChangedEventArgs.ItemChanged(newElement, i));
				}
			}
		}
#endif
	}
}