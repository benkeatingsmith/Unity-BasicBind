namespace BasicBind.Core
{
	/// <summary>
	/// Currently used by the `PrefabCollectionBinding` to inject configuration data into instantiated view models bound to a configuration data list.
	/// </summary>
	public interface IViewModelConfigurable
	{
		void Configure(object data);
	}
}