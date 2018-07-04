using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class CameraEntity : MonoBehaviour
{

	Transform t;
	EntityManager entity_mgr;
	EntityArchetype archetype;
	Entity entity;

	/// <summary>
	/// Start is called on the frame when a script is enabled just before
	/// any of the Update methods is called the first time.
	/// </summary>
	void Start()
	{
		entity_mgr = World.Active.GetOrCreateManager<EntityManager>();
		archetype = entity_mgr.CreateArchetype(CameraComponents());

		entity = entity_mgr.CreateEntity(archetype);

		entity_mgr.SetComponentData(entity, new TransformMatrix { Value = math.identity4 });
		entity_mgr.SetComponentData(entity, new MyGame.Camera { target_id = 10 });
	}

	ComponentType[] CameraComponents()
	{
		return new ComponentType[]
		{
			typeof(TransformMatrix),
			typeof(MyGame.Camera)
		};
	}


	/// <summary>
	/// Update is called every frame, if the MonoBehaviour is enabled.
	/// </summary>
	void Update()
	{
		
	}
}
