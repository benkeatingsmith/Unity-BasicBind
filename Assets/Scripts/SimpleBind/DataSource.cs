using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace SimpleBind
{
	public interface IDataSource
	{
		event Action Changed;

		Type DataType { get; }

		void SetValue(object value);
		T GetValue<T>();
	}

	[Serializable]
	public class DataSource<T> : IDataSource
	{
		public event Action Changed;

		public Type DataType => typeof(T);

		public T Value
		{
			get => value;
			set
			{
				if (Equals(value, this.value)) return;
				this.value = value;
				Changed?.Invoke();
			}
		}
		[SerializeField] private T value; 

		public DataSource(T value = default)
		{
			this.value = value;
		}

		public void SetValue(object value)
		{
			T convertedValue = default;
			if (value != null)
			{
				if (value.GetType().IsClass && DataType.IsClass)
				{
					convertedValue = (T) value;
				}
				else
				{
					convertedValue = (T) Convert.ChangeType(value, DataType);
				}
			}
			Value = convertedValue;
		}

		public T1 GetValue<T1>()
		{
			if (DataType.IsClass && typeof(T1).IsClass) return (T1) (object) value;
			return (T1) Convert.ChangeType(value, typeof(T1));
		}
	}
	
#if UNITY_EDITOR
	[CustomPropertyDrawer(typeof(IDataSource), true)]
	public class DataSourcePropertyDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			return 0;
		}
	}
#endif
}