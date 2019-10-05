using System;
using SimpleBind;
using UnityEngine;

[Serializable]
public struct ListItemData
{
	public string Name;
	public Sprite Avatar;
}

[Serializable]
public class ListItemDataSourceList : DataSourceList<ListItemData> { }

public class ListItemViewModel : ViewModel, IViewModelConfigurable
{
	public StringDataSource Name;
	public SpriteDataSource Avatar;
	
	public void Configure(object data)
	{
		if (!(data is ListItemData itemData)) return;
		
		Name.SetValue(itemData.Name);
		Avatar.SetValue(itemData.Avatar);
	}
}