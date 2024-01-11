using UnityEngine;

[CreateAssetMenu(fileName = "Default Launcher", menuName = "Weapon Scriptables/Launcher")]
public class LauncherScriptable : ScriptableObject
{
    public int energyCost;
    public int damage;
    public float firingSpeed;
    public Sprite weaponSprite;
    public GameObject projectile;
    public float shoveForce;
}
