using UnityEngine;
using UnityEngine.UI;

public class Enemy_Melee : Enemy
{
    private void OnEnable()
    {
        base.OnEnable();
    }

    private void Update()
    {
        mov.Move(transform.position.x,transform.position.y,currentSpeed, "follower");
    }
}
