using UnityEngine;
using UnityEngine.Audio;

public class EnemyCreeper : Enemy
{
    private AudioSource audioSource;

    public override void OnEnable()
    {
        base.OnEnable();
        audioSource = GetComponent<AudioSource>();
    }

    private void FixedUpdate()
    {
        Vector2 whereToMove = transform.position - GameManager.Instance.playerObject.transform.position;
        mov.Move(-whereToMove.x, -whereToMove.y, currentSpeed);
    }

    // Explode and shove enemy
    public void OnTriggerEnter2D(Collider2D collider)
    {
        if (!collider.CompareTag("Player")) return;

        // Damage character
        if (collider.TryGetComponent(out IDamageable damageable)) damageable?.Hurt(currentDmg, gameObject);
        
        // Shove character
        collider.GetComponent<Rigidbody2D>().AddForce((transform.position - collider.transform.position).normalized * -35f, ForceMode2D.Impulse);
        
        // Play audiosource and die
        if (gameObject.activeSelf) audioSource.PlayOneShot(audioSource.clip);
        anim.Bom();
    }
}
