using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementBehaviour : MonoBehaviour
{
    public void Move(float x, float y, float speed) { transform.Translate(speed * Time.deltaTime * new Vector2(x, y).normalized); }
}
