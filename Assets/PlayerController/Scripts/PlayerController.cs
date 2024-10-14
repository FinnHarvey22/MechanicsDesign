using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent (typeof(CharacterMovement))]
public class PlayerController : MonoBehaviour
{
	private PlayerControls m_ActionMap;
	private CharacterMovement m_Movement;
	[SerializeField] private HealthComponent m_Health;
	private void Awake()
	{
		m_ActionMap = new PlayerControls();
		m_Movement = GetComponent<CharacterMovement>();

		if(m_Health != null)
		{
			m_Health.OnDamaged += Handle_HealthDamaged;
		}
	}

	private void OnEnable()
	{
		m_ActionMap.Enable();

		m_ActionMap.Default.MoveHoriz.performed += Handle_MovePerformed;
		m_ActionMap.Default.MoveHoriz.canceled += Handle_MoveCancelled;
		m_ActionMap.Default.Jump.performed += Handle_JumpPerformed;
		m_ActionMap.Default.Jump.canceled += Handle_JumpCancelled;

		m_ActionMap.Default.Shoot.performed += Handle_ShootPerformed;
		
	}

	private void OnDisable()
	{
		m_ActionMap.Disable();

		m_ActionMap.Default.MoveHoriz.performed -= Handle_MovePerformed;
		m_ActionMap.Default.MoveHoriz.canceled -= Handle_MoveCancelled;
		m_ActionMap.Default.Jump.performed -= Handle_JumpPerformed;
		m_ActionMap.Default.Jump.canceled -= Handle_JumpCancelled;

		m_ActionMap.Default.Shoot.performed -= Handle_ShootPerformed;

	}

	private void Handle_MovePerformed(InputAction.CallbackContext context)
	{
		m_Movement.SetInMove(context.ReadValue<float>());
	}
	private void Handle_MoveCancelled(InputAction.CallbackContext context)
	{
		m_Movement.SetInMove(0f);
	}

	private void Handle_JumpPerformed(InputAction.CallbackContext context)
	{
		m_Movement.StartJump();
	}
	private void Handle_JumpCancelled(InputAction.CallbackContext context)
	{
		m_Movement.StopJump();
	}

	private void Handle_HealthDamaged(float current, float max, float change)
	{
		Debug.Log("Damaged: " + change);
	}

	public void Init() 
	{
		Debug.Log("Initialized Player Controller");
	}

	private void Handle_ShootPerformed(InputAction.CallbackContext context)
	{

	}

	
}
