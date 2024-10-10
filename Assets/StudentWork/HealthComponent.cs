using System;
using Unity.Mathematics;
using UnityEngine;

public class HealthComponent : MonoBehaviour, IDamageable
{
	/// <summary>
	/// args = current, max, change
	/// </summary>
	public event Action<float, float, float> OnDamaged;
	public event Action<MonoBehaviour> OnDead;

	[SerializeField] private float m_MaxHealth;
	private float m_CurrentHealth;

	private void Awake()
	{
		m_CurrentHealth = m_MaxHealth;
	}
	public void ApplyDamage(float Damage, MonoBehaviour Causer)
	{
		float change = Mathf.Min(m_CurrentHealth, Damage);
		m_CurrentHealth -= change;

		OnDamaged?.Invoke(m_CurrentHealth, m_MaxHealth, change);
		if(m_CurrentHealth == 0.0f)
		{
		OnDead?.Invoke(Causer);
		}
	}

}




