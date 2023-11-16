using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D), typeof(Animator))]
public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float test = Input.GetAxis("Horizontal");
        float test2 = Input.GetAxis("Vertical");
        test *= Time.deltaTime;
        test2 *= Time.deltaTime;
        transform.Translate(test, test2, transform.position.z);
    }
}
