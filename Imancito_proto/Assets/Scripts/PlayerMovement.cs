using UnityEngine;
using AideTool;
using AideTool.ExtendedEditor;
using AideTool.Extensions;
using Code;
using Core.Input;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class PlayerMovement : MonoBehaviour
{
    
    [Foldout("Componentes"),SerializeField] private CharacterController m_controller;
    
    [Foldout("Variables"),SerializeField] private float m_speed;
    [SerializeField] private float m_jumpForce;
    [SerializeField] private float m_gravity= -20f;

    private PlayerState m_playerState;



    private Vector3 m_appliedMovement = Vector3.zero;
    public float AppliedX { get => m_appliedMovement.x; set => m_appliedMovement.x = value; }
    public float AppliedZ { get => m_appliedMovement.z; set => m_appliedMovement.z = value; }
    public float AppliedY { get => m_appliedMovement.y; set => m_appliedMovement.y = value; }

    private readonly InputAxis2D m_move = new();

    public void OnMovement(InputAction.CallbackContext context) => m_move.SetValues(context);




    // Update is called once per frame
    void Update()
    {
        CheckIsGrounded();
        HandleInput();
        HandleGravity();
        HandleMovement();
        Debug.Log(m_controller.isGrounded);
    }


    private void HandleInput()
    {
        AppliedX = m_move.X;
        AppliedZ = m_move.Y;

        if (JumpConditions)
            Jump();
    }

    private void CheckIsGrounded()
    {
        if (m_controller.isGrounded)
        {
            m_playerState =PlayerState.Grounded;
            return;
        }
        m_playerState = PlayerState.Fall;
    }

    private void Jump()
    {
        AppliedY = m_jumpForce;
        m_playerState=PlayerState.Jump;
    }

   
    private void HandleMovement()
    {
        Vector3 currVector = m_appliedMovement;
        
            
        m_controller.Move(Time.deltaTime*m_speed*currVector);
    }

    private void HandleGravity()
    {
        if (m_playerState== PlayerState.Grounded)
        {
            AppliedY = -1f;
            return;
        }
            AppliedY += m_gravity * Time.deltaTime;
    }
    private bool JumpConditions
    {
        get
        {
            bool[] conditions =
            {
                    Input.GetKeyDown(KeyCode.Space),
                    m_playerState == PlayerState.Grounded
                };

            return AideMath.AndCheck(conditions);
        }
    }
}
