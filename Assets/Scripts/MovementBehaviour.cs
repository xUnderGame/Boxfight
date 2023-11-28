using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MovementBehaviour : MonoBehaviour
{
    public Rigidbody2D rb;
    public Renderer rd;

    public void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rd = GetComponent<Renderer>();
    }

    // Moves the current character.
    public void Move(float x, float y, float speed) { transform.Translate(speed * Time.deltaTime * new Vector2(x, y).normalized); }
    
    // Makes the character "dash" forward.
    public void Dash(float x, float y, float force = 15f, bool doIframes = true) {
        rb.AddForce(new Vector2(x, y) * force, ForceMode2D.Impulse);
        if (doIframes) StartCoroutine(DashIframes());
    }

    // Changes color of the character renderer and gives some iframes (not yet implemented)
    public IEnumerator DashIframes(float time = 0.3f) {
        rd.material.color = new Color(0.5f, 0.5f, 0.5f);
        yield return new WaitForSeconds(time);
        rd.material.color = new Color(1f, 1f, 1f);
    }
}
