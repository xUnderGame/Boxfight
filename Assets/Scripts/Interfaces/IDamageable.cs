using UnityEngine;

public interface IDamageable
{
    public void Hurt(float damage, GameObject damageSource);
    public void Kill();
}
