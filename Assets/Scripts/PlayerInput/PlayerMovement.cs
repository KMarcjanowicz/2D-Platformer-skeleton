using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;

public class PlayerMovement : MonoBehaviour
{
    //animator variable for changing in-component variables
    [SerializeField] private Animator Anim = null;

    // RigidBody2D component - responsible for physics for the player object
    [SerializeField] private Rigidbody2D Rigidbody2DComponent = null;

    // A mask determining what is ground to the character
    [SerializeField] private LayerMask WhatIsGround;

    // A position marking where to check if the player is grounded.
    [SerializeField] private Transform GroundCheck = null;

    // A position marking where to check if the player has hit the ceiling.
    [SerializeField] private Transform CeilingCheck = null;

    // A collider that will be disabled when crouching
    [SerializeField] private Collider2D CrouchDisableCollider = null;

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

    // air conctrol
    [SerializeField] bool AirControl = false;

    private bool FacingRight = true;

    private bool IsGrounded = true;
    private bool IsInAir = false;

    private bool IsCrouching = false;
    private bool WantsToCrouch = false;

    private bool IsJumping = false;
    private bool WantsToJump = false;

    private enum MovementState
    {
        Idle,
        Running,
        Jumping,
        Falling
    };

    private void Start()
    {
        Rigidbody2DComponent = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        IsGrounded = false;
        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        // This can be done using layers instead but Sample Assets will not overwrite your project settings.
        Collider2D[] groundColliders = Physics2D.OverlapCircleAll(GroundCheck.position, .2f, WhatIsGround); ;
        for (int i = 0; i < groundColliders.Length; i++)
        {
            if (groundColliders[i].gameObject != gameObject)
            {
                IsGrounded = true;
            }
        }

        if(WantsToCrouch)
        {
            if(IsGrounded)
            {
                IsCrouching = true;
            }   
        }
        else{
            if(IsGrounded)
            {
                // The player must crouch if a circlecast to the ceilingcheck position hits anything designated as ground
                // This can be done using layers instead but Sample Assets will not overwrite your project settings.
                // If the character has a ceiling preventing them from standing up, keep them crouching
                if (Physics2D.OverlapCircle(CeilingCheck.position, .5f, WhatIsGround))
                {
                    IsCrouching = true;
                }
                else
                {
                    IsCrouching = false;
                }
            }    
        }

        if (IsGrounded || AirControl) {
            if (IsCrouching) //&& is grounded for small jums wwhile crouch but it doesn't work when going from crouch -> stand
            {
                // Disable one of the colliders when crouching
                if (CrouchDisableCollider != null)
                {
                    CrouchDisableCollider.enabled = false;
                }
                // Move speed while crouching
                if (Rigidbody2DComponent.bodyType != RigidbodyType2D.Static) //if not dead
                    Rigidbody2DComponent.velocity = new Vector2(MoveVector.x * MoveSpeed * CrouchSpeed * Time.fixedDeltaTime, Rigidbody2DComponent.velocity.y);
            }
            else
            {
                // Enable the collider when not crouching
                if (CrouchDisableCollider != null)
                {
                    CrouchDisableCollider.enabled = true;
                }
                //Move speed while normal moving
                if(Rigidbody2DComponent.bodyType != RigidbodyType2D.Static) //if not dead
                    Rigidbody2DComponent.velocity = new Vector2(MoveVector.x * MoveSpeed * Time.fixedDeltaTime, Rigidbody2DComponent.velocity.y);
            }
        }

        UpdateAnimationState();
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

        // check if grounded
        if (IsGrounded)
        {       
            Rigidbody2DComponent.velocity = new Vector2(Rigidbody2DComponent.velocity.x, JumpForce);
            IsInAir = true;
            IsGrounded = false;
        }
        
    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Debug.Log("Crouch");
            WantsToCrouch = true;
        }
        if (context.canceled) {
            Debug.Log("Uncrouch");
            WantsToCrouch = false; 
        }
/*
 *              if (context.started)
                {
                    Debug.Log("Crouch");
                    IsCrouching = true;
                }
                if (context.canceled)
                {
                    Debug.Log("Uncrouch");

                    // This can be done using layers instead but Sample Assets will not overwrite your project settings.
                    // If the character has a ceiling preventing them from standing up, keep them crouching
                    if (!Physics2D.OverlapCircle(CeilingCheck.position, .2f, WhatIsGround))
                    {
                        IsCrouching = false;
                    }
                }
        */

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



    private void UpdateAnimationState()
    {
        MovementState Movement = MovementState.Idle;



        if(Rigidbody2DComponent.velocity.x > .1f)
        {
            Movement = MovementState.Running;
        }
        else if(Rigidbody2DComponent.velocity.x < -.1f)
        {
            Movement = MovementState.Running;
        }
        else
        {
            Movement = MovementState.Idle;
        }

        if(Rigidbody2DComponent.velocity.y > .1f)
        {
            Movement = MovementState.Jumping;
        }
        else if(Rigidbody2DComponent.velocity.y < -.1f)
        {
            Movement = MovementState.Falling;
        }
        Anim.SetInteger("state", (int)Movement);
    }
}
