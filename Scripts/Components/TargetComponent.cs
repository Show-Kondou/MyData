using System;
using Unity.Entities;

namespace MyGame
{
	[Serializable]
	public struct Target : IComponentData
	{
		public int id;
	}

	public class TargetComponent : ComponentDataWrapper<Target> {}
}



