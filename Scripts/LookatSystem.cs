using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Unity.Rendering;

namespace MyJob
{
	[UpdateAfter(typeof(MoveForwardSystem))]
	[UpdateAfter(typeof(TransformSystem))]
	[UpdateAfter(typeof(MeshInstanceRendererSystem))]
	public class LookatTargetSystem : JobComponentSystem
	{
		public struct LookatObjectGroup
		{
			[ReadOnly] public ComponentDataArray<Position> positions;
			[ReadOnly] public ComponentDataArray<Lookat> lookats;
			public ComponentDataArray<Heading> heading;
			public int Length;
		}

		public struct TargetObjectGroup
		{
			[ReadOnly] public ComponentDataArray<Position> positions;
			[ReadOnly] public ComponentDataArray<Target> targets;
			public int Length;
		}

		public struct LookatJob : IJobParallelFor
		{
			public LookatObjectGroup lookat_group;
			[ReadOnly] public TargetObjectGroup target_group;
			[ReadOnly] public float dt;

			public void Execute(int i)
			{
				float3 pos = lookat_group.positions[i].Value;
				float3 tpos = new float3(0F,0F,0F);
				int target_id = lookat_group.lookats[i].targetId;
				
				for (int j = 0; j < target_group.Length; j++)
				{
					if (target_id != target_group.targets[j].id) continue;
					tpos = target_group.positions[j].Value;
					break;
				}

				var forward = lookat_group.heading[i].Value;
				var direction = math.normalize(tpos - pos);
				forward += (direction - forward) * 1F * dt;
				lookat_group.heading[i] = new Heading(math.normalize(forward));
			}
		}

		[Inject] LookatObjectGroup lookatGroup;
		[Inject, ReadOnly] TargetObjectGroup targetGroup;

		protected override JobHandle OnUpdate(JobHandle inputDeps)
		{
			if (targetGroup.Length <= 0) return inputDeps;

			var job = new LookatJob()
			{
				lookat_group = lookatGroup,
				target_group = targetGroup,
				dt = Time.deltaTime
			};
			return job.Schedule(lookatGroup.Length, 64, inputDeps);
		}
	}
}