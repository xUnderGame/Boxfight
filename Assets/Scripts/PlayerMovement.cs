using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(PolygonCollider2D), typeof(Animator))]
[RequireComponent(typeof(MovementBehaviour), typeof(CooldownBehaviour))]
public class PlayerMovement : MonoBehaviour
{
    public float speed;
    public MovementBehaviour mov;
    private Vector2 movement = new();
    private Coroutine meleeCoroutine;
    private GameObject meleeAttack;
    private InventoryScriptable inv;
    [HideInInspector] public CooldownBehaviour cd;

    void Awake() 
    {
        meleeAttack = gameObject.transform.Find("Melee Area").gameObject;
        mov = GetComponent<MovementBehaviour>();
        inv = GetComponent<Player>().inv;
        cd = GetComponent<CooldownBehaviour>();
    }

    // Update is called once per frame
    void Update() { mov.Move(movement.x, movement.y, speed * GameManager.Instance.player.currentSpeed); }

    // Moves the player.
    private void OnMove(InputValue ctx) { movement = ctx.Get<Vector2>(); }

    // Makes the player dash forward.
    private void OnDash() { mov.Dash(movement.x, movement.y); }

    // Fires current weapon.
    private void OnFire()
    {
        if (inv.activeWeapon && inv.globalCanShoot) { 
            Vector2 dir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - inv.activeWeapon.transform.position;
            inv.activeWeapon.Shoot(dir);
        }
    }

    // Melee attack event
    private void OnMelee()
    {
        if (meleeCoroutine != null) { StopCoroutine(meleeCoroutine); meleeAttack.SetActive(false); }
        meleeCoroutine = StartCoroutine(MeleeAttack());
    }

    // Swaps the current weapon.
    private void OnSwapWeapon() { inv.SwapWeapon(this); }

    // Picks up the weapon on the floor and drops the current active weapon.
    private void OnInteractNearest()
    {
        if (!GameManager.Instance.nearestInteractable) return;

        // Pick up weapon
        if (GameManager.Instance.nearestInteractable.CompareTag("Weapon")) inv.PickupWeapon(); 
        
        // Interact with nearest
        else if (GameManager.Instance.nearestInteractable.TryGetComponent(out IInteractable interactable))
        interactable?.Interact(gameObject); 
    }

    // Melee attacks
    private IEnumerator MeleeAttack()
    {
        meleeAttack.SetActive(true);
        meleeAttack.GetComponent<Collider2D>().enabled = true;
        yield return new WaitForSeconds(0.2f);
        meleeAttack.SetActive(false);
    }

    // Interactables and damageables. (CHANGE .NAME CONDITION LATER, USED FOR TESTING RN)
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name != "NPC" && other.TryGetComponent(out IInteractable interactable)) interactable?.Interact(gameObject);
        // if (other.TryGetComponent(out IDamageable damageable)) damageable?.Hurt();
    }
}
