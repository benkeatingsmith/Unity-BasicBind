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
}