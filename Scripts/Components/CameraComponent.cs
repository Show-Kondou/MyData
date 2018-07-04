using Unity.Entities;

namespace MyGame
{
    [System.Serializable]
	public struct Camera :IComponentData
	{
		public int target_id;

		public Camera(int _target_id = 0)
		{
			target_id = _target_id;
		}
	}
	
}
public class CameraComponent : ComponentDataWrapper<MyGame.Camera> {}