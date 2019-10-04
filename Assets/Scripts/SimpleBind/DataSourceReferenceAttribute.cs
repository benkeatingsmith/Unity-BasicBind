using System;

namespace SimpleBind
{
	public class DataSourceReferenceAttribute : Attribute
	{
		public Type[] AllowedDataTypes { get; }
        
		public DataSourceReferenceAttribute(params Type[] allowedDataTypes)
		{
			AllowedDataTypes = allowedDataTypes;
		}
	}
}