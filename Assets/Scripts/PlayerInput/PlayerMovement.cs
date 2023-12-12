using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    // RigidBody2D component - responsible for physics for th eplayer object
    private Rigidbody2D Rigidbody2DComponent = null;

    // responsible to display of play has jumped and is currenlty in air
    private bool IsInAir = false;

    // vector responsible for moving the player - add velocity to the vector direction
    private Vector2 MoveVector = Vector2.zero;

    private float MoveSpeed = 7.0f;



    private void Start()
    {
        Rigidbody2DComponent = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        Rigidbody2DComponent.velocity = new Vector2(MoveVector.x * MoveSpeed, Rigidbody2DComponent.velocity.y);
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            MoveVector = context.ReadValue<Vector2>();
        }
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

        Rigidbody2DComponent.velocity = new Vector2(Rigidbody2DComponent.velocity.x, 7.0f);
    }
}
