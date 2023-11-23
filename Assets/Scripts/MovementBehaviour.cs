using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementBehaviour : MonoBehaviour
{
    public Rigidbody2D rb;

    public void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Moves the current character.
    public void Move(float x, float y, float speed) { transform.Translate(speed * Time.deltaTime * new Vector2(x, y).normalized); }
    
    // Makes the character "dash forward.
    public void Dash(float x, float y, float force = 2f) {
        rb.AddForce(new Vector2(x, y) * force, ForceMode2D.Impulse);
    }
}
