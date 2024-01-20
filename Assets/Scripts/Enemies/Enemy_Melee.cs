using UnityEngine;
using UnityEngine.UI;

public class Enemy_Melee : Enemy
{
    public override void OnEnable()
    {
        base.OnEnable();
    }

    private void FixedUpdate()
    {
        Vector2 whereToMove = transform.position - GameManager.Instance.playerObject.transform.position;
        mov.Move(-whereToMove.x, -whereToMove.y, currentSpeed);
    }
}
