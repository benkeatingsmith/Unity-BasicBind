using UnityEngine.UI;

namespace SimpleBind
{
	public class TextFieldTextDataBinding : DataBinding
	{
		public Text TextField;
		public DataSourceReference StringSource;

		protected override void Setup()
		{
			Bind(StringSource, OnValueChanged);
		}

		private void OnValueChanged()
		{
			if (StringSource != null && StringSource.TryGetValue<string>(out var value))
			{
				if (TextField) TextField.text = value;
			}
		}
	}
}