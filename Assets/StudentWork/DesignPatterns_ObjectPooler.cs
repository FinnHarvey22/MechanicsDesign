using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class DesignPatterns_ObjectPooler : MonoBehaviour
{
	[Serializable] 
	private struct PooledObjectData
	{
		public GameObject Prefab;
		public string Name;
		public int PoolSize;
		public bool CanGrow;
	}

	public event Action OnPoolCleanup;

	[SerializeField] private PooledObjectData[] m_ObjectData;
	private List<PooledObject>[] m_PooledObjects;
	private GameObject[] m_Pools;

	private void Awake()
	{
		int poolNum = m_ObjectData.Length;
		m_Pools = new GameObject[poolNum];
		m_PooledObjects = new List<PooledObject>[poolNum];

		for (int pooledIndex = 0; pooledIndex < poolNum; pooledIndex++)
		{
			GameObject Pool = new GameObject($"Pool: {m_ObjectData[pooledIndex].Name}");
			Pool.transform.parent = transform;
			m_Pools[pooledIndex] = Pool;
			m_PooledObjects[pooledIndex] = new List<PooledObject> ();
			for (int ObjectIndex = 0; ObjectIndex < m_ObjectData[pooledIndex].PoolSize; ObjectIndex++)
			{
				SpawnObject(pooledIndex, ObjectIndex);
			}
		}
	}

	public GameObject GetPooledObject(string name)
	{
		int poolCount = m_Pools.Length;
		int targetPool = -1;
		for (int poolIndex = 0; poolIndex < poolCount; poolIndex++)
		{
			if (m_Pools[poolIndex].name == $"Pool: {name}")
			{
				targetPool = poolIndex;
				break;
			}
		}
		Debug.Assert( targetPool >= 0, $"No pool for objects by the name of {name}" );

		int objectCount = m_PooledObjects[targetPool].Count;
		int targetObject = -1;

		for (int objectIndex = 0; objectIndex < objectCount; objectIndex++)
		{
			if (m_PooledObjects[targetPool][objectIndex] != null)
			{
				if (!m_PooledObjects[targetPool][objectIndex].m_Active)
				{
					targetObject = objectIndex;
				}
			}
			else
			{
				SpawnObject(targetPool, objectIndex);
				targetObject = objectIndex;
				break;
			}
		}

		if (targetObject == -1)
		{
			if (m_ObjectData[targetObject].CanGrow)
			{
				SpawnObject(targetPool, objectCount);
				targetObject = objectCount;
			}
			else
			{
				Debug.LogWarning($"No {name} objects left in the pool and no option for pool to grown, returning NULL");
				return null;
			}
		}

		PooledObject toReturn = m_PooledObjects[targetPool][targetObject];
		toReturn.m_Active = true;
		OnPoolCleanup += toReturn.RecycleSelf;
		return toReturn.gameObject;
	}

	public void RecycleObject(GameObject obj)
	{
		PooledObject pooledObj = obj.GetComponent<PooledObject>();
		Debug.Assert(pooledObj != null, $"Trying to recyle an object called {obj.name} that didn't come from the object pooler");
		RecycleObject(pooledObj);
	}

	public void RecycleObject(PooledObject poolRef)
	{
		poolRef.transform.SetParent(m_Pools[poolRef.m_PoolIndex].transform);
		poolRef.gameObject.SetActive(false);
		poolRef.m_Active = false;
		OnPoolCleanup -= poolRef.RecycleSelf;
	}
	private PooledObject SpawnObject(int pooledIndex, int ObjectIndex)
	{
		GameObject TempGO = Instantiate(m_ObjectData[pooledIndex].Prefab, m_Pools[pooledIndex].transform);
		PooledObject pooledRef = TempGO.AddComponent<PooledObject> ();
		TempGO.name = m_ObjectData[pooledIndex].Name;
		
		TempGO.SetActive(false);
		if (ObjectIndex >= m_PooledObjects[pooledIndex].Count)
		{
			m_PooledObjects[pooledIndex].Add(pooledRef);
		}
		else
		{
			m_PooledObjects[pooledIndex].Insert(ObjectIndex, pooledRef);
		}

		pooledRef.Init(pooledIndex, this);

		return pooledRef;


	}
}
