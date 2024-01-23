using UnityEngine;

public class AnimationBehaviour : MonoBehaviour
{
    public Animator animator;
    private SpriteRenderer spriteRenderer;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Bom()
    {
        animator.SetBool("Death", true);
    }
}
