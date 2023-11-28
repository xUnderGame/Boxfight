using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(PolygonCollider2D), typeof(Animator))]
public class PlayerMovement : MovementBehaviour
{
    public float speed;
    private Vector2 movement = new();

    // Update is called once per frame
    void Update() { Move(movement.x, movement.y, speed); }

    // Moves the player.
    private void OnMove(InputValue ctx) { movement = ctx.Get<Vector2>(); }

    // Makes the player dash forward.
    private void OnDash() { Dash(movement.x, movement.y); }
}
