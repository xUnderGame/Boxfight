using UnityEngine;

[CreateAssetMenu(fileName = "Default Shotgun", menuName = "Weapon Scriptables/Shotgun")]
public class ShotgunScriptable : ScriptableObject
{
    public int energyCost;
    public int damage;
    public float firingSpeed;
    public float timeToLive;
    public Sprite weaponSprite;
    public GameObject projectile;
    public int bulletsPerShot;
    public int bulletSpread;
}
