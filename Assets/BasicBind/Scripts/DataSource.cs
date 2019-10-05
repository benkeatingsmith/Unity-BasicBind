using System;
using UnityEngine;

namespace BasicBind
{
	public interface IDataSource
	{
		event EventHandler Changed;

		Type DataType { get; }

		void SetValue(object value);
		T GetValue<T>();
		
		#if UNITY_EDITOR
		void NotifyChanged();
		#endif
	}

	/// <summary>
	/// DataSources are wrappers around data that emit Changed events when the wrapped data is modified.
	/// DataBindings bind to individual DataSources in order to run logic on data changes.
	/// </summary>
	/// <typeparam name="TData">Type of wrapped data</typeparam>
	[Serializable]
	public class DataSource<TData> : IDataSource
	{
		public event EventHandler Changed;

		public Type DataType => typeof(TData);

		public TData Value
		{
			get => value;
			set
			{
				if (Equals(value, this.value)) return;
				this.value = value;
				NotifyChanged();
			}
		}
		[SerializeField] protected TData value; 

		public DataSource(TData value = default)
		{
			this.value = value;
		}

		public void SetValue(object value)
		{
			TData convertedValue = default;
			if (value != null)
			{
				if (value.GetType().IsClass && DataType.IsClass)
				{
					convertedValue = (TData) value;
				}
				else
				{
					convertedValue = (TData) Convert.ChangeType(value, DataType);
				}
			}
			Value = convertedValue;
		}

		public T GetValue<T>()
		{
			var targetType = typeof(T);
			if (DataType.IsClass && targetType.IsClass || targetType == typeof(object)) return (T) (object) value;
			return (T) Convert.ChangeType(value, typeof(T));
		}

		#if UNITY_EDITOR
		public void NotifyChanged()
		{
			Changed?.Invoke(this, EventArgs.Empty);
		}
		#endif
	}
}