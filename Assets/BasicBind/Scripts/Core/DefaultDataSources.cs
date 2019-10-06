using System;
using UnityEngine;

namespace BasicBind.Core
{
	[Serializable] public class BoolDataSource : DataSource<bool> { }
	[Serializable] public class IntDataSource : DataSource<int> { }
	[Serializable] public class FloatDataSource : DataSource<float> { }
	[Serializable] public class DoubleDataSource : DataSource<double> { }
	[Serializable] public class StringDataSource : DataSource<string> { }
	[Serializable] public class Vector2DataSource : DataSource<Vector2> { }
	[Serializable] public class Vector3DataSource : DataSource<Vector3> { }
	[Serializable] public class QuaternionDataSource : DataSource<Quaternion> { }
	[Serializable] public class ColorDataSource : DataSource<Color> { }
	[Serializable] public class SpriteDataSource : DataSource<Sprite> { }
}