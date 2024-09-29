using UnityEngine;
using UnityEngine.InputSystem;

public class TestInput : MonoBehaviour
{

    private TestInputAction m_ActionMap;
    private void Awake()
    {
        m_ActionMap = new TestInputAction();
    }
    private void OnEnable()
    {
        m_ActionMap.Enable();
        m_ActionMap.Test_Control_Map.Jump.performed += Handle_JumpPerformed;
        m_ActionMap.Test_Control_Map.Interact.performed += Handle_InteractionPerformed;
        m_ActionMap.Test_Control_Map.DoubleTapSlam.performed += Handle_SlamPerformed;
        m_ActionMap.Test_Control_Map.Move.performed += Handle_MovePerformed;
        m_ActionMap.Test_Control_Map.Move.canceled += Handle_MoveCancelled;
    }



    private void OnDisable()
    {
        

    }

    private void Handle_MovePerformed(InputAction.CallbackContext context)
    {
        Debug.Log("Move Performed, and the input is: " +context.ReadValue<float>());
    }
    private void Handle_MoveCancelled(InputAction.CallbackContext context)
    {
        Debug.Log("Move Cancelled, and the input is: " + context.ReadValue<float>());
    }
    private void Handle_JumpPerformed(InputAction.CallbackContext context)
    {
        Debug.Log("Jump Performed");
    }
    private void Handle_InteractionPerformed(InputAction.CallbackContext context)
    {
        Debug.Log("Interaction Performed");
    }
    private void Handle_SlamPerformed(InputAction.CallbackContext context)
    {
        Debug.Log("Slam Performed");
    }
}
