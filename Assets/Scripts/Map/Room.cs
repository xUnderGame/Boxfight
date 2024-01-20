using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Room : MonoBehaviour
{
    public int id;
    public int size;
    [SerializeField] public List<GameObject> enemyList = new();
    public List<Vector3Int> corridorPosition = new();
    public List<GameObject> roomsShared = new();

    public List<Vector3Int> lostCorridors = new();

    private GameObject corridorObject;
    private CorridorBlock corridorScript;

    private void Awake()
    {
        corridorObject = GameObject.Find("Puente");
        corridorScript = corridorObject.GetComponent<CorridorBlock>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") == false) return;
        Player playerScript = collision.GetComponent<Player>();

        if (roomsShared.Count == 0)
        {
            TrapIsOn(playerScript, false);
            return;
        }

        TrapIsOn(playerScript, true);
        for (int i = 0; i < roomsShared.Count; i++)
        {
            roomsShared[i].GetComponent<Room>().TrapIsOn(playerScript, true);
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

    public void TrapIsOn(Player player, bool isShared)
    {
        if (enemyList.Count != 0 && !player.idRoomsVisited.Contains(id))
        {
            if (lostCorridors.Count != 0)
            {
                 Vector3Int lostcorr = GetCorridorLost(lostCorridors);
                corridorPosition.Add(lostcorr);
            }
            corridorScript.PrintCorridors(corridorPosition, "block");
            player.idRoomsVisited.Add(id);
            for (int i = 0; i < enemyList.Count; i++)
            {
                enemyList[i].SetActive(true);
            }
        }
        else if (isShared == true)
        {
            corridorScript.PrintCorridors(corridorPosition, "block");
            player.idRoomsVisited.Add(id);
            if (lostCorridors.Count != 0)
            {
                Vector3Int lostcorr = GetCorridorLost(lostCorridors);
                corridorPosition.Add(lostcorr);
            }
        }
    }

    private Vector3Int GetCorridorLost(List<Vector3Int> lostedCorridors)
    {
        if (lostCorridors[0].x == lostCorridors[lostCorridors.Count - 1].x)
        {
            if (corridorScript.TileIn(lostedCorridors[0].x, lostedCorridors[0].y + 1, 0, "corridor")) return new Vector3Int(lostedCorridors[0].x, lostedCorridors[0].y + 1, 0);
            else if (corridorScript.TileIn(lostedCorridors[0].x, lostedCorridors[0].y - 1, 0, "corridor")) return new Vector3Int(lostedCorridors[0].x, lostedCorridors[0].y - 1, 0);
            else if (corridorScript.TileIn(lostedCorridors[lostCorridors.Count - 1].x, lostedCorridors[lostCorridors.Count - 1].y + 1, 0, "corridor")) return new Vector3Int(lostedCorridors[lostCorridors.Count - 1].x, lostedCorridors[lostCorridors.Count - 1].y + 1, 0);
            else if (corridorScript.TileIn(lostedCorridors[lostCorridors.Count - 1].x, lostedCorridors[lostCorridors.Count - 1].y - 1, 0, "corridor")) return new Vector3Int(lostedCorridors[lostCorridors.Count - 1].x, lostedCorridors[lostCorridors.Count - 1].y - 1, 0);
            else return new Vector3Int(0, 0, 0);
        }
        else if (lostCorridors[0].y == lostCorridors[lostCorridors.Count - 1].y)
        {
            if (corridorScript.TileIn(lostedCorridors[0].x + 1, lostedCorridors[0].y, 0, "corridor")) return new Vector3Int(lostedCorridors[0].x + 1, lostedCorridors[0].y, 0);
            else if (corridorScript.TileIn(lostedCorridors[0].x - 1, lostedCorridors[0].y - 1, 0, "corridor")) return new Vector3Int(lostedCorridors[0].x - 1, lostedCorridors[0].y, 0);
            else if (corridorScript.TileIn(lostedCorridors[lostCorridors.Count - 1].x + 1, lostedCorridors[lostCorridors.Count - 1].y, 0, "corridor")) return new Vector3Int(lostedCorridors[lostCorridors.Count - 1].x + 1, lostedCorridors[lostCorridors.Count - 1].y, 0);
            else if (corridorScript.TileIn(lostedCorridors[lostCorridors.Count - 1].x - 1, lostedCorridors[lostCorridors.Count - 1].y, 0, "corridor")) return new Vector3Int(lostedCorridors[lostCorridors.Count - 1].x - 1, lostedCorridors[lostCorridors.Count - 1].y, 0);
            else return new Vector3Int(0, 0, 0);
        }
        else return new Vector3Int(0, 0, 0);
    }


}
