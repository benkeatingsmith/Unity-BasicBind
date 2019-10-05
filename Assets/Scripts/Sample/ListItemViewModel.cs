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

public class ListItemViewModel : ViewModel
{
	public StringDataSource Name;
	public SpriteDataSource Avatar;
}