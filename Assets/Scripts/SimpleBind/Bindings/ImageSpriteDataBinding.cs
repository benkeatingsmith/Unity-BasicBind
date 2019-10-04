using UnityEngine;
using UnityEngine.UI;

namespace SimpleBind
{
	public class ImageSpriteDataBinding : DataBinding
	{
		public Image Image;
		[AllowedDataTypes(typeof(Sprite))] public DataSourceReference SpriteSource;

		protected override void Setup()
		{
			Image = Image ? Image : GetComponent<Image>();
			Bind(SpriteSource, UpdateImage);
		}

		private void UpdateImage()
		{
			Image.sprite = ((DataSource<Sprite>) SpriteSource.Source).Value;
		}
	}
}