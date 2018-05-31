using UnityEngine;
using UnityEngine.UI;

// ECSで使う
using Unity.Entities;
using Unity.Transforms;
using Unity.Rendering;
using Unity.Mathematics;

public class ECS : MonoBehaviour
{
	EntityManager entityManager;
	EntityArchetype objArchetype;
	MeshInstanceRenderer renderer;

	[SerializeField] Text text;

	[SerializeField] int addObjectNum = 100;
	int objectCount;

	/// <summary>
	/// Start is called on the frame when a script is enabled just before
	/// any of the Update methods is called the first time.
	/// </summary>
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
	void Start()
	{
		// Entitiyを生成したり設定したりするマネージャー
		entityManager = World.Active.GetOrCreateManager<EntityManager>();
		objArchetype = entityManager.CreateArchetype(
			typeof(TransformMatrix),
			typeof(Position),
			typeof(Rotation),
			typeof(MoveForward),
			typeof(MoveSpeed)
			 );
		renderer = GetComponent<MeshInstanceRendererComponent>().Value;
		objectCount = 0;
	}


	/// <summary>
	/// Update is called every frame, if the MonoBehaviour is enabled.
	/// </summary>
	void Update()
	{
		text.text = $"オブジェクトの数:{objectCount}";
		
		if(Input.GetKeyDown(KeyCode.Space))
		{
			CreateObject();
		}
	}

	void CreateObject()
	{
		for (int i = 0; i < addObjectNum; i++)
		{
			// アーキテクチャを元にエンティティを作成
			var entity = entityManager.CreateEntity(objArchetype);

			// エンティティに値を設定
			var pos = transform.position;
			var s = Random.Range(0F, 5F);
			entityManager.SetComponentData(entity, new Position { Value = new float3(pos.x, pos.y, pos.z) });
			entityManager.SetComponentData(entity, new Rotation { Value = Quaternion.Euler(Random.Range(0F, 360F), 90F, 90F) });
			entityManager.SetComponentData(entity, new MoveSpeed { speed = s });
			entityManager.SetSharedComponentData(entity, new MoveForward());
			entityManager.AddSharedComponentData(entity, renderer);

		}
		objectCount += addObjectNum;
	}
}