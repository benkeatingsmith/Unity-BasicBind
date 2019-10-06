using UnityEditor;
using UnityEngine;

namespace BasicBind.Core
{
	[CustomEditor(typeof(DataBinding), true)]
	[CanEditMultipleObjects]
	public class DataBindingEditor : Editor
	{
		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();

			if (GUILayout.Button("Locate View Model"))
			{
				((DataBinding) target).LocateViewModel();
			}
		}
	}
}