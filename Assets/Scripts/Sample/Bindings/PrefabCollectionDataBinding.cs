using System;
using System.Collections.Generic;
using SimpleBind;
using UnityEngine;

public class PrefabCollectionDataBinding : DataBinding
{
	public Transform Root;
	public GameObject Prefab;
	[AllowedDataTypes(typeof(List<>))] public DataSourceReference CollectionSource;
	
	private readonly List<GameObject> instances = new List<GameObject>();
	
	protected override void Setup()
	{
		BindCollection(CollectionSource, OnCollectionChanged);
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
				var instance = Root.transform.GetChild(evt.Index);
				if (instance) DestroyInstance(instance.gameObject);
				instances.RemoveAt(evt.Index);
				break;
			}
			case CollectionChangedEventArgs.ChangeTypes.ItemChanged:
			{
				var instance = Root.transform.GetChild(evt.Index);
				if (instance) TryConfigureViewModel(instance.gameObject, evt.Item);
				break;
			}
			case CollectionChangedEventArgs.ChangeTypes.Cleared:
			{
				while (Root.transform.childCount > 0) DestroyInstance(Root.transform.GetChild(0).gameObject);
				instances.Clear();
				break;	
			}
			default:
				throw new ArgumentOutOfRangeException();
		}
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
		Destroy(instance);
	}
}