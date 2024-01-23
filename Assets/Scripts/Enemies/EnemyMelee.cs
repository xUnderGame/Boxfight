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
        //Vector2 whereToMove = transform.position - GameManager.Instance.playerObject.transform.position;
        //mov.Move(-whereToMove.x, -whereToMove.y, currentSpeed);
        mov.Move(1, 0, currentSpeed);
        LookAtPlayer();
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

    private void LookAtPlayer()
    {
        Vector2 directionToPlayer = GameManager.Instance.playerObject.transform.position - transform.position;

        // Calcula el ángulo en radianes y conviértelo a grados
        float angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;

        // Crea una rotación basada en el ángulo
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        // Aplica la rotación al objeto
        transform.rotation = rotation;
    }

}
