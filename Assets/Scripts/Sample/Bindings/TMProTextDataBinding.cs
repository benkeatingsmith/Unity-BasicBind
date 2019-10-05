using System;
using SimpleBind;
using TMPro;

public class TMProTextDataBinding : DataBinding
{
	public TMP_Text TextField;
	[AllowedDataTypes(typeof(string))] public DataSourceReference StringSource;

	protected override void Setup()
	{
		Bind(StringSource, OnValueChanged);
	}

	private void OnValueChanged(object sender, EventArgs eventArgs)
	{
		if (StringSource != null && StringSource.TryGetValue<string>(out var value))
		{
			if (TextField) TextField.SetText(value);
		}
	}
}