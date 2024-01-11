using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CorridorBlock : MonoBehaviour
{
    public Tilemap tilemapCorridor;
    public Tilemap tilemapWall;

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

            Vector3Int corridorPrincipal = GetAproxCorridor(tilePlayerPosition);
            tilemapCorridor.SetTile(corridorPrincipal, wall);
            PrintAllCorridors(corridorPrincipal, "",0);
        }
    }

    private Vector3Int GetAproxCorridor(Vector3Int tilePlayerPosition)
    {
        if (TileIn(tilePlayerPosition.x + 1, tilePlayerPosition.y, tilePlayerPosition.z, "corridor"))
        {
            tilePlayerPosition = new Vector3Int(tilePlayerPosition.x + 1, tilePlayerPosition.y, tilePlayerPosition.z);
        }
        else if (TileIn(tilePlayerPosition.x - 1, tilePlayerPosition.y, tilePlayerPosition.z, "corridor"))
        {
            tilePlayerPosition = new Vector3Int(tilePlayerPosition.x - 1, tilePlayerPosition.y, tilePlayerPosition.z);
        }
        else if (TileIn(tilePlayerPosition.x, tilePlayerPosition.y + 1, tilePlayerPosition.z, "corridor"))
        {
            tilePlayerPosition = new Vector3Int(tilePlayerPosition.x, tilePlayerPosition.y + 1, tilePlayerPosition.z);
        }
        else
        {
            tilePlayerPosition = new Vector3Int(tilePlayerPosition.x, tilePlayerPosition.y - 1, tilePlayerPosition.z);
        }
        return tilePlayerPosition;
    }

    private bool TileIn(int x, int y, int z, string typeTile)
    {
        Vector3Int position = new Vector3Int(x, y, z);
        if (typeTile == "corridor") return tilemapCorridor.GetTile(position) != null;
        else if (typeTile == "wall") return tilemapWall.GetTile(position) != null;
        return false;
    }


    private void PrintAllCorridors(Vector3Int posCorridor, string direction, int corridorsPrinted)
    {
       
        if (direction == "")
        {
            direction = SetDirectionBlockCorridor(posCorridor, "");
        }
        
        if(direction == "right")
        {
            if (TileIn(posCorridor.x + 1, posCorridor.y, 0, "wall"))
            {
                PrintAllCorridors(new Vector3Int(posCorridor.x + 1, posCorridor.y, 0), "right", corridorsPrinted);
            }
            else if (TileIn(posCorridor.x + 1, posCorridor.y, 0, "corridor"))
            {
                tilemapCorridor.SetTile(new Vector3Int(posCorridor.x + 1, posCorridor.y, 0), wall);
                corridorsPrinted = corridorsPrinted + 1;
                if (corridorsPrinted != 3) PrintAllCorridors(new Vector3Int(posCorridor.x + 1, posCorridor.y, 0), "right", corridorsPrinted);
            }
            else
            {
                direction = SetDirectionBlockCorridor(posCorridor, "right");
                PrintAllCorridors(new Vector3Int(posCorridor.x, posCorridor.y, 0), direction, corridorsPrinted);
            }
        }
        else if (direction == "left")
        {
            if (TileIn(posCorridor.x - 1, posCorridor.y, 0, "wall"))
            {
                PrintAllCorridors(new Vector3Int(posCorridor.x - 1, posCorridor.y, 0), "left", corridorsPrinted);
            }
            else if (TileIn(posCorridor.x - 1, posCorridor.y, 0, "corridor"))
            {
                tilemapCorridor.SetTile(new Vector3Int(posCorridor.x -1, posCorridor.y, 0), wall);
                corridorsPrinted = corridorsPrinted + 1;
                if (corridorsPrinted != 3) PrintAllCorridors(new Vector3Int(posCorridor.x - 1, posCorridor.y, 0), "left", corridorsPrinted);
            }
            else
            {
                direction = SetDirectionBlockCorridor(posCorridor, "left");
                PrintAllCorridors(new Vector3Int(posCorridor.x, posCorridor.y, 0), direction, corridorsPrinted);
            }
        }
        else if(direction == "up")
        {
            if (TileIn(posCorridor.x, posCorridor.y + 1, 0, "wall"))
            {
                PrintAllCorridors(new Vector3Int(posCorridor.x, posCorridor.y + 1, 0), "up", corridorsPrinted);
            }
            else if (TileIn(posCorridor.x, posCorridor.y + 1, 0, "corridor"))
            {
                tilemapCorridor.SetTile(new Vector3Int(posCorridor.x, posCorridor.y + 1, 0), wall);
                corridorsPrinted = corridorsPrinted + 1;
                if (corridorsPrinted != 3) PrintAllCorridors(new Vector3Int(posCorridor.x, posCorridor.y + 1, 0), "up", corridorsPrinted);
                else return;
            }
            else
            {
                direction = SetDirectionBlockCorridor(posCorridor, "up");
                PrintAllCorridors(new Vector3Int(posCorridor.x, posCorridor.y, 0), direction, corridorsPrinted);
            }
        }
        else if(direction == "down")
        {
            if (TileIn(posCorridor.x, posCorridor.y - 1, 0, "wall"))
            {
                PrintAllCorridors(new Vector3Int(posCorridor.x, posCorridor.y -1, 0), "down", corridorsPrinted);
            }
            else if (TileIn(posCorridor.x, posCorridor.y - 1, 0, "corridor"))
            {
                tilemapCorridor.SetTile(new Vector3Int(posCorridor.x, posCorridor.y -1, 0), wall);
                corridorsPrinted = corridorsPrinted + 1;
                if (corridorsPrinted != 3) PrintAllCorridors(new Vector3Int(posCorridor.x, posCorridor.y - 1, 0), "down", corridorsPrinted);
                else return;
            }
            else
            {
                direction = SetDirectionBlockCorridor(posCorridor, "down");
                PrintAllCorridors(new Vector3Int(posCorridor.x, posCorridor.y, 0), direction, corridorsPrinted);
            }
        }

    }

    private string SetDirectionBlockCorridor(Vector3Int posCorridor, string isComing)
    {
        int i = 1;
        Debug.Log("Change direction");
        if (TileIn(posCorridor.x + i, posCorridor.y, 0, "wall"))
        {
            if (isComing != "left") return "right";
            else
            {
                if (TileIn(posCorridor.x, posCorridor.y + i, 0, "wall"))
                {
                    return "up";
                }
                else { return "down"; }
            }
        }
        else if (TileIn(posCorridor.x - i, posCorridor.y, 0, "wall"))
        {
            if (isComing != "right") return "left";
            else
            {
                if (TileIn(posCorridor.x, posCorridor.y + i, 0, "wall"))
                {
                    return "up";
                }
                else { return "down"; }
            }
        }
        else if (TileIn(posCorridor.x, posCorridor.y + i, 0, "wall"))
        {
            if (isComing != "down") return "up";
            else
            {
                if(TileIn(posCorridor.x + i, posCorridor.y, 0, "wall"))
                {
                    return "right";
                }
                else { return "left"; }
            }
        }
        else if (TileIn(posCorridor.x, posCorridor.y - i, 0, "wall"))
        {
            if (isComing != "up") return "down";
            else
            {
                if (TileIn(posCorridor.x + i, posCorridor.y, 0, "wall"))
                {
                    return "right";
                }
                else { return "left"; }
            }
        }
        else return "";
    }
}
