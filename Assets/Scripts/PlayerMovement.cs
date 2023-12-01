using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(PolygonCollider2D), typeof(Animator))]
[RequireComponent(typeof(MovementBehaviour), typeof(CooldownBehaviour))]
public class PlayerMovement : MonoBehaviour
{
    public float speed;
    private Vector2 movement = new();
    private MovementBehaviour mov;
    private InventoryScriptable inv;

    void Awake() 
    {
        mov = GetComponent<MovementBehaviour>();
        inv = GetComponent<Player>().inv;
    }

    // Update is called once per frame
    void Update() { mov.Move(movement.x, movement.y, speed); }

    // Moves the player.
    private void OnMove(InputValue ctx) { movement = ctx.Get<Vector2>(); }

    // Makes the player dash forward.
    private void OnDash() { mov.Dash(movement.x, movement.y); }

    private void OnFire() { inv.weapons[inv.selectedWeapon].Shoot(); }
}
