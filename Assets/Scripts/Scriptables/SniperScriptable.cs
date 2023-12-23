using UnityEngine;

[CreateAssetMenu(fileName = "Default Sniper", menuName = "Weapon Scriptables/Sniper")]
public class SniperScriptable : ScriptableObject
{
    public int energyCost;
    public int damage;
    public float firingSpeed;
    public int bulletPenetration;
    public Sprite weaponSprite;
    public GameObject projectile;
}
