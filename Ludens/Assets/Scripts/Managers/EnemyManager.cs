using System.Collections.Generic;
using CharacterSystem.EnemySystem;
using CharacterSystem.PlayerSystem;
using Containers;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace Managers
{
	public class EnemyManager : MonoBehaviour
	{
		public static EnemyManager Instance { get; private set; }
		private List<Enemy> enemies = new List<Enemy>();
		private List<EnemyMovementController> movementControllers = new List<EnemyMovementController>();
		private List<EnemyAttackController> attackControllers = new List<EnemyAttackController>();

		private NativeArray<Vector3> positions;
		private NativeArray<Quaternion> rotations;
		private NativeArray<float> moveSpeeds;
		private NativeArray<float> rotationSpeeds;

		private NativeArray<float> attackRanges;
		private NativeArray<float> damages;
		private NativeArray<int> attackResults;

		private Player player;

		private bool isGameOver = false;

		private void Awake()
		{
			if (Instance != null && Instance != this) Destroy(gameObject);
			else Instance = this;
		}

		private void Start()
		{
			player = Player.Instance;
		}

		private void OnEnable()
		{
			GameStateManager.OnStateChanged += HandleGameStateChanged;
		}

		private void OnDisable()
		{
			GameStateManager.OnStateChanged -= HandleGameStateChanged;
		}

		private void HandleGameStateChanged(GameState newState)
		{
			if (newState == GameState.Lose)
			{
				isGameOver = true;
			}
		}

		public void RegisterEnemy(EnemyMovementController movement, EnemyAttackController attack, Enemy enemy)
		{
			movementControllers.Add(movement);
			attackControllers.Add(attack);
			enemies.Add(enemy);

			ReallocateNativeArrays();

			movement.Initialize(enemy, movementControllers.Count - 1);
			attack.Initialize(enemy, enemy.Stats.attackRange, enemy.Stats.damage);
		}

		public void UnregisterEnemy(EnemyMovementController movement, EnemyAttackController attack, Enemy enemy)
		{
			int index = movementControllers.IndexOf(movement);
			if (index >= 0)
			{
				movementControllers.RemoveAt(index);
				attackControllers.RemoveAt(index);
				enemies.Remove(enemy);

				ReallocateNativeArrays();
				
				if (enemies.Count == 0 && !isGameOver)
				{
					GameManager.Instance.gameStateManager.CurrentState = GameState.Win;
					Debug.Log("Win");
				}
			}
		}

		private void ReallocateNativeArrays()
		{
			DisposeArrays();

			int count = movementControllers.Count;

			if (count == 0) return;

			positions = new NativeArray<Vector3>(count, Allocator.Persistent);
			rotations = new NativeArray<Quaternion>(count, Allocator.Persistent);
			moveSpeeds = new NativeArray<float>(count, Allocator.Persistent);
			rotationSpeeds = new NativeArray<float>(count, Allocator.Persistent);

			attackRanges = new NativeArray<float>(count, Allocator.Persistent);
			damages = new NativeArray<float>(count, Allocator.Persistent);
			attackResults = new NativeArray<int>(count, Allocator.Persistent);

			for (int i = 0; i < count; i++)
			{
				positions[i] = movementControllers[i].transform.position;
				rotations[i] = movementControllers[i].transform.rotation;
				moveSpeeds[i] = movementControllers[i].MoveSpeed;
				rotationSpeeds[i] = movementControllers[i].RotationSpeed;

				attackRanges[i] = attackControllers[i].AttackRange;
				damages[i] = attackControllers[i].Damage;
			}
		}

		private void Update()
		{
			if (isGameOver) return;
			if (GameManager.Instance.gameStateManager.CurrentState != GameState.Start) return;
			if (movementControllers.Count == 0 || player == null) return;

			for (int i = 0; i < movementControllers.Count; i++)
			{
				positions[i] = movementControllers[i].transform.position;
				rotations[i] = movementControllers[i].transform.rotation;
			}

			Vector3 playerPos = player.transform.position;

			var moveJob = new EnemyFollowJob
			{
				positions = positions,
				rotations = rotations,
				moveSpeeds = moveSpeeds,
				rotationSpeeds = rotationSpeeds,
				target = playerPos,
				deltaTime = Time.deltaTime
			};

			JobHandle moveHandle = moveJob.Schedule(movementControllers.Count, 64);

			var attackJob = new EnemyAttackJob { positions = positions, attackRanges = attackRanges, results = attackResults, target = playerPos };

			JobHandle attackHandle = attackJob.Schedule(movementControllers.Count, 64, moveHandle);
			attackHandle.Complete();

			for (int i = 0; i < movementControllers.Count; i++)
			{
				movementControllers[i].transform.position = positions[i];
				movementControllers[i].transform.rotation = rotations[i];

				if (attackResults[i] == 1)
				{
					attackControllers[i].OnAttack(player);
					attackResults[i] = 0;
				}
			}
		}

		private void DisposeArrays()
		{
			if (positions.IsCreated) positions.Dispose();
			if (rotations.IsCreated) rotations.Dispose();
			if (moveSpeeds.IsCreated) moveSpeeds.Dispose();
			if (rotationSpeeds.IsCreated) rotationSpeeds.Dispose();

			if (attackRanges.IsCreated) attackRanges.Dispose();
			if (damages.IsCreated) damages.Dispose();
			if (attackResults.IsCreated) attackResults.Dispose();
		}

		private void OnDestroy()
		{
			DisposeArrays();
		}
	}

	[BurstCompile]
	public struct EnemyAttackJob : IJobParallelFor
	{
		[ReadOnly] public NativeArray<Vector3> positions;
		[ReadOnly] public NativeArray<float> attackRanges;

		public NativeArray<int> results;

		public Vector3 target;

		public void Execute(int index)
		{
			float sqrDistance = (target - positions[index]).sqrMagnitude;
			float range = attackRanges[index];

			results[index] = (sqrDistance <= range * range) ? 1 : 0;
		}
	}

	[BurstCompile]
	public struct EnemyFollowJob : IJobParallelFor
	{
		public NativeArray<Vector3> positions;
		public NativeArray<Quaternion> rotations;

		[ReadOnly] public NativeArray<float> moveSpeeds;
		[ReadOnly] public NativeArray<float> rotationSpeeds;

		[ReadOnly] public Vector3 target;
		[ReadOnly] public float deltaTime;

		public void Execute(int index)
		{
			Vector3 pos = positions[index];
			Vector3 dir = (target - pos).normalized;

			// Hareket
			pos += dir * moveSpeeds[index] * deltaTime;

			// Rotasyon
			if (dir.sqrMagnitude > 0.001f)
			{
				float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
				Quaternion targetRot = Quaternion.Euler(0, 0, angle);
				rotations[index] = Quaternion.Slerp(rotations[index], targetRot, rotationSpeeds[index] * deltaTime);
			}

			positions[index] = pos;
		}
	}
}