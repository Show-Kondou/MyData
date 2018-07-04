
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;
using MyGame;

public class BoidFactory : MonoBehaviour
{
	EntityManager   entity_mgr;
	EntityArchetype boid_archetype; // アーキタイプ

	[SerializeField] int                  create_num = 100;
	[SerializeField] SpeedMinMaxValue     speed_Value;
	[SerializeField] Vector3              Create_Space;
	[SerializeField] MeshInstanceRenderer boid_render_data;

	/// <summary>
	/// Start is called on the frame when a script is enabled just before
	/// any of the Update methods is called the first time.
	/// </summary>
	void Start()
	{
		entity_mgr = World.Active.GetOrCreateManager<EntityManager>();
		boid_archetype = entity_mgr.CreateArchetype(BoidComponrnts());

		CreateBoids(create_num);
	}


	ComponentType[] BoidComponrnts()
	{
		return new ComponentType[]
		{
			typeof(MeshInstanceRenderer),
			typeof(TransformMatrix),
			typeof(Position),
			typeof(Heading),
			typeof(MoveForward),
			typeof(MoveSpeed),
			typeof(Lookat),
			// typeof(Target),
		};
	}


	void CreateBoids(int create_num, int target_id = 0)
	{
		var center_pos = transform.position;
		for (int i = 0; i < create_num; i++)
		{
			var space = Create_Space / 2.0F;
			var pos = new float3(
				Random.Range( -space.x, space.x ) + center_pos.x,
				Random.Range( -space.y, space.y )+ center_pos.y,
				Random.Range( -space.z, space.z )+ center_pos.z);
			var speed = Random.Range( speed_Value.Min, speed_Value.Max );

			var boid_entity = entity_mgr.CreateEntity(boid_archetype);
			entity_mgr.SetSharedComponentData(boid_entity, boid_render_data);
			entity_mgr.SetComponentData(boid_entity, new Position(pos));
			entity_mgr.SetComponentData(boid_entity, new Heading(transform.forward));
			entity_mgr.SetComponentData(boid_entity, new MoveSpeed { speed = speed });
			entity_mgr.SetComponentData(boid_entity, new MoveSpeed { speed = speed });
			entity_mgr.SetComponentData(boid_entity, new Lookat { targetId = target_id });
			// entity_mgr.SetComponentData(boid_entity, new Lookat { targetId = i });
			// entity_mgr.SetComponentData(boid_entity, new Target { id = i + 1 });
		}
	}

	[System.Serializable]
	struct SpeedMinMaxValue
	{
		public float Min;
		public float Max;
	}
}