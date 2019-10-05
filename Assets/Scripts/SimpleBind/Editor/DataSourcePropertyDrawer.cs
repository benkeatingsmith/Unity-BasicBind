using System.Collections;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace SimpleBind
{
	[CustomPropertyDrawer(typeof(IDataSource), true)]
	public class DataSourcePropertyDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			var valueProp = property.FindPropertyRelative("value");
			var dataSource = fieldInfo.GetValue(property.serializedObject.targetObject) as IDataSource;
			if (valueProp != null && dataSource != null)
			{
				EditorGUI.BeginChangeCheck();
				
				var oldValue = dataSource.GetValue<object>();
				if (dataSource is IDataSourceList) oldValue = ((IList) oldValue).Cast<object>().ToList();

				EditorGUI.PropertyField(position, valueProp, label, true);
				if (EditorGUI.EndChangeCheck())
				{
					property.serializedObject.ApplyModifiedProperties();
					InvokeChangedEvents(dataSource, oldValue);
				}
			}
		}

		private void InvokeChangedEvents(IDataSource dataSource, object oldValue)
		{
			if (dataSource is IDataSourceList dataSourceList)
			{
				var oldList = ((IList) oldValue).Cast<object>().ToList();
				dataSourceList.NotifyChanges(oldList);
			}
			else
			{
				dataSource.NotifyChanged();
			}
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			var valueProp = property.FindPropertyRelative("value");
			return EditorGUI.GetPropertyHeight(valueProp);
		}
	}
}