using System;

namespace SimpleBind
{
	/// <summary>
	/// Use on DataSourceReference fields to allow for filtering of DataSources by type.
	/// If no [AllowedDataTypes] attribute is found on a DataSourceReference, all types will appear in the inspector dropdown.
	/// </summary>
	public class AllowedDataTypesAttribute : Attribute
	{
		public Type[] AllowedDataTypes { get; }
        
		public AllowedDataTypesAttribute(params Type[] allowedDataTypes)
		{
			AllowedDataTypes = allowedDataTypes;
		}
	}
}