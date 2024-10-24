using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

[RequireComponent(typeof(Rigidbody2D))]
public class CharacterMovement : MonoBehaviour
{
	private Rigidbody2D m_RB;

	[SerializeField] private StatefulRaycastSensor2D m_GroundSensor;
	[SerializeField] private float m_MoveSpeed;
	[SerializeField] private float m_JumpStrength;

	[SerializeField] private BoxCollider2D m_ColliderBox;
	private Coroutine m_CNudge;
	private Vector3 m_OverlapPoint;

	private float m_InMove;
	private bool m_isMoving;
	private Coroutine m_CMove;

	[SerializeField] private float m_CoyoteTimer;
	[SerializeField] private float m_CoyoteThreshold;

	[SerializeField] private float m_JumpBufferingTimer;
	[SerializeField] private float m_JumpBufferingThreshold;
	private Coroutine m_CJumpBuffering;
	private bool m_WaitingToJump = false;

	private void Awake()
	{
		m_RB = GetComponent<Rigidbody2D>();
		Debug.Assert(m_GroundSensor != null);
	}

	public void SetInMove(float newMove)
	{
		m_InMove = newMove;

		if (m_InMove == 0)
		{
			m_isMoving = false;
		}
		else
		{
			m_isMoving = true;
			m_CMove = StartCoroutine(C_MoveUpdate());
		}
	}

	IEnumerator C_MoveUpdate()
	{
		while (m_isMoving)
		{
			yield return new WaitForFixedUpdate();
			m_RB.linearVelocityX = m_MoveSpeed * m_InMove;
			m_ColliderBox.size = new Vector2(m_ColliderBox.size.x, 1.0f);


		}
		m_ColliderBox.size = new Vector2(m_ColliderBox.size.x, 2.0f);
	}
	public void StartJump()
	{
		if (m_GroundSensor.HasDetectedHit() || m_CoyoteTimer > 0)
		{
			Jump();
			m_CNudge = StartCoroutine(HeadNudging());
		}
		else
		{
			m_JumpBufferingTimer = m_JumpBufferingThreshold;
			m_CJumpBuffering = StartCoroutine(JumpBuffering());
		}
	}

	public void Jump()
	{
		m_RB.AddForce(Vector2.up * m_JumpStrength, ForceMode2D.Impulse);
		StartCoroutine(AntiGravityApex());
	}
	public void StopJump() 
	{
		
		StopCoroutine(AntiGravityApex());
		//StopCoroutine(HeadNudging());
		StopCoroutine(JumpBuffering());
	}

	private void FixedUpdate()
	{
		m_RB.linearVelocityX = m_MoveSpeed * m_InMove;

		m_CoyoteTimer -= Time.fixedDeltaTime;

		m_JumpBufferingTimer -= Time.fixedDeltaTime;
	}

	private void OnCollisionExit2D(Collision2D collision)
	{
		if (m_RB.linearVelocityY <= 0)
		{
			m_CoyoteTimer = m_CoyoteThreshold;
		}

	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (!collision.CompareTag("SemiSolid"))
		{
			StartCoroutine(CollisionRepel(collision));
		}
	

		
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		StopCoroutine(CollisionRepel(collision));
	}





	//Student Work!
	IEnumerator HeadNudging()
	{
		while (m_RB.linearVelocityY > 0 )
		{
			yield return new WaitForFixedUpdate();
			m_ColliderBox.size = new Vector2(0.5f, m_ColliderBox.size.y);
			
		}
		while (m_RB.linearVelocityY < 0)
		{
			yield return new WaitForFixedUpdate();
			m_ColliderBox.size = new Vector2(1.5f, m_ColliderBox.size.y);
		}
		
		m_ColliderBox.size = new Vector2(1.0f, m_ColliderBox.size.y);

	}

	IEnumerator JumpBuffering()
	{
		m_WaitingToJump = true;
		while (m_JumpBufferingTimer > 0)
		{
			yield return new WaitForFixedUpdate();
			if (m_GroundSensor.HasDetectedHit() && m_WaitingToJump == true)
			{
				Jump();
				m_WaitingToJump = false;
			}
		}
	}

	IEnumerator AntiGravityApex()
	{
		while (m_RB.linearVelocityY > 5)
		{
			yield return new WaitForFixedUpdate();
			m_RB.gravityScale = 1.5f;
		}
		while (m_RB.linearVelocityY > 0)
		{
			yield return new WaitForFixedUpdate();
			m_RB.gravityScale = 0.75f;
		}
		while (m_RB.linearVelocityY <= -0.0f)
		{
			yield return new WaitForFixedUpdate();
			m_RB.gravityScale = 2.0f;
			m_MoveSpeed = 6f;
		}
		m_RB.gravityScale = 1.5f;
		m_MoveSpeed = 5.0f;
	}

	IEnumerator CollisionRepel(Collider2D Collision)
	{
		while (Collision != null)
		{
			if (Collision == null)
			{
				yield break;
			}
			yield return new WaitForFixedUpdate();
			Debug.Log("triggered");
			m_OverlapPoint = Collision.ClosestPoint(this.transform.position);
			Debug.Log(m_OverlapPoint);
			Vector3 Direction = (m_OverlapPoint - this.transform.position);
			Debug.DrawLine(this.transform.position, m_OverlapPoint, Color.red, Mathf.Infinity);
			m_RB.AddForceAtPosition(-(m_OverlapPoint - this.transform.position), this.transform.position, ForceMode2D.Impulse);
		}
	}
	
	[SerializeField] private DesignPatterns_ObjectPooler m_ObjectPooler;

	public void Shoot()
	{
		GameObject bullet = m_ObjectPooler.GetPooledObject("Bullet");
	}





}
