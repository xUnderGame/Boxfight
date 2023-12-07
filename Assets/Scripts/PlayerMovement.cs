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
    void Update() { mov.Move(movement.x, movement.y, speed * GameManager.Instance.player.currentSpeed); }

    // Moves the player.
    private void OnMove(InputValue ctx) { movement = ctx.Get<Vector2>(); }

    // Makes the player dash forward.
    private void OnDash() { mov.Dash(movement.x, movement.y); }

    // Fires current weapon.
    private void OnFire() { inv.activeWeapon.Shoot(); }

    // Swaps the current weapon.
    private void OnSwapSwapon() { inv.SwapWeapon(); }

    // Interactables and damageables.
    public void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"hii {other.name}!");
        if (other.TryGetComponent(out IInteractable interactable)) interactable?.Interact();
        // if (other.TryGetComponent(out IDamageable damageable)) damageable?.Hurt();
    }
}
