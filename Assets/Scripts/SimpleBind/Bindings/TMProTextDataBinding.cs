using TMPro;

namespace SimpleBind
{
	public class TMProTextDataBinding : DataBinding
	{
		public TMP_Text TextField;
		public DataSourceReference StringSource;

		protected override void Setup()
		{
			Bind(StringSource, OnValueChanged);
		}

		private void OnValueChanged()
		{
			if (StringSource != null && StringSource.TryGetValue<string>(out var value))
			{
				if (TextField) TextField.SetText(value);
			}
		}
	}
}