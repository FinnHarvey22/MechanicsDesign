using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private GameObject[] m_Goals;
    private float m_Speed = 5.0f;
	private int m_Index;
	private Vector3 m_TargetPos;

	void Start()
    {
		m_TargetPos = m_Goals[1].transform.position;
		Debug.Log(m_Goals[1].transform.position);
	}

    void Update()
	{
		transform.position = Vector3.MoveTowards(transform.position, m_TargetPos, m_Speed * BulletTime.deltaTime);


	}

	private void FixedUpdate()
	{


	}

	
	
	
	private void OnTriggerEnter2D(Collider2D collision)
	{
		Debug.Log(collision.gameObject);
		if (collision.gameObject.CompareTag("TargetPos"))
		{
			m_Index++;
			if (m_Index % 2 == 0)
			{
				m_TargetPos = m_Goals[1].transform.position;
			}
			else
			{
				m_TargetPos = m_Goals[0].transform.position;
			}

		}
	}
	private void OnCollisionEnter2D(Collision2D collision)
	{
		
	}
}





