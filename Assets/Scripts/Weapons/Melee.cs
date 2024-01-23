using UnityEngine;

public class Melee : MonoBehaviour
{
    private readonly float shoveForce = -48; // Keep it negative, it SHOVES, not attracts.
    private Transform rotationReference;

    public void Start()
    {
        rotationReference = GameManager.Instance.playerObject.transform.Find("Melee Rotation");
        gameObject.SetActive(false);
    }

    public void FixedUpdate() { RotateAttackArea(); }

    // Rotates the melee area around the player using the mouse position (this doesn't work but it looks so funny LMAO (nvm fixed it :<))
    private void RotateAttackArea()
    {
        Vector3 lookDir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - GameManager.Instance.playerObject.transform.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        rotationReference.rotation = Quaternion.Euler(0, 0, angle - 90);
    }

    // When Melee hits something...
    public void OnTriggerStay2D(Collider2D hit)
    {
        // Hit!
        if (hit.TryGetComponent(out IDamageable damageable)) damageable?.Hurt(GameManager.Instance.player.currentDmg, gameObject);

        // Shove enemy
        if (hit.TryGetComponent(out Rigidbody2D shove)) {
            shove.AddForce((transform.position - hit.transform.position).normalized * shoveForce, ForceMode2D.Impulse);
        }
        
        // Disable collider after hit
        GetComponent<Collider2D>().enabled = false;
    }
}
