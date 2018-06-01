using Unity.Entities;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;
using MyJob;
using Unity.Mathematics;

public class BoidManager : MonoBehaviour
{
	EntityManager entityManager;
	EntityArchetype boidArchetype;
	EntityArchetype targetArchetype;

	[SerializeField] int createNum = 100;

	[SerializeField] MeshInstanceRenderer boid_render_data;
	[SerializeField] MeshInstanceRenderer target_render_data;

	/// <summary>
	/// Start is called on the frame when a script is enabled just before
	/// any of the Update methods is called the first time.
	/// </summary>
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
	void Start()
	{
		Debug.Log("Create Entity Mangaer");
		entityManager = World.Active.GetOrCreateManager<EntityManager>();
		CreateArchetype();
		CreateBoids(createNum);
		CreateTaget();
	}

	void CreateArchetype()
	{
		Debug.Log("Create Archetype");
		// Boidのアーキテクチャ生成
		boidArchetype = entityManager.CreateArchetype(
			// 移動に必要なComponent
			typeof(Position),
			typeof(Heading),
			typeof(MoveForward),
			typeof(MoveSpeed),
			// 方向盛業に必要なComponent
			typeof(Lookat),
			// 描画で必要なComponent
			typeof(TransformMatrix),
			typeof(MeshInstanceRenderer)
			);

		// ターゲットのアーキテクチャ生成
		targetArchetype = entityManager.CreateArchetype(
			typeof(Position),
			typeof(Target),
			typeof(TransformMatrix),
			typeof(MeshInstanceRenderer)
			);
	}

	void CreateBoids(int num)
	{
		Debug.Log("Create boid");
		var r = 100.0F / 2.0F;
		var f = new float3(transform.forward.x, transform.forward.y, transform.forward.z);
		var render_data = new MeshInstanceRenderer { mesh = boid_render_data.mesh, material = boid_render_data.material };
		for (int i = 0; i < num; i++)
		{
			var pos = new float3(Random.Range(-r,r), Random.Range(-r,r),Random.Range(-r,r) + 50F);
			// Entity生成
			var boid = entityManager.CreateEntity(boidArchetype);
			// EntityのComponentにデータ設定
			entityManager.SetComponentData(boid, new Position(pos) );
			entityManager.SetComponentData(boid, new Heading { Value = new float3(0,0,1) });
			entityManager.SetComponentData(boid, new MoveSpeed { speed = 10.0F });
			entityManager.SetComponentData(boid, new Lookat { targetId = 1 });
			entityManager.SetSharedComponentData(boid, boid_render_data);
		}
	}

	void CreateTaget()
	{
		var target = entityManager.CreateEntity(targetArchetype);
		entityManager.SetComponentData(target, new Position { Value = new float3(0F, 10F, 100F) });
		entityManager.SetComponentData(target, new Target { id = 1 });
		entityManager.SetSharedComponentData(target, target_render_data);
	}

	/// <summary>
	/// Update is called every frame, if the MonoBehaviour is enabled.
	/// </summary>
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			CreateBoids(createNum);
		}
	}
}

[System.Serializable]
public struct RenderData
{
	public Mesh mesh;
	public Material material;
}