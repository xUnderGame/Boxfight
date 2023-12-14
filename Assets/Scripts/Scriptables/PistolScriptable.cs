using UnityEngine;

[CreateAssetMenu(fileName = "DefaultPistol", menuName = "Weapon Scriptables/Pistol")]
public class PistolScriptable : ScriptableObject
{
    public int energyCost;
    public float damage;
    public float firingSpeed;
    public Sprite weaponSprite;
    public GameObject projectile;
}
