using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace SimpleBind
{
	public interface IDataSource
	{
		event EventHandler Changed;

		Type DataType { get; }

		void SetValue(object value);
		T GetValue<T>();
		
		void InvokeChanged();
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
				InvokeChanged();
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

		public T1 GetValue<T1>()
		{
			if (DataType.IsClass && typeof(T1).IsClass) return (T1) (object) value;
			return (T1) Convert.ChangeType(value, typeof(T1));
		}

		public void InvokeChanged()
		{
			Changed?.Invoke(this, EventArgs.Empty);
		}
	}
	
#if UNITY_EDITOR
	// Do not draw data sources in inspector individually.
	// Instead, View Models have special rendering code for data sources.
	[CustomPropertyDrawer(typeof(IDataSource), true)]
	public class DataSourcePropertyDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			var valueProp = property.FindPropertyRelative("value");
			if (valueProp != null)
			{
				EditorGUI.BeginChangeCheck();
				EditorGUI.PropertyField(position, valueProp, label, true);
				if (EditorGUI.EndChangeCheck())
				{
					property.serializedObject.ApplyModifiedProperties();
					var dataSource = (IDataSource) fieldInfo.GetValue(property.serializedObject.targetObject);
					dataSource.InvokeChanged();
				}
			}
		}
		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			var valueProp = property.FindPropertyRelative("value");
			return EditorGUI.GetPropertyHeight(valueProp);
		}
	}
#endif
}