using MyGame;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

[UpdateAfter(typeof(TransformSystem))]
[UpdateAfter(typeof(CharacterSystem))]
public class CameraSystem : JobComponentSystem
{
	struct CameraMatrixGroup
	{
		[ReadOnly] internal ComponentDataArray<Camera> cameras;
		internal ComponentDataArray<TransformMatrix> matrixs;
		[ReadOnly] internal int Length;
	}

	struct TargetMatrixGroup
	{
		[ReadOnly] internal ComponentDataArray<Target> targets;
		[ReadOnly] internal ComponentDataArray<TransformMatrix> matrixs;
		[ReadOnly] internal int Length;
	}

	[Inject] CameraMatrixGroup cameraMatrix;
	[Inject] TargetMatrixGroup targetMatrix;

	NativeList<TransformMatrix> cameraMatArray;
	NativeList<TransformMatrix> targetMatArray;

	protected override JobHandle OnUpdate(JobHandle inputDeps)
	{
		if (cameraMatrix.Length == 0 || targetMatrix.Length == 0) return inputDeps;

		cameraMatArray.ResizeUninitialized(cameraMatrix.Length);
		targetMatArray.ResizeUninitialized(targetMatrix.Length);
		var set_job = new CopyComponentDataArrayToNativeArray<TransformMatrix>
		{
			original = targetMatrix.matrixs,
			copy = targetMatArray
		};
		inputDeps = set_job.Schedule(cameraMatrix.Length, 64, inputDeps);

		var trans_job = new CameraMatrixJob
		{
			camera_mats = cameraMatrix.matrixs,
			cameras = cameraMatrix.cameras,
			target_mats = targetMatArray,
			targets = targetMatrix.targets
		};
		inputDeps = trans_job.Schedule(cameraMatrix.Length, 64, inputDeps);

		// var ser2_job = new SetMatrix2Job
		// {
		// 	camera_mat_array = cameraMatArray,
		// 	camera_mats = cameraMatrix.matrixs
		// };

		// inputDeps = ser2_job.Schedule(cameraMatrix.Length, 64, inputDeps);
		return inputDeps;
	}

	protected override void OnCreateManager(int capacity)
	{
		cameraMatArray = new NativeList<TransformMatrix>(Allocator.Persistent);
		targetMatArray = new NativeList<TransformMatrix>(Allocator.Persistent);
	}

	protected override void OnDestroyManager()
	{
		cameraMatArray.Dispose();
		targetMatArray.Dispose();
	}


	[ComputeJobOptimization]
	public struct SetMatrixJob : IJobParallelFor
	{
		[ReadOnly] internal ComponentDataArray<TransformMatrix> camera_mats;
		[ReadOnly] internal ComponentDataArray<TransformMatrix> target_mats;
		internal NativeArray<TransformMatrix> camera_mat_array;
		internal NativeArray<TransformMatrix> target_mat_array;

		public void Execute(int i)
		{
			camera_mat_array[i] = camera_mats[i];
			target_mat_array[i] = target_mats[i];
		}
	}

	[ComputeJobOptimization]
	public struct CameraMatrixJob : IJobParallelFor
	{
		internal ComponentDataArray<TransformMatrix> camera_mats;
		[ReadOnly] internal NativeArray<TransformMatrix> target_mats;
		[ReadOnly] internal ComponentDataArray<Camera> cameras;
		[ReadOnly] internal ComponentDataArray<Target> targets;

		public void Execute(int i)
		{
			var camera_target_id = cameras[i].target_id;
			var camera_mat = camera_mats[i];
			for (int j = 0; j < targets.Length; j++)
			{
				if (camera_target_id == targets[j].id)
				{
					// var new_camera_mat = math.mul(camera_mat.Value, target_mats[j].Value);//camera_mat.Value * target_mats[j].Value;
					camera_mats[i] = new TransformMatrix { Value = math.mul(camera_mat.Value, target_mats[j].Value) };
					return;
				}
			}
		}
	}
}