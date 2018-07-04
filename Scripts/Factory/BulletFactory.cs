
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;

public class BulletFactory : MonoBehaviour
{
	EntityManager entity_mgr;
	EntityArchetype bullet_archetype;

	[SerializeField] float move_speed;
	[SerializeField] MeshInstanceRenderer bullet_render_data;

	/// <summary>
	/// Start is called on the frame when a script is enabled just before
	/// any of the Update methods is called the first time.
	/// </summary>
	void Start()
	{
		entity_mgr = World.Active.GetOrCreateManager<EntityManager>();
		bullet_archetype = entity_mgr.CreateArchetype(BulletComponents());

	}

	ComponentType[] BulletComponents()
	{
		return new ComponentType[]
		{
			typeof(MeshInstanceRenderer),
			typeof(TransformMatrix),
			typeof(Position),
			typeof(Heading),
			typeof(MoveForward),
			typeof(MoveSpeed),
		};
	}

	public void ShotBullet(float3 pos, float3 vec)
	{
		var bullet_entity = entity_mgr.CreateEntity(bullet_archetype);
		entity_mgr.SetSharedComponentData(bullet_entity, bullet_render_data);
		entity_mgr.SetComponentData(bullet_entity, new Position(pos));
		entity_mgr.SetComponentData(bullet_entity, new Heading(vec));
		entity_mgr.SetComponentData(bullet_entity, new MoveSpeed { speed = move_speed });
	}


	/// <summary>
	/// Update is called every frame, if the MonoBehaviour is enabled.
	/// </summary>
	void Update()
	{
		if (Input.GetKey(KeyCode.Space))
		{
			ShotBullet( transform.position, transform.forward );
		}
	}
}