using UnityEngine;

public class Spike : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D collision)
	{
		/*IDamageable Squishything = collision.gameObject.GetComponent<IDamageable>();
		if (Squishything != null)
		{
			Squishything.ApplyDamage(100.01f, this);
		}*/

		Debug.Log("Hit a things");
		collision.gameObject.transform.root.GetComponentInChildren<IDamageable>()?.ApplyDamage(100.01f, this);
	}
}
