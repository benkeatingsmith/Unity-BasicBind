using System;

namespace SimpleBind
{
	public class AllowedDataTypesAttribute : Attribute
	{
		public Type[] AllowedDataTypes { get; }
        
		public AllowedDataTypesAttribute(params Type[] allowedDataTypes)
		{
			AllowedDataTypes = allowedDataTypes;
		}
	}
}