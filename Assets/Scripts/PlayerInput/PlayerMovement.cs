using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{                        
    // RigidBody2D component - responsible for physics for th eplayer object
    [SerializeField] private Rigidbody2D Rigidbody2DComponent = null;

    // A mask determining what is ground to the character
    [SerializeField] private LayerMask WhatIsGround;

    // A position marking where to check if the player is grounded.
    [SerializeField] private Transform GroundCheck = null;

    // A position marking where to check if the player has hit the ceiling.
    [SerializeField] private Transform CeilingCheck = null;

    // A collider that will be disabled when crouching
    [SerializeField] private Collider2D m_CrouchDisableCollider = null;

    // Amount of maxSpeed applied to crouching movement. 1 = 100%
    [Range(0, 1)][SerializeField] private float CrouchSpeed = .36f;

    // How much to smooth out the movement
    [Range(0, .3f)][SerializeField] private float MovementSmoothing = .05f;

    // Amount of force added when the player jumps.
    [SerializeField] private float JumpForce = 400f;

    // Amount of force added when the player moves.
    [SerializeField] private float MoveSpeed = 200.0f;

    // vector responsible for moving the player - add velocity to the vector direction
    private Vector2 MoveVector = Vector2.zero;

    private bool FacingRight = true;

    private bool IsGrounded = true;

    private bool IsCrouching = false;


    private void Start()
    {
        Rigidbody2DComponent = GetComponent<Rigidbody2D>();


    }

    private void FixedUpdate()
    {
        Rigidbody2DComponent.velocity = new Vector2(MoveVector.x * MoveSpeed * Time.fixedDeltaTime, Rigidbody2DComponent.velocity.y);
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            // Read Value from input
            MoveVector = context.ReadValue<Vector2>();

            // Flipping the character sprite if the direction of the image does now align with the movement direction
            if(FacingRight && MoveVector.x < 0)
            {
                Flip();
            }
            else if (!FacingRight && MoveVector.x > 0)
            {
                Flip();
            }
            
        }
        // if key is cancelled remove movement vector ( prevents infinite sliding )
        else if(context.canceled)
        {
            MoveVector = Vector2.zero;
        }
        else
        {
            return;
        }
        
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (!context.started) return;

        // check if grounded()
        Rigidbody2DComponent.velocity = new Vector2(Rigidbody2DComponent.velocity.x, JumpForce);
    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
        if (context.started) IsCrouching = true;
        if(context.canceled) IsCrouching = false;
    }

    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        FacingRight = !FacingRight;

        // Multiply the player's x local scale by -1.
        Vector3 TheScale = transform.localScale;
        TheScale.x *= -1;
        transform.localScale = TheScale;
    }
}
