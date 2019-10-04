using UnityEngine;

namespace SimpleBind
{
	public class GameObjectActiveDataBinding : DataBinding
	{
		public GameObject Target;
		public DataSourceReference BoolSource;

		protected override void Setup()
		{
			Bind(BoolSource, OnValueChanged);
		}

		private void OnValueChanged()
		{
			if (BoolSource != null && BoolSource.TryGetValue<bool>(out var value))
			{
				if (Target) Target.SetActive(value);
			}
		}
	}
}