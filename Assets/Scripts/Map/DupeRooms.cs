using UnityEngine;

public class DupeRooms : MonoBehaviour
{ 
    public void OnTriggerEnter2D(Collider2D col)
    {
        if (col.TryGetComponent(out Room room)) room.roomsShared.Add(gameObject);
    }
}