using UnityEngine;

public class EnemyMelee : Enemy
{
    private Rigidbody2D rb;
    
    public override void OnEnable()
    {
        rb = GetComponent<Rigidbody2D>();
        base.OnEnable();
    }

    private void FixedUpdate()
    {
        Vector2 whereToMove = transform.position - GameManager.Instance.playerObject.transform.position;
        mov.Move(-whereToMove.x, -whereToMove.y, currentSpeed);
    }

    // Damage player
    public void OnTriggerEnter2D(Collider2D collider)
    {
        if (!collider.CompareTag("Player")) return;

        // Damage character
        if (collider.TryGetComponent(out IDamageable damageable)) damageable?.Hurt(currentDmg, gameObject);

        // Move away a little after attacking
        rb.AddForce((transform.position - collider.transform.position).normalized * 40f, ForceMode2D.Impulse);

    }
}
