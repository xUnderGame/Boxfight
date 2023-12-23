using UnityEngine;

[CreateAssetMenu(fileName = "Default Shotgun", menuName = "Weapon Scriptables/Shotgun")]
public class ShotgunScriptable : ScriptableObject
{
    public int energyCost;
    public int damage;
    public float firingSpeed;
    public int bulletsPerShot;
    public int bulletSpread;
    public Sprite weaponSprite;
    public GameObject projectile;
}
