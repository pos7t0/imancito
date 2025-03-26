using UnityEngine;
using AideTool;
using AideTool.ExtendedEditor;
using AideTool.Extensions;
using Code;
using Core.Input;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    
    [Foldout("Componentes"),SerializeField] private CharacterController m_controller;
    [Foldout("Variables"),SerializeField] private float m_speed;
    [SerializeField] private float m_fall;
    
    

    private Vector3 m_appliedMovement = Vector3.zero;
    public float AppliedX { get => m_appliedMovement.x; set => m_appliedMovement.x = value; }
    public float AppliedZ { get => m_appliedMovement.z; set => m_appliedMovement.z = value; }
    public float AppliedY { get => m_appliedMovement.y; set => m_appliedMovement.y = value; }

    private readonly InputAxis2D m_move = new();

    public void OnMovement(InputAction.CallbackContext context) => m_move.SetValues(context);

    // Update is called once per frame
    void Update()
    {
        HandleInput();
        HandleMovement();
        CheckGravity();
    }

    private void HandleInput()
    {
        AppliedX = m_move.X;
        AppliedZ = m_move.Y;

        if (Input.GetKey(KeyCode.Space))
        {

        }
            
    }

    private void CheckGravity()
    {
        if (m_controller.isGrounded)
        {
            AppliedY = 0f;
            return;
        }
            


        float previousYSpeed = AppliedY;
        AppliedY = previousYSpeed + m_fall;

    }
    private void HandleMovement()
    {
        Vector3 currVector = m_appliedMovement;
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"),1, Input.GetAxis("Vertical")) ;

        m_controller.Move(Time.deltaTime*m_speed*currVector);
    }
}
