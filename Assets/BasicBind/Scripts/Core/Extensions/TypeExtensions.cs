using System;

namespace BasicBind.Core
{
	public static class TypeExtensions
	{
		public static Type GetSubclassMatchingRawGeneric(this Type type, Type generic)
		{
			while (type != null && type != typeof(object))
			{
				var cur = type.IsGenericType ? type.GetGenericTypeDefinition() : type;
				if (generic == cur) return type;
				type = type.BaseType;
			}
			return null;
		}

		public static bool IsSubclassOfRawGeneric(this Type toCheck, Type generic)
		{
			return GetSubclassMatchingRawGeneric(toCheck, generic) != null;
		}
	}
}