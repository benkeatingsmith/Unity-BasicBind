using System;

namespace BasicBind
{
	public class CollectionChangedEventArgs : EventArgs
	{
		public enum ChangeTypes { ItemAdded, ItemRemoved, ItemChanged, Cleared }

		public ChangeTypes ChangeType;
		public object Item;
		public int Index = -1;

		public static CollectionChangedEventArgs ItemAdded(object item, int index) => new CollectionChangedEventArgs
		{
			ChangeType = ChangeTypes.ItemAdded,
			Item = item,
			Index = index
		};
        
		public static CollectionChangedEventArgs ItemRemoved(object item, int index) => new CollectionChangedEventArgs
		{
			ChangeType = ChangeTypes.ItemRemoved,
			Item = item,
			Index = index
		};
        
		public static CollectionChangedEventArgs ItemChanged(object item, int index) => new CollectionChangedEventArgs
		{
			ChangeType = ChangeTypes.ItemChanged,
			Item = item,
			Index = index
		};
        
		public static CollectionChangedEventArgs Cleared() => new CollectionChangedEventArgs
		{
			ChangeType = ChangeTypes.Cleared
		};
	}
}