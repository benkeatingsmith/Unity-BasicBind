using System;
using BasicBind.Core;
using UnityEngine.UI;

namespace BasicBind.Bindings
{
	public class ImageSizeBinding : DataBinding
	{
		public Image Image;
		[AllowedDataTypes(typeof(float), typeof(int), typeof(double))] public DataSourceReference WidthSource;
		[AllowedDataTypes(typeof(float), typeof(int), typeof(double))] public DataSourceReference HeightSource;
	
		protected override void Setup()
		{
			Bind(WidthSource, OnSizeChanged);
			Bind(HeightSource, OnSizeChanged);
		}

		private void OnSizeChanged(object sender, EventArgs e)
		{
			if (!Image) return;
		
			var size = Image.rectTransform.sizeDelta;
			if (WidthSource.TryGetValue<float>(out var width))
			{
				size.x = width;
			}
			if (HeightSource.TryGetValue<float>(out var height))
			{
				size.y = height;
			}
			Image.rectTransform.sizeDelta = size;
		}
	}
}