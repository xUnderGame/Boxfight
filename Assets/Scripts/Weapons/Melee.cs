using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : MonoBehaviour
{
    public void Awake() { gameObject.SetActive(false); }

    // When Melee hits something...
    public void OnTriggerEnter2D(Collider2D hit)
    {
        if (hit.TryGetComponent(out IDamageable damageable)) damageable?.Hurt(GameManager.Instance.player.currentDmg);
    }
}
