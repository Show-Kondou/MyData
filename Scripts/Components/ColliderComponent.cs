using Unity.Entities;
using Unity.Mathematics;

public struct SphereCollider : IComponentData
{
	public float3 center;
	public float radius;

	public SphereCollider( float _radius, float3 _center)
	{
		radius = _radius;
		center = _center;
	}
}


public struct ResultCollider : IComponentData
{
	public int res; // ヒット回数

	public int Hit()
	{
		return ++res;
	}
}