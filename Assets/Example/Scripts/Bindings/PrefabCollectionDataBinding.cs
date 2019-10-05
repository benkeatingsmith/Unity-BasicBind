using System;
using System.Collections;
using System.Collections.Generic;
using BasicBind;
using UnityEngine;

public class PrefabCollectionDataBinding : DataBinding
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
	
	private GameObject TryGetChildAt(Transform parent, int index)
	{
		if (index < 0 || index >= parent.childCount) return null;
		return parent.GetChild(index).gameObject;
	}

	private GameObject NewInstance(GameObject prefab, object item)
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

	private static void DestroyInstance(GameObject instance)
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
		while (Root.transform.childCount > 0) DestroyInstance(Root.transform.GetChild(0).gameObject);
		instances.Clear();
	}
}