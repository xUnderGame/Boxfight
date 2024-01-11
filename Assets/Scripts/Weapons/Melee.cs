using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : MonoBehaviour
{
    private readonly float shoveForce = -28; // Keep it negative, it SHOVES, not attracts.

    public void Awake() { gameObject.SetActive(false); }

    public void FixedUpdate() { RotateAttackArea(); }

    // Rotates the melee area around the player using the mouse position (this doesn't work but it looks so funny LMAO)
    private void RotateAttackArea()
    {
        Vector3 player = Camera.main.WorldToScreenPoint(GameManager.Instance.playerObject.transform.position);
        player = Input.mousePosition - player;
        float angle = Mathf.Atan2(player.y, player.x) * Mathf.Rad2Deg;

        transform.Rotate(Vector3.forward * angle);
    }

    // When Melee hits something...
    public void OnTriggerStay2D(Collider2D hit)
    {
        // Hit!
        if (hit.TryGetComponent(out IDamageable damageable)) damageable?.Hurt(GameManager.Instance.player.currentDmg, gameObject);

        // Shove enemy
        if (hit.TryGetComponent(out Rigidbody2D shove)) {
            shove.AddForce((transform.position - hit.transform.position).normalized * shoveForce, ForceMode2D.Impulse);
        }
        
        // Disable collider after hit
        GetComponent<Collider2D>().enabled = false;
    }
}
