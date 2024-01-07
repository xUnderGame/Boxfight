using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCamera : MonoBehaviour
{
    private Vector3 offset = new(0f, 0f, -8f);
    private readonly float smoothTime = 0.15f;
    private Vector3 velocity = Vector3.zero;
    
    void LateUpdate()
    {
        Vector3 targetPosition = GameManager.Instance.playerObject.transform.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }
}
