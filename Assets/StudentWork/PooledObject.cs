using UnityEngine;

public class PooledObject : MonoBehaviour
{
	[HideInInspector] public int m_PoolIndex {  get; private set; }
	public bool m_Active;
	DesignPatterns_ObjectPooler m_Pooler;
	public void Init(int pooledIndex, DesignPatterns_ObjectPooler poolerRef)
	{
		m_PoolIndex = pooledIndex;
		m_Pooler = poolerRef;
		m_Active = false;
	}

	public void RecycleSelf()
	{
		m_Pooler.RecycleObject(this);
	}

	private void OnDestroy()
	{
		m_Pooler.OnPoolCleanup  -= this.RecycleSelf;
	}
}
