using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(PolygonCollider2D), typeof(Animator))]
[RequireComponent(typeof(MovementBehaviour), typeof(CooldownBehaviour))]
public class PlayerMovement : MonoBehaviour
{
    public float speed;
    public MovementBehaviour mov;
    private Vector2 movement = new();
    private GameObject meleeAttack;
    private InventoryScriptable inv;
    private Animator animator;
    [HideInInspector] public CooldownBehaviour cd;
    [HideInInspector] public bool canMeleeCD;

    void Awake() 
    {
        meleeAttack = gameObject.transform.Find("Melee Rotation").Find("Melee Area").gameObject;
        mov = GetComponent<MovementBehaviour>();
        inv = GetComponent<Player>().inv;
        cd = GetComponent<CooldownBehaviour>();
        animator = GetComponent<Animator>();
        canMeleeCD = true;
    }

    // Update is called once per frame
    void Update() {
        mov.Move(movement.x, movement.y, speed * GameManager.Instance.player.currentSpeed); 

        animator.SetFloat("X",movement.x);
        animator.SetFloat("Y",movement.y);
    }

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
        if (!inv.globalCanMelee || !canMeleeCD) return;
        StartCoroutine(cd.StartCooldown(0.4f, result => canMeleeCD = result, canMeleeCD));
        StartCoroutine(MeleeAttack());
    }

    // Swaps the current weapon.
    private void OnSwapWeapon() { inv.SwapWeapon(this); }

    // Picks up the weapon on the floor and drops the current active weapon.
    private void OnInteractNearest()
    {
        if (!GameManager.Instance.nearestInteractable) return;

        // Pick up weapon
        if (GameManager.Instance.nearestInteractable.CompareTag("Weapon")) {
            if (GameManager.Instance.nearestInteractable.transform.root != GameManager.Instance.pickupPool.transform) return;
            inv.PickupWeapon();
        }

        // Interact with nearest
        else if (GameManager.Instance.nearestInteractable.TryGetComponent(out IInteractable interactable))
            interactable?.Interact(gameObject); 
    }

    // Pauses the game
    private void OnPause()
    {
        if (GameManager.Instance.gameUI.gameOver.activeSelf) return;
        
        // Turn off
        if (GameManager.Instance.gameUI.pause.activeSelf)
        {
            GameManager.Instance.gameUI.pause.SetActive(false);
            Time.timeScale = 1;
            return;
        }
        
        // Turn on
        GameManager.Instance.gameUI.pause.SetActive(true);
        Time.timeScale = 0;
    }

    // Melee attacks
    private IEnumerator MeleeAttack()
    {
        meleeAttack.SetActive(true);
        meleeAttack.GetComponent<Collider2D>().enabled = true;

        yield return new WaitForSeconds(0.1f);
        meleeAttack.SetActive(false);
    }

    // Interactables and damageables. (CHANGE .NAME CONDITION LATER, USED FOR TESTING RN)
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name != "NPC" && other.TryGetComponent(out IInteractable interactable)) interactable?.Interact(gameObject);
        // if (other.TryGetComponent(out IDamageable damageable)) damageable?.Hurt();
    }
}
