using UnityEngine;

public class AnimationBehaviour : MonoBehaviour
{

    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }


}
