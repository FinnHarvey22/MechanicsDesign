using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CharacterMovement : MonoBehaviour
{
	private Rigidbody2D m_RB;

	[SerializeField] private StatefulRaycastSensor2D m_GroundSensor;
	[SerializeField] private float m_MoveSpeed;
	[SerializeField] private float m_JumpStrength;

	[SerializeField] private CapsuleCollider2D m_ColliderCapsule;
	private Coroutine m_CNudge;
	private Vector2 m_OverlapPoint;

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
			m_ColliderCapsule.size = new Vector2(m_ColliderCapsule.size.x, 1.0f);
			
			
		}
		m_ColliderCapsule.size = new Vector2(m_ColliderCapsule.size.x, 2.0f);
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
		m_OverlapPoint =  collision.ClosestPoint(this.transform.position);
		Debug.Log(m_OverlapPoint.ToString());
		
		//m_RB.AddForce(m_OverlapPoint * -0.5f, ForceMode2D.Impulse);
	}
	




	//Student Work!
	IEnumerator HeadNudging()
	{
		while (m_RB.linearVelocityY > 0)
		{
			yield return new WaitForFixedUpdate();
			m_ColliderCapsule.size = new Vector2(0.5f, m_ColliderCapsule.size.y);
			
		}
		while (m_RB.linearVelocityY <= 0)
		{
			yield return new WaitForFixedUpdate();
			m_ColliderCapsule.size = new Vector2(1.5f, m_ColliderCapsule.size.y);
		}
		m_ColliderCapsule.size = new Vector2(1.0f, m_ColliderCapsule.size.y);

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
		while (m_RB.linearVelocityY > 2)
		{
			yield return new WaitForFixedUpdate();
			m_RB.gravityScale = 1.0f;
		}
		while (m_RB.linearVelocityY > -2)
		{
			yield return new WaitForFixedUpdate();
			m_RB.gravityScale = 0.3f;
		}
		while (m_RB.linearVelocityY <= -0.5f)
		{
			yield return new WaitForFixedUpdate();
			m_RB.gravityScale = 1.0f;
			m_MoveSpeed = 6f;
		}

		m_MoveSpeed = 5.0f;
	}
	
	[SerializeField] private DesignPatterns_ObjectPooler m_ObjectPooler;

	public void Shoot()
	{
		GameObject bullet = m_ObjectPooler.GetPooledObject("Bullet");
	}


	// TODO: add jump buffering, apex speed reduction thing (forgot proper name????), head nudging,step ups, improve coyote time system

	//Jump buffering - Press space if !grounded -> start buffer timer -> if grounded & buffertimer > 0 -> jump 

	//apex speed reduction - if !grounded & velocity.y < [speed value] -> set gravity scale to [lower Gravity Scale] 


	/*
	JUMP BUFFERING

	Jump Pressed
	if (!grounded) 
	{
	start buffering timer
	}

	grounded event 
	{
	if (buffer timer > 0){ jump}
	}



	
	*/

	/*
	 APEX SPEED REDUCTION

	 Not grounded coroutine

	while (velocity.y > [speed value])
	{
	going up
	}
	while (velocity.y < [speed value])
	{
	set gravity lower
	}
	while (velocity.y > [minus speed value])
	{
	 set gravity to normal
	increase air control
	}
	*/

	/*
	HEAD NUDGING

	Jump Function
	{
	start nudging coroutine
	}

	nudging coroutine
	{
	if (velocity.x != 0)
		{
		collision box size.x = 0.5
		if (velocity.y != 0)
			{
			collision box size.y = 1
			}
		}
		else { collision box size.y = 2 }
	}
	else
	{
	collision box size.x = 1

		

	
	
	*/


}
