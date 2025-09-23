using System;
using System.Collections.Generic;
using UnityEngine;

namespace Utilities
{
	public class ObjectPooler : MonoBehaviour
	{
		[Serializable]
		public class Pool
		{
			[Tooltip("Give a tag to the pool to call")]
			public string Tag;
			[Tooltip("Prefab of the object to be pooled")]
			public GameObject Prefab;
			[Tooltip("The size (count) of the pool")]
			public int Size;
		}

		[SerializeField] private List<Pool> pools = new List<Pool>();
		private Dictionary<string, Queue<GameObject>> poolDictionary = new Dictionary<string, Queue<GameObject>>();

		public static ObjectPooler Instance { get; private set; }

		private void Awake()
		{
			if (Instance != null && Instance != this)
			{
				Destroy(gameObject);
				return;
			}

			Instance = this;

			InitPool();
		}

		private void InitPool()
		{
			foreach (var pool in pools)
			{
				Queue<GameObject> objectPool = new Queue<GameObject>();

				for (int i = 0; i < pool.Size; i++)
				{
					GameObject obj = Instantiate(pool.Prefab, transform);
					obj.SetActive(false);
					objectPool.Enqueue(obj);
				}

				poolDictionary.Add(pool.Tag, objectPool);
			}
		}

		public GameObject Spawn(string tag, Vector3 position, Quaternion rotation)
		{
			if (!poolDictionary.ContainsKey(tag))
			{
				Debug.LogWarning($"Pool with tag {tag} doesn't exist!");
				return null;
			}

			var objectToSpawn = poolDictionary[tag].Dequeue();
			objectToSpawn.SetActive(true);
			objectToSpawn.transform.position = position;
			objectToSpawn.transform.rotation = rotation;

			poolDictionary[tag].Enqueue(objectToSpawn);

			return objectToSpawn;
		}

		public void ReturnToPool(string tag, GameObject obj)
		{
			obj.SetActive(false);
			obj.transform.SetParent(transform);
			if (poolDictionary.ContainsKey(tag))
				poolDictionary[tag].Enqueue(obj);
		}
	}
}