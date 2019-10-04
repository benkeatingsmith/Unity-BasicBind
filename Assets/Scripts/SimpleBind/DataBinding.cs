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
	/// <summary>
	/// DataBindings are components which observe DataSources and can execute logic when they change.
	/// Each DataBinding should contain DataSourceReferences which allow for the binding of DataSources on specific ViewModels.
	/// The ViewModel for DataBindings can be explicitly set or can be located (by searching upwards from the
	/// DataBinding owner) using the 'Locate View Model' button in the inspector.
	/// </summary>
	[ExecuteInEditMode]
	public abstract class DataBinding : MonoBehaviour
	{
		private struct Entry
		{
			public DataSourceReference DataSourceReference;
			public EventHandler Handler;
		}

		public ViewModel ViewModel;

		protected bool UnbindOnDisable = true;
		protected bool UnbindOnDestroy = true;

		private readonly List<Entry> bindings = new List<Entry>();

		protected virtual void OnEnable()
		{
			if (!ViewModel) LocateViewModel();
			Rebind();
		}

		protected abstract void Setup();

		protected void Bind(DataSourceReference dataSourceReference, EventHandler handler)
		{
			if (dataSourceReference?.Source == null) return;

			bindings.Add(new Entry
			{
				DataSourceReference = dataSourceReference,
				Handler = handler
			});
			
			dataSourceReference.Source.Changed += handler;
			handler?.Invoke(this, EventArgs.Empty);
		}

		protected void Unbind(DataSourceReference dataSourceReference)
		{
			var idx = bindings.FindIndex(x => x.DataSourceReference == dataSourceReference);
			if (idx == -1) return;
			
			Unbind(idx, true);
		}

		protected virtual void OnDisable()
		{
			if (UnbindOnDisable) UnbindAll();
		}

		protected virtual void OnDestroy()
		{
			if (UnbindOnDestroy) UnbindAll();
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

		internal void Rebind()
		{
			UnbindAll();
			Setup();
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