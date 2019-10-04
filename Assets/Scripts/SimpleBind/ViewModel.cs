using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;

#endif

namespace SimpleBind
{
	/// <summary>
	/// ViewModels represent bindable containers of DataSource objects.
	/// All bindings will target a specific ViewModel in order to obtain a list of bindable data sources.
	/// </summary>
	public abstract class ViewModel : MonoBehaviour
	{
	}

#if UNITY_EDITOR
	[CustomEditor(typeof(ViewModel), true)]
	[CanEditMultipleObjects]
	public class ViewModelEditor : Editor
	{
		private bool expanded = true;

		public static IEnumerable<FieldInfo> GetDataSourceFields(Type viewModelType)
		{
			return viewModelType.GetFields().Where(f => f.IsPublic && f.FieldType.IsSubclassOfRawGeneric(typeof(DataSource<>)));
		}

		public static IEnumerable<string> GetDataSourceFieldNames(Type viewModelType, Type[] allowedDataTypes = null)
		{
			return GetDataSourceFields(viewModelType)
				.Where(fieldInfo => allowedDataTypes == null || allowedDataTypes.Contains(GetDataSourceElementType(fieldInfo)))
				.Select(x => x.Name);
		}

		public static IEnumerable<string> GetDataSourceDescriptions(Type viewModelType, Type[] allowedDataTypes = null)
		{
			return GetDataSourceFields(viewModelType)
				.Where(fieldInfo => allowedDataTypes == null || allowedDataTypes.Contains(GetDataSourceElementType(fieldInfo)))
				.Select(GetDataSourceDescription);
		}

		public static string GetDataSourceDescription(FieldInfo field)
		{
			//TODO cache this
			var typeName = GetDataSourceElementType(field)?.Name ?? "Type Missing";
			return $"{field.Name} : {typeName}";
		}

		public static Type GetDataSourceElementType(FieldInfo field)
		{
			var dataSourceType = field.FieldType.GetSubclassMatchingRawGeneric(typeof(DataSource<>));
			return dataSourceType?.GetGenericArguments().FirstOrDefault();
		}

		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();

			var firstSource = true;

			foreach (var dataSourceField in GetDataSourceFields(target.GetType()))
			{
				if (firstSource)
				{
					firstSource = false;
					expanded = EditorGUILayout.BeginFoldoutHeaderGroup(expanded, "Data Sources");
				}

				if (expanded)
				{
					var oldIndent = EditorGUI.indentLevel++;
					var dataSource = (IDataSource) dataSourceField.GetValue(target);
					var dataSourceType = dataSource.DataType;
					
					if (dataSourceType == typeof(int))
					{
						dataSource.SetValue(EditorGUILayout.IntField(dataSourceField.Name, dataSource.GetValue<int>()));
					}
					else if (dataSourceType == typeof(float))
					{
						dataSource.SetValue(EditorGUILayout.FloatField(dataSourceField.Name, dataSource.GetValue<float>()));
					}
					else if (dataSourceType == typeof(string))
					{
						dataSource.SetValue(EditorGUILayout.TextField(dataSourceField.Name, dataSource.GetValue<string>()));
					}
					else if (typeof(UnityEngine.Object).IsAssignableFrom(dataSourceType))
					{
						dataSource.SetValue(EditorGUILayout.ObjectField(dataSourceField.Name, dataSource.GetValue<UnityEngine.Object>(), dataSourceType, true));
					}
					else if (dataSourceType == typeof(bool))
					{
						dataSource.SetValue(EditorGUILayout.Toggle(dataSourceField.Name, dataSource.GetValue<bool>()));
					}
					EditorGUI.indentLevel = oldIndent;
				}
			}

			if (!firstSource)
			{
				EditorGUILayout.EndFoldoutHeaderGroup();
			}
		}
	}
#endif
}