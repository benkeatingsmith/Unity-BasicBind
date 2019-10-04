using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SimpleBind
{
	public static class DataBindingReflection
	{
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
		
		private static IEnumerable<FieldInfo> GetDataSourceFields(Type viewModelType)
		{
			return viewModelType.GetFields().Where(f => f.IsPublic && f.FieldType.IsSubclassOfRawGeneric(typeof(DataSource<>)));
		}

		private static string GetDataSourceDescription(FieldInfo field)
		{
			//TODO cache this
			var typeName = GetDataSourceElementType(field)?.Name ?? "Type Missing";
			return $"{field.Name} : {typeName}";
		}

		private static Type GetDataSourceElementType(FieldInfo field)
		{
			var dataSourceType = field.FieldType.GetSubclassMatchingRawGeneric(typeof(DataSource<>));
			return dataSourceType?.GetGenericArguments().FirstOrDefault();
		}
	}
}