using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CharacterMovement : MonoBehaviour
{
	private Rigidbody2D m_RB;

	[SerializeField] private StatefulRaycastSensor2D m_GroundSensor;
	[SerializeField] private float m_MoveSpeed;
	[SerializeField] private float m_JumpStrength;

	private float m_InMove;
	private bool m_isMoving;
	private Coroutine m_CMove;

	[SerializeField] private float m_CoyoteTimer;
	[SerializeField] private float m_CoyoteThreshold;

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
		}
	}
	public void StartJump()
	{
		if (m_GroundSensor.HasDetectedHit() || m_CoyoteTimer > 0)
		{
			m_RB.AddForce(Vector2.up * m_JumpStrength, ForceMode2D.Impulse);
		}
	}
	public void StopJump() { }

	private void FixedUpdate()
	{
		m_RB.linearVelocityX = m_MoveSpeed * m_InMove;

		m_CoyoteTimer -= Time.fixedDeltaTime;
	}

	private void OnCollisionExit2D(Collision2D collision)
	{
		if (m_RB.linearVelocityY <= 0)
		{
			m_CoyoteTimer = m_CoyoteThreshold;
		}


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
