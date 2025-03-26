using AideTool;
using AideTool.ExtendedEditor;
using AideTool.Extensions;
using Code;
using Core.Input;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Persona
{
    public class PlayerBehaviour : MonoBehaviour
    {
        [SerializeField] private CharacterController m_controller;
        [Foldout("Variables"), SerializeField] private float m_speed;
        [SerializeField] private float m_rotationFactor;
        [SerializeField] private float m_jumpTime = 5f;
        [SerializeField] private float m_jumpHeight = 16f;

        private readonly Dictionary<GravityType, float> Gravities = new();

        private PlayerStates m_state;

        private Vector3 m_appliedMovement = Vector3.zero;
        public float AppliedX { get => m_appliedMovement.x; set => m_appliedMovement.x = value; }
        public float AppliedZ { get => m_appliedMovement.z; set => m_appliedMovement.z = value; }
        public float AppliedY { get => m_appliedMovement.y; set => m_appliedMovement.y = value; }

        private readonly InputAxis2D m_move = new();
        private readonly InputButton m_jump = new();

        public void OnMovement(InputAction.CallbackContext context) => m_move.SetValues(context);
        public void OnJump(InputAction.CallbackContext context) => m_jump.SetValues(context);

        private void Start()
        {
            SetGravities();   
        }

        private void SetGravities()
        {
            float timeToTop = m_jumpTime * 0.5f;
            float initialJumpForce = 2f * m_jumpHeight / timeToTop;

            Gravities[GravityType.InitialJump] = initialJumpForce;

            float jumpGravity = -2f * m_jumpHeight / Mathf.Pow(timeToTop, 2f);
            Gravities[GravityType.JumpGravity] = jumpGravity;
            Gravities[GravityType.FallGravity] = 2f * jumpGravity;
            Gravities[GravityType.MaxFallSpeed] = -20f;

            Aide.LogDictionary(Gravities);
        }

        private void Update()
        {
            CheckGrounded();
            CheckStartFalling();
            HandleInput();
            HandleMovement();
            HandleGravity();
        }

        private void CheckGrounded()
        {
            if (m_controller.isGrounded)
            {
                m_state = PlayerStates.Grounded;
                return;
            }

            m_state = PlayerStates.Falling;
        }

        private void CheckStartFalling()
        {
            if (m_state != PlayerStates.Jumping)
                return;

            if(AppliedY <= 0f)
                m_state = PlayerStates.Falling;
        }

        private void HandleInput()
        {
            AppliedZ = m_move.Y;

            if (m_move.IsHorizontalActive)
            {
                Quaternion currentRotation = Quaternion.LookRotation(transform.forward);
                Vector3 input = transform.right * m_move.X;

                Quaternion targetRotation = Quaternion.LookRotation(input);

                if(currentRotation != targetRotation)
                    transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, Time.deltaTime * m_rotationFactor);
            }

            if (JumpConditions)
                Jump();

            if (m_jump.Released)
                m_state = PlayerStates.Falling;
        }

        private bool JumpConditions
        {
            get
            {
                bool[] conditions =
                {
                    m_jump.Pressed,
                    m_state == PlayerStates.Grounded
                };

                return AideMath.AndCheck(conditions);
            }
        }

        private void HandleMovement()
        {
            Vector3 currVector = transform.rotation * m_appliedMovement;
            m_controller.Move(Time.deltaTime * m_speed * currVector);
        }

        private void Jump()
        {
            m_state = PlayerStates.Jumping;
            float previousYSpeed = AppliedY;
            float nextYSpeed = AppliedY + Gravities[GravityType.InitialJump];
            float avgYSpeed = (previousYSpeed + nextYSpeed) * 0.5f;
            AppliedY = avgYSpeed;
        }

        private void HandleGravity()
        {
            if (m_state == PlayerStates.Grounded)
            {
                AppliedY = 0f;
                return;
            }

            float factor = 0;

            if (m_state == PlayerStates.Jumping)
                factor = Gravities[GravityType.JumpGravity];

            if (m_state == PlayerStates.Falling)
                factor = Gravities[GravityType.FallGravity];

            float previousYSpeed = AppliedY;
            float nextYSpeed = AppliedY + (factor * Time.deltaTime);
            float avgYSpeed = (previousYSpeed + nextYSpeed) * 0.5f;
            AppliedY = Mathf.Clamp(avgYSpeed, Gravities[GravityType.MaxFallSpeed], -Gravities[GravityType.MaxFallSpeed]);
        }

        [ContextMenu(nameof(RefreshGravities))]
        private void RefreshGravities()
        {
            SetGravities();
        }
    }
}
