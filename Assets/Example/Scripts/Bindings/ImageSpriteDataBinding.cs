using System;
using BasicBind;
using UnityEngine;
using UnityEngine.UI;

public class ImageSpriteDataBinding : DataBinding
{
	public Image Image;
	[AllowedDataTypes(typeof(Sprite))] public DataSourceReference SpriteSource;

	protected override void Setup()
	{
		Image = Image ? Image : GetComponent<Image>();
		Bind(SpriteSource, UpdateImage);
	}

	private void UpdateImage(object sender, EventArgs eventArgs)
	{
		Image.sprite = ((DataSource<Sprite>) SpriteSource.Source).Value;
	}
}