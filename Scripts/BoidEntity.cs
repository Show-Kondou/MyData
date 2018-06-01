using MyJob;
using Unity.Entities;
using Unity.Rendering;
using Unity.Transforms;

public class Boid
{
	public static ComponentType[] cpmponents = 
	{
		typeof(Position),
		typeof(Rotation),
		typeof(MoveForward),
		typeof(MoveSpeed),
		typeof(Lookat),
		typeof(TransformMatrix),
		typeof(MeshInstanceRenderer)
	};
}


public class TargetClass
{
	public static ComponentType[] cpmponents =
	{
		typeof(Position),
		typeof(MyJob.Target)
	};
}