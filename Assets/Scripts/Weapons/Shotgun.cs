using UnityEngine;

public class Shotgun : Weapon
{
    public ShotgunScriptable ws;
    private float bulletSpread;
    private float bulletsPerShot;

    public override void Shoot(Vector2 direction)
    {
        if (!CanShoot()) return;

        // Shoot the bullets!
        float offset = bulletSpread * -1;
        float addition = bulletSpread / bulletsPerShot * (bulletsPerShot / 2f);
        for (int i = 0; i < bulletsPerShot; i++)
        {
            // Defines the angle
            float ang = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg / 2) + offset;
            offset += addition;

            // Instantiates
            GameObject tempBullet = Instantiate(projectile,
            gameObject.transform.position,
            Quaternion.Euler(new Vector3(0, 0, ang)),
            GameManager.Instance.bulletPool.transform);

            // Set projectile damage
            ShotgunProjectile gunProjectile = tempBullet.GetComponent<ShotgunProjectile>();
            if (transform.parent.parent.CompareTag("Player")) gunProjectile.shotByPlayer = true;
            gunProjectile.bulletDamage = damage;
            gunProjectile.ttl = ws.timeToLive;

            // Ignores collision
            Physics2D.IgnoreCollision(transform.parent.parent.GetComponent<Collider2D>(), tempBullet.GetComponent<Collider2D>());
        }

        // Discount the player mana and start cooldown coroutine
        StartCoroutine(cd.StartCooldown(firingSpeed, result => canShoot = result, canShoot));
        if (transform.parent.parent.CompareTag("Player")) DiscountMana();
        
        // Play the weapon SFX
        audioSource.PlayOneShot(audioSource.clip);
    }

    public override void LoadScriptable()
    {
        weaponSprite = ws.weaponSprite;
        energyCost = ws.energyCost;
        damage = ws.damage;
        firingSpeed = ws.firingSpeed;
        bulletsPerShot = ws.bulletsPerShot;
        bulletSpread = ws.bulletSpread;
        projectile = ws.projectile;

        SetWeaponSprite(weaponSprite, ws.color);
    }
}
