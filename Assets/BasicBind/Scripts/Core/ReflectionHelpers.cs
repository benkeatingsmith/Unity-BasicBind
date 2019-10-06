using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BasicBind.Core
{
	public static class ReflectionHelpers
	{
        private static readonly Dictionary<Type, FieldInfo[]> CachedDataSourceFields = new Dictionary<Type, FieldInfo[]>();
        private static readonly Dictionary<Type, string> CachedTypeDescriptions = new Dictionary<Type, string>();
        
		public static IEnumerable<string> GetDataSourceFieldNames(Type viewModelType, Type[] allowedDataTypes = null)
		{
			return GetDataSourceFields(viewModelType, allowedDataTypes)
				.Select(x => x.Name);
		}

		public static IEnumerable<string> GetDataSourceDescriptions(Type viewModelType, Type[] allowedDataTypes = null)
		{
			return GetDataSourceFields(viewModelType, allowedDataTypes)
				.Select(GetDataSourceDescription);
		}
		
		private static IEnumerable<FieldInfo> GetDataSourceFields(Type viewModelType, Type[] allowedDataTypes)
		{
			if (!CachedDataSourceFields.TryGetValue(viewModelType, out var dataSourceTypes))
			{
				dataSourceTypes = viewModelType.GetFields()
					.Where(f => f.IsPublic && f.FieldType.IsSubclassOfRawGeneric(typeof(DataSource<>)))
					.ToArray();
				CachedDataSourceFields[viewModelType] = dataSourceTypes;
			}
			return dataSourceTypes.Where(fieldInfo =>
				{
					var elementType = GetDataSourceElementType(fieldInfo);
					return allowedDataTypes == null || allowedDataTypes.Any(x => x.IsAssignableFrom(elementType));
				});
		}

		private static string GetDataSourceDescription(FieldInfo field)
		{
			if (!CachedTypeDescriptions.TryGetValue(field.FieldType, out var typeName))
			{
				typeName = "Type Missing";
				var dataType = GetDataSourceElementType(field);
				if (dataType != null)
				{
					typeName = GetFormattedTypeName(dataType);
				}
				CachedTypeDescriptions[field.FieldType] = typeName;
			}
			return $"{field.Name} : {typeName}";
		}

		private static Type GetDataSourceElementType(FieldInfo field)
		{
			var dataSourceType = field.FieldType.GetSubclassMatchingRawGeneric(typeof(DataSource<>));
			return dataSourceType?.GetGenericArguments().FirstOrDefault();
		}

		private static string GetFormattedTypeName(Type type)
		{
            if (type == null) return null;
            
            var typeName = type.Name;

            if (!type.IsGenericType) return typeName;
            var genArgs = type.GetGenericArguments();

            if (genArgs.Length <= 0) return typeName;
            typeName = typeName.Substring(0, typeName.Length - 2);

            var args = "";
            foreach (var argType in genArgs)
            {
                var argName = argType.Name;

                if (argType.IsGenericType)
                {
                    argName = GetFormattedTypeName(argType);
                }
                args += argName + ", ";
            }

            typeName = $"{typeName}<{args.Substring(0, args.Length - 2)}>";
            return typeName;
		}
	}
}