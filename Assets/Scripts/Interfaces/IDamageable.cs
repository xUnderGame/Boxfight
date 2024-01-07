using UnityEngine;

public interface IDamageable
{
    public void Hurt(int damage, GameObject damageSource);
    public void Kill();
}
