using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CorridorBlock : MonoBehaviour
{
    public Tilemap tilemapCorridor;

    public TileBase corridor;
    public TileBase wall;
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            TilemapCollider2D collider = GetComponent<TilemapCollider2D>();
            collider.isTrigger = false;
            Vector3 posPlayer = other.transform.position;
            Vector3Int tilePlayerPosition = tilemapCorridor.WorldToCell(posPlayer);
            tilemapCorridor.SetTile(GetAproxCorridor(tilePlayerPosition), wall);
        }
    }

    private Vector3Int GetAproxCorridor(Vector3Int tilePlayerPosition)
    {
        if (CorridorIn(tilePlayerPosition.x + 1, tilePlayerPosition.y, tilePlayerPosition.z))
        {
            tilePlayerPosition = new Vector3Int(tilePlayerPosition.x + 1, tilePlayerPosition.y, tilePlayerPosition.z);
        }
        else if(CorridorIn(tilePlayerPosition.x - 1, tilePlayerPosition.y, tilePlayerPosition.z))
        {
            tilePlayerPosition = new Vector3Int(tilePlayerPosition.x - 1, tilePlayerPosition.y, tilePlayerPosition.z);
        }
        else if(CorridorIn(tilePlayerPosition.x, tilePlayerPosition.y + 1, tilePlayerPosition.z))
        {
            tilePlayerPosition = new Vector3Int(tilePlayerPosition.x, tilePlayerPosition.y + 1, tilePlayerPosition.z);
        }
        else
        {
            tilePlayerPosition = new Vector3Int(tilePlayerPosition.x, tilePlayerPosition.y -1, tilePlayerPosition.z);
        }


        return tilePlayerPosition;
    }

    private bool CorridorIn(int x, int y ,int z)
    {
        Vector3Int position = new Vector3Int(x, y, z);
        return tilemapCorridor.GetTile(position) != null;
    }
}
