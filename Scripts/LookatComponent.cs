using Unity.Entities;

namespace MyJob
{
	public struct Target : IComponentData
	{
		public int id;
	}

	public struct Lookat : IComponentData
	{
		public int targetId;
	}
}