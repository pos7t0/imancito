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
    [SerializeField] private float m_jumpForce;
    [SerializeField] private float m_gravity= -20f;
    [SerializeField] public float m_magnetForce= 0;

    private PlayerState m_playerState;
    private float m_polarity=1f;

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
        
    }


    private void HandleInput()
    {
        AppliedX = m_move.X;
        AppliedZ = m_move.Y;

        if (JumpConditions)
            Jump();

        if (Input.GetKeyDown(KeyCode.Z))
            ChangePolarity();
    }

    private void CheckIsGrounded()
    {
        if(m_playerState!= PlayerState.Magnet)
        {
            if (m_controller.isGrounded)
            {
                m_playerState =PlayerState.Grounded;
                return;
            }
            m_playerState = PlayerState.Fall;
        }
        
    }

    private void Jump()
    {
        AppliedY = m_jumpForce;
        m_playerState=PlayerState.Jump;
    }

    private void ChangePolarity()
    {
        m_polarity = (m_polarity>0) ? -1 : 1;
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
            AppliedY = -0.5f;
            return;
        }

        if (m_playerState == PlayerState.Magnet)
        {
            Debug.Log(AppliedY);
            AppliedY += m_magnetForce*m_polarity *Time.deltaTime;
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
                    //m_playerState == PlayerState.Grounded
                    m_controller.isGrounded
                };

            return AideMath.AndCheck(conditions);
        }
    }


    public void MagnetState( float magnetForce)
    {
        m_playerState = PlayerState.Magnet;
        m_magnetForce = magnetForce;
    }
    public void MagnetQuit()
    {
        m_playerState = PlayerState.Fall;
        m_magnetForce = 0f;
    }

}
