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
    public TileBase floor;

    public List<Vector3Int> posWalled = new List<Vector3Int>();

    private bool cantBeFirstStep = true;

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            TilemapCollider2D collider = GetComponent<TilemapCollider2D>();
            collider.isTrigger = false;
            Vector3 posPlayer = other.transform.position;
            Vector3Int tilePlayerPosition = tilemapCorridor.WorldToCell(posPlayer);

            Vector3Int firstPoint = GetAproxCorridor(tilePlayerPosition);
            posWalled.Add(firstPoint);
            tilemapCorridor.SetTile(firstPoint, wall);
            PrintAllCorridors(firstPoint, "");
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


    private void PrintAllCorridors(Vector3Int posCorridor, string direction)
    {
       
        if (posCorridor != posWalled[0] || cantBeFirstStep == true)
        {
            cantBeFirstStep = false;
            if (direction == "")
            {
                direction = SetDirectionBlockCorridor(posCorridor, "");
            }

            if (direction == "right")
            {
                if (TileIn(posCorridor.x + 1, posCorridor.y, 0, "wall"))
                {
                    PrintAllCorridors(new Vector3Int(posCorridor.x + 1, posCorridor.y, 0), "right");
                }
                else if (TileIn(posCorridor.x + 1, posCorridor.y, 0, "corridor"))
                {
                    tilemapCorridor.SetTile(new Vector3Int(posCorridor.x + 1, posCorridor.y, 0), wall);
                    posWalled.Add(new Vector3Int(posCorridor.x + 1, posCorridor.y, 0));
                    PrintAllCorridors(new Vector3Int(posCorridor.x + 1, posCorridor.y, 0), "right");
                }
                else
                {
                    direction = SetDirectionBlockCorridor(posCorridor, "right");
                    PrintAllCorridors(new Vector3Int(posCorridor.x, posCorridor.y, 0), direction);
                }
            }
            else if (direction == "left")
            {
               if (TileIn(posCorridor.x - 1, posCorridor.y, 0, "wall"))
                {
                    PrintAllCorridors(new Vector3Int(posCorridor.x - 1, posCorridor.y, 0), "left");
                }
                else if (TileIn(posCorridor.x - 1, posCorridor.y, 0, "corridor"))
                {
                    tilemapCorridor.SetTile(new Vector3Int(posCorridor.x - 1, posCorridor.y, 0), wall);
                    posWalled.Add(new Vector3Int(posCorridor.x - 1, posCorridor.y, 0));
                    PrintAllCorridors(new Vector3Int(posCorridor.x - 1, posCorridor.y, 0), "left");
                }
                else
                {
                    direction = SetDirectionBlockCorridor(posCorridor, "left");
                    PrintAllCorridors(new Vector3Int(posCorridor.x, posCorridor.y, 0), direction);
                }
            }
            else if (direction == "up")
            {
                if (TileIn(posCorridor.x, posCorridor.y + 1, 0, "wall"))
                {
                    PrintAllCorridors(new Vector3Int(posCorridor.x, posCorridor.y + 1, 0), "up");
                }
                else if (TileIn(posCorridor.x, posCorridor.y + 1, 0, "corridor"))
                {
                    tilemapCorridor.SetTile(new Vector3Int(posCorridor.x, posCorridor.y + 1, 0), wall);
                    posWalled.Add(new Vector3Int(posCorridor.x, posCorridor.y + 1, 0));
                    PrintAllCorridors(new Vector3Int(posCorridor.x, posCorridor.y + 1, 0), "up");

                }
                else
                {
                    direction = SetDirectionBlockCorridor(posCorridor, "up");
                    PrintAllCorridors(new Vector3Int(posCorridor.x, posCorridor.y, 0), direction);
                }
            }
            else if (direction == "down")
            {
                if (TileIn(posCorridor.x, posCorridor.y - 1, 0, "wall"))
                {
                    PrintAllCorridors(new Vector3Int(posCorridor.x, posCorridor.y - 1, 0), "down");
                }
                else if (TileIn(posCorridor.x, posCorridor.y - 1, 0, "corridor"))
                {
                    tilemapCorridor.SetTile(new Vector3Int(posCorridor.x, posCorridor.y - 1, 0), wall);
                    posWalled.Add(new Vector3Int(posCorridor.x, posCorridor.y - 1, 0));
                    PrintAllCorridors(new Vector3Int(posCorridor.x, posCorridor.y - 1, 0), "down");

                }
                else
                {
                    direction = SetDirectionBlockCorridor(posCorridor, "down");
                    PrintAllCorridors(new Vector3Int(posCorridor.x, posCorridor.y, 0), direction);
                }
            }
        } else return; 
        

    }

    //Aqui hay fallos en las entradas
    private string SetDirectionBlockCorridor(Vector3Int posCorridor, string isComing)
    {
        int i = 1;
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
                if (TileIn(posCorridor.x + i, posCorridor.y, 0, "wall"))
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

    public void SetPointsWalledOnCorridor()
    {
        for (int i = 0; i < posWalled.Count; i++)
        {
            TilemapCollider2D collider = GetComponent<TilemapCollider2D>();
            collider.isTrigger = true;
            tilemapCorridor.SetTile(posWalled[i], corridor);
        }
    }
}
