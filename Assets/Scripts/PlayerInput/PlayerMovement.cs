using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    // RigidBody2D component - responsible for physics for th eplayer object
    private Rigidbody2D Rigidbody2DComponent;

    // responsible to display of play has jumped and is currenlty in air
    private bool IsInAir = false;


    private void Start()
    {
        Rigidbody2DComponent = GetComponent<Rigidbody2D>();
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        if (!context.started) return;

        Debug.Log(context.control);
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (!context.started) return;

        GetComponent<Rigidbody2D>().velocity = new Vector3(0, 7.0f, 0);
    }
}
