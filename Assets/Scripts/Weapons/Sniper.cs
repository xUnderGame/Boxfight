using UnityEngine;

public class Sniper : Weapon
{
    public SniperScriptable ws;

    private int bulletPenetration;

    public override void Shoot(Vector2 direction)
    {
        if (!CanShoot()) return;


        // Defines the angle
        float ang = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg / 2;

        // Shoot the bullet
        GameObject tempBullet = Instantiate(projectile,
        gameObject.transform.position,
        Quaternion.Euler(new Vector3(0, 0, ang)),
        GameManager.Instance.bulletPool.transform);

        // Set projectile damage & add penetration
        SniperProjectile gunProjectile = tempBullet.GetComponent<SniperProjectile>();
        if (transform.parent.parent.CompareTag("Player")) gunProjectile.shotByPlayer = true;
        gunProjectile.bulletPenetration = bulletPenetration;
        gunProjectile.bulletDamage = damage;
        gunProjectile.ttl = ws.timeToLive;

        // Ignore collision
        Physics2D.IgnoreCollision(transform.parent.parent.GetComponent<Collider2D>(), tempBullet.GetComponent<Collider2D>());

        // Shove character backwards from where they aimed
        // transform.parent.parent.GetComponent<Rigidbody2D>().AddForce(
        //     (transform.parent.parent.position - gunProjectile.transform.position).normalized * -40f, ForceMode2D.Impulse);

        // Discount the player mana and start cooldown coroutine
        StartCoroutine(cd.StartCooldown(firingSpeed, result => canShoot = result, canShoot));
        if (transform.parent.parent.CompareTag("Player")) DiscountMana();
        audio.Play();
    }

    public override void LoadScriptable()
    {
        weaponSprite = ws.weaponSprite;
        energyCost = ws.energyCost;
        damage = ws.damage;
        firingSpeed = ws.firingSpeed;
        projectile = ws.projectile;
        bulletPenetration = ws.bulletPenetration;
        
        SetWeaponSprite(weaponSprite);
    }
}
