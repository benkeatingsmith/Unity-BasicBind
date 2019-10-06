using System;
using System.Collections;
using System.Collections.Generic;
using BasicBind.Core;
using UnityEngine;

namespace BasicBind.Bindings
{
	public class PrefabCollectionBinding : DataBinding
	{
		public Transform Root;
		public GameObject Prefab;
		[AllowedDataTypes(typeof(IList))] public DataSourceReference CollectionSource;

		private readonly List<GameObject> instances = new List<GameObject>();

		protected override void Setup()
		{
			ClearInstances();
			BindCollection(CollectionSource, OnCollectionChanged, OnCollectionUnbound);
		}

		private void OnCollectionChanged(object sender, CollectionChangedEventArgs evt)
		{
			switch (evt.ChangeType)
			{
				case CollectionChangedEventArgs.ChangeTypes.ItemAdded:
				{
					var instance = NewInstance(Prefab, evt.Item);
					instance.transform.SetSiblingIndex(evt.Index);
					instances.Insert(evt.Index, instance);
					break;
				}
				case CollectionChangedEventArgs.ChangeTypes.ItemRemoved:
				{
					var instance = TryGetChildAt(Root.transform, evt.Index);
					if (instance) DestroyInstance(instance.gameObject);
					instances.RemoveAt(evt.Index);
					break;
				}
				case CollectionChangedEventArgs.ChangeTypes.ItemChanged:
				{
					var instance = TryGetChildAt(Root.transform, evt.Index);
					if (instance) TryConfigureViewModel(instance.gameObject, evt.Item);
					break;
				}
				case CollectionChangedEventArgs.ChangeTypes.Cleared:
				{
					ClearInstances();
					break;
				}
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		private void OnCollectionUnbound()
		{
			ClearInstances();
		}

		private static GameObject TryGetChildAt(Transform parent, int index)
		{
			if (index < 0 || index >= parent.childCount) return null;
			return parent.GetChild(index).gameObject;
		}

		protected virtual GameObject NewInstance(GameObject prefab, object item)
		{
			var instance = Instantiate(prefab, Root);
			TryConfigureViewModel(instance, item);
			return instance;
		}

		private static void TryConfigureViewModel(GameObject instance, object item)
		{
			var viewModel = instance.GetComponent<ViewModel>();
			if (viewModel is IViewModelConfigurable configurable) configurable.Configure(item);
		}

		protected virtual void DestroyInstance(GameObject instance)
		{
			if (Application.isPlaying)
			{
				Destroy(instance);
			}
			else
			{
				DestroyImmediate(instance);
			}
		}

		private void ClearInstances()
		{
			var childCount = Root.transform.childCount;
			for (var i = childCount - 1; i >= 0; --i)
			{
				DestroyInstance(Root.transform.GetChild(i).gameObject);
			}
			instances.Clear();
		}
	}
}