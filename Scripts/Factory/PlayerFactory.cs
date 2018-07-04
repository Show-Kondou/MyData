using MyGame;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;

public class PlayerFactory : MonoBehaviour
{
	EntityManager entity_mgr;
	EntityArchetype player_archetype;

	[SerializeField] MeshInstanceRenderer player_render_data;
	[SerializeField] float move_speed;
	[SerializeField] int target_id;

	/// <summary>
	/// Start is called on the frame when a script is enabled just before
	/// any of the Update methods is called the first time.
	/// </summary>
	void Start()
	{
		entity_mgr = World.Active.GetOrCreateManager<EntityManager>();
		player_archetype = entity_mgr.CreateArchetype(PlayerComponents());
		CreatePlayer();
	}


	ComponentType[] PlayerComponents()
	{
		return new ComponentType[]
		{
			typeof(MeshInstanceRenderer),
			typeof(TransformMatrix),
			typeof(Position),
			typeof(Rotation),
			typeof(MoveSpeed),
			typeof(Player),
			typeof(Target),
		};
	}


	void CreatePlayer()
	{
		var f = transform.forward;
		var u = transform.up;
		var r = math.cross(u,f);
		Debug.Log(r);
		Debug.Log(transform.right);


		var player_entuty = entity_mgr.CreateEntity(player_archetype);
		entity_mgr.SetSharedComponentData(player_entuty, player_render_data);
		// entity_mgr.SetComponentData(player_entuty, new TransformMatrix {Value = math.identity4 });
		entity_mgr.SetComponentData(player_entuty, new Position(transform.position));
		entity_mgr.SetComponentData(player_entuty, new Rotation(transform.rotation));
		entity_mgr.SetComponentData(player_entuty, new MoveSpeed { speed = move_speed });
		entity_mgr.SetComponentData(player_entuty, new Target { id = target_id });
	}

}