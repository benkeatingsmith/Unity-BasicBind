using System;
using BasicBind.Core;
using UnityEngine;
using UnityEngine.UI;

namespace BasicBind.Bindings
{
	public class ImageSpriteBinding : DataBinding
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
}