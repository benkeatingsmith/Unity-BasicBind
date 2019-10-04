using System;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

// TODO Filter data sources by bind type
// TODO Support collections
// TODO Cache reflection data
// TODO Performance testing
// TODO GitHub
// TODO Readme

namespace SimpleBind
{
	[ExecuteInEditMode]
	public abstract class DataBinding : MonoBehaviour
	{
		private struct Entry
		{
			public DataSourceReference DataSourceReference;
			public Action Handler;
		}

		public ViewModel ViewModel;

		private readonly List<Entry> bindings = new List<Entry>();

		private void OnEnable()
		{
			if (!ViewModel) LocateViewModel();
			UnbindAll();
			Setup();
		}

		protected abstract void Setup();

		protected void Bind(DataSourceReference dataSourceReference, Action handler)
		{
			if (dataSourceReference?.Source == null) return;

			bindings.Add(new Entry
			{
				DataSourceReference = dataSourceReference,
				Handler = handler
			});
			
			dataSourceReference.Source.Changed += handler;
			handler?.Invoke();
		}

		protected void Unbind(DataSourceReference dataSourceReference)
		{
			var idx = bindings.FindIndex(x => x.DataSourceReference == dataSourceReference);
			if (idx == -1) return;
			
			Unbind(idx, true);
		}

		private void OnDisable()
		{
			UnbindAll();
		}

		private void Unbind(int bindingEntryIdx, bool removeEntry)
		{
			var entry = bindings[bindingEntryIdx];
			if (entry.DataSourceReference.Source != null) entry.DataSourceReference.Source.Changed -= entry.Handler;
			if (removeEntry) bindings.EraseSwap(bindingEntryIdx);
		}

		private void UnbindAll()
		{
			for (var i = bindings.Count - 1; i >= 0; --i)
			{
				Unbind(i, false);
			}
			bindings.Clear();
		}

		internal void LocateViewModel()
		{
			ViewModel = GetComponentInParent<ViewModel>();
		}
	}

#if UNITY_EDITOR
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
#endif
}