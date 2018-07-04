using System;
using Unity.Entities;

namespace MyGame
{
	[Serializable]
	public struct Lookat : IComponentData
	{
		public int targetId;
	}

	public class LookatComponent : ComponentDataWrapper<Lookat> {}
}