using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public PlayerInput input;

    Rigidbody rb;

    public int speed;
    public float drag;

    public Vector2 move;

    void Start()
    {
        input = new PlayerInput();
        input.Player.Enable();
        input.Player.Movement.performed += OnMovePerformed;
        input.Player.Movement.canceled += OnMoveCancelled;

        rb = GetComponent<Rigidbody>();
    }

    void OnMovePerformed(InputAction.CallbackContext value)
    {
        move = value.ReadValue<Vector2>();
    }

    void OnMoveCancelled(InputAction.CallbackContext value)
    {
        move = value.ReadValue<Vector2>();
    }

    void Update()
    {
        SpeedControl();
    }

    private void FixedUpdate()
    {
        Movement();
    }

    void Movement()
    {
        rb.AddForce(move.y * speed * transform.forward, ForceMode.Force);
        rb.AddForce(move.x * speed * transform.right, ForceMode.Force);

        rb.drag = drag;
    }

    void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0, rb.velocity.z);

        if(flatVel.magnitude > speed)
        {
            Vector3 limitedVel = flatVel.normalized * speed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }
}
