using System;
using SimpleBind;
using UnityEngine;

public class GameObjectActiveDataBinding : DataBinding
{
	public GameObject Target;
	[AllowedDataTypes(typeof(bool))] public DataSourceReference BoolSource;

	protected override void Setup()
	{
		UnbindOnDisable = false;
		Bind(BoolSource, OnValueChanged);
	}

	private void OnValueChanged(object sender, EventArgs eventArgs)
	{
		if (BoolSource != null && BoolSource.TryGetValue<bool>(out var value))
		{
			if (Target) Target.SetActive(value);
		}
	}
}