using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace MyGame
{
	public class CharacterSystem : JobComponentSystem
	{
		protected override JobHandle OnUpdate(JobHandle inputDeps)
		{

			return inputDeps;
		}
	}

	public class PlayerSystem : JobComponentSystem
	{
		internal struct PlayerMoveGroup
		{
			[ReadOnly] internal ComponentDataArray<Player> players;
			[ReadOnly] internal ComponentDataArray<MoveSpeed> moveSpeeds;
			internal ComponentDataArray<Rotation> rotations;
			internal ComponentDataArray<Position> poses;
			internal int Length;
		}

		[Inject] PlayerMoveGroup playerMove;

		protected override JobHandle OnUpdate(JobHandle inputDeps)
		{
			var input = new float3
			{
				x = Input.GetAxis("Horizontal"),
				y = Input.GetAxis("Length"),
				z = Input.GetAxis("Vertical"),
			};
			var job = new PlayerMoveJob
			{
				player_move = playerMove,
				dt = Time.deltaTime,
				input = input
			};
			inputDeps = job.Schedule(playerMove.Length, 64, inputDeps);
			var rot_job = new PlayerRotJob
			{
				player_move = playerMove,
				dt = Time.deltaTime,
				input = new float2(Input.GetAxis("Mouse X"),Input.GetAxis("Mouse Y")),
			};
			inputDeps = rot_job.Schedule(playerMove.Length, 64, inputDeps);
			return inputDeps;
		}


		struct PlayerMoveJob : IJobParallelFor
		{
			[ReadOnly] internal float dt;
			[ReadOnly] internal float3 input;

			internal PlayerMoveGroup player_move;

			public void Execute(int i)
			{
				var player_quaternion = player_move.rotations[i].Value;
				var player_up = math.up(player_quaternion);
				var player_forward = math.forward(player_quaternion);
				var player_right = math.cross(player_up, player_forward);

				var add_pos = input * player_move.moveSpeeds[i].speed * dt;
				var pos = player_move.poses[i];
				pos.Value += add_pos.x * player_right;
				pos.Value += add_pos.y * player_up;
				pos.Value += add_pos.z * player_forward;
				player_move.poses[i] = pos;
			}
		}

		struct PlayerRotJob : IJobParallelFor
		{
			[ReadOnly] internal float dt;
			[ReadOnly] internal float2 input;
			internal PlayerMoveGroup player_move; 

			public void Execute(int i)
			{
				var player_quaternion = player_move.rotations[i].Value;
				var player_up = math.up(player_quaternion);
				var player_forward = math.forward(player_quaternion);
				var player_right = math.cross(player_up, player_forward);

				var add_angle = new float3(input * player_move.moveSpeeds[i].speed * 0.05F * dt, 0F);
				var x_rot = math.axisAngle(math.up(), add_angle.x);
				var y_rot = math.axisAngle(player_right, -add_angle.y);
				var rot = math.mul(x_rot,y_rot);
				player_move.rotations[i] = new Rotation(math.mul(rot,player_move.rotations[i].Value));
				// rot.Value = math.mul(rot.Value,  math.axisAngle(math.up(), add_rot.y));
				// player_move.rotations[i] = rot;
			}
		}
	}
}