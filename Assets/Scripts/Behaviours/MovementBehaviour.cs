using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CooldownBehaviour))]
public class MovementBehaviour : MonoBehaviour
{
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public Renderer rd;
    [HideInInspector] public CooldownBehaviour cd;
    [HideInInspector] public bool canMove;
    [HideInInspector] public bool canDash;
    [HideInInspector] public bool chainDash;
    private Coroutine dashCoroutine = null;
    private Coroutine chainCoroutine = null;
    private readonly float dashCD = 0.6f;
    private readonly float chainCD = 0.45f;
    private readonly float iframesCD = 0.3f;

    public void Awake()
    {
        cd = GetComponent<CooldownBehaviour>();
        rb = GetComponent<Rigidbody2D>();
        rd = GetComponent<Renderer>();
        canMove = true;
        chainDash = true;
        canDash = true;
    }

    // Moves the current character.
    public void Move(float x, float y, float speed) { if (canMove) transform.Translate(speed * Time.deltaTime * new Vector2(x, y).normalized); }
    
    // Makes the character "dash" forward.
    public void Dash(float x, float y, float force = 15f, bool doIframes = true) {
        if (!canDash && !chainDash) { if (chainCoroutine != null) StopCoroutine(chainCoroutine); return; }

        // Chaining dashes (oh hell yes)
        if (chainDash && !canDash) {
            if (dashCoroutine != null) StopCoroutine(dashCoroutine);
            if (chainCoroutine != null) StopCoroutine(chainCoroutine);
            canDash = true;
        }

        // Dashes
        rb.AddForce(new Vector2(x, y) * force, ForceMode2D.Impulse);
        if (doIframes) StartCoroutine(DashIframes());
        chainDash = false;

        // Start cooldown
        dashCoroutine = StartCoroutine(cd.StartCooldown(dashCD, result => canDash = result, canDash));
        chainCoroutine = StartCoroutine(ChainDash()); // Dash chaining
    }

    // Changes color of the character renderer and gives some iframes
    private IEnumerator DashIframes() {
        int oldLayer = gameObject.transform.root.gameObject.layer;
        gameObject.transform.root.gameObject.layer = 7;
        rd.material.color = new Color(0.5f, 0.5f, 0.5f);

        yield return new WaitForSeconds(iframesCD);
        gameObject.transform.root.gameObject.layer = oldLayer;
        rd.material.color = new Color(1f, 1f, 1f);
    }

    // Dash chaining
    private IEnumerator ChainDash()
    {
        yield return new WaitForSeconds(chainCD);
        chainDash = true;
    }
}
