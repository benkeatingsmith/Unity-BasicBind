using BasicBind;
using UnityEngine;

public class AppViewModel : ViewModel
{
	public StringDataSource AppTitle;
	
	[Header("Data Source Examples")]
	public StringDataSource DisplayedText;
	public SpriteDataSource IconSprite;
	public BoolDataSource IconActive;
	public FloatDataSource IconWidth;
	
	[Header("Data Source List Example")]
	public ListItemDataSourceList ListItems;
}