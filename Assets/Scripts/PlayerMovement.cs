using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(PolygonCollider2D), typeof(Animator))]
public class PlayerMovement : MovementBehaviour
{
    public float speed;

    void OnEnable() {
        GetComponent<PlayerInput>().actions["OnMove"].performed += ctx => OnMove();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        // Move(horizontal, vertical, speed);
        if (Input.GetKeyDown(KeyCode.Space)) Dash(horizontal, vertical);
    }

    // public void MovePlayer(InputAction.CallbackContext context) {
    //     Debug.Log($"{context.ReadValue<Vector2>().x}, {context.ReadValue<Vector2>().y}");
    //     Move(context.ReadValue<Vector2>().x, context.ReadValue<Vector2>().y, speed);
    // }

    public void OnMove() {
        Debug.Log("hi");
    }
}
