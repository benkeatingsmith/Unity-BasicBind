using System;
using BasicBind.Core;
using UnityEngine.UI;

namespace BasicBind.Bindings
{
	public class TextFieldTextBinding : DataBinding
	{
		public Text TextField;
		[AllowedDataTypes(typeof(string))] public DataSourceReference StringSource;

		protected override void Setup()
		{
			Bind(StringSource, OnValueChanged);
		}

		private void OnValueChanged(object sender, EventArgs eventArgs)
		{
			if (StringSource != null && StringSource.TryGetValue<string>(out var value))
			{
				if (TextField) TextField.text = value;
			}
		}
	}
}