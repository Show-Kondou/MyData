using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;

public struct CopyComponentDataArrayToNativeArray<T> : IJobParallelFor
	where T : struct, IComponentData
{
	[ReadOnly] internal ComponentDataArray<T> original;
	[WriteOnly] internal NativeArray<T> copy;

	void IJobParallelFor.Execute(int index)
	{
		copy[index] = original[index];
	}
}