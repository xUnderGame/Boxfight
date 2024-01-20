using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Room : MonoBehaviour
{
    public int id;
    public int size;
    public List<GameObject> enemyList = new();
    public List<Vector3Int> corridorPosition = new();
    public List<GameObject> roomsShared = new();

    GameObject corridorObject;
    CorridorBlock corridorScript;

    private void Awake()
    {
        corridorObject = GameObject.Find("Puente");
        corridorScript = corridorObject.GetComponent<CorridorBlock>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") == false) return;
        
        Player playerScript = collision.GetComponent<Player>();
        TrapIsOn(playerScript);

        if (roomsShared.Count == 0) return;
        for (int i = 0; i < roomsShared.Count; i++)
        {
            roomsShared[i].GetComponent<Room>().TrapIsOn(playerScript);
        }
    }

    public bool CheckIfAllEnemiesDead()
    {
        if (enemyList.Count != 0) return false;
        return !roomsShared.Any(room => { return room.GetComponent<Room>().enemyList.Count != 0; });
    }

    public void TrapIsOff()
    {
        corridorScript.PrintCorridors(corridorPosition, "unblock");
        foreach (GameObject room in roomsShared)
        {
            corridorScript.PrintCorridors(room.GetComponent<Room>().corridorPosition, "unblock");
        }
    }

    public void TrapIsOn(Player player)
    {
        if (enemyList.Count != 0 && !player.idRoomsVisited.Contains(id))
        {
            corridorScript.PrintCorridors(corridorPosition, "block");
            player.idRoomsVisited.Add(id);
            for (int i = 0; i < enemyList.Count; i++)
            {
                enemyList[i].SetActive(true);
            }
        }
    }
}
