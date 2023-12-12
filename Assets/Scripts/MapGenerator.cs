using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapRandomSizedSquaresDrawer : MonoBehaviour
{
    public Tilemap tilemap;
    public TileBase tileToDraw; // La casilla que usarás para dibujar
    public int minSquareSize = 3; // Tamaño mínimo de los cuadrados
    public int maxSquareSize = 8; // Tamaño máximo de los cuadrados
    public int separation = 2; // Separación entre los cuadrados

    void Start()
    {
        DrawRandomSizedSquares();
    }

    void DrawRandomSizedSquares()
    {
        if (tilemap == null || tileToDraw == null)
            return;

        // Tamaño aleatorio para el cuadrado central
        int centerSquareSize = Random.Range(minSquareSize, maxSquareSize + 1);
        Vector3Int centerSquareStart = new Vector3Int(-centerSquareSize / 2, -centerSquareSize / 2, 0);
        DrawSquare(centerSquareStart, centerSquareSize);

        // Dibujar cuadrados adicionales con tamaños aleatorios
        for (int i = 0; i < 4; i++)
        {
            int squareSize = Random.Range(minSquareSize, maxSquareSize + 1);
            Vector3Int squareStart = CalculateSquareStart(centerSquareStart, centerSquareSize, squareSize, i);
            DrawSquare(squareStart, squareSize);
            ConnectSquares(centerSquareStart, centerSquareSize, squareStart, squareSize);
        }
    }

    Vector3Int CalculateSquareStart(Vector3Int centerStart, int centerSize, int squareSize, int direction)
    {
        // Calcula la posición inicial de un cuadrado adicional en una dirección específica
        int totalDistance = centerSize / 2 + squareSize / 2 + separation;
        switch (direction)
        {
            case 0: // Arriba
                return new Vector3Int(centerStart.x, centerStart.y + totalDistance, 0);
            case 1: // Abajo
                return new Vector3Int(centerStart.x, centerStart.y - totalDistance, 0);
            case 2: // Derecha
                return new Vector3Int(centerStart.x + totalDistance, centerStart.y, 0);
            case 3: // Izquierda
                return new Vector3Int(centerStart.x - totalDistance, centerStart.y, 0);
            default:
                return Vector3Int.zero;
        }
    }

    void DrawSquare(Vector3Int startPosition, int squareSize)
    {
        for (int x = 0; x < squareSize; x++)
        {
            for (int y = 0; y < squareSize; y++)
            {
                Vector3Int tilePosition = new Vector3Int(startPosition.x + x, startPosition.y + y, 0);
                tilemap.SetTile(tilePosition, tileToDraw);
            }
        }
    }

    void ConnectSquares(Vector3Int firstStart, int firstSize, Vector3Int secondStart, int secondSize)
    {
        // Calcula los puntos medios
        Vector3Int midPointFirst = firstStart + new Vector3Int(firstSize / 2, firstSize / 2, 0);
        Vector3Int midPointSecond = secondStart + new Vector3Int(secondSize / 2, secondSize / 2, 0);

        // Dibuja una línea de casillas entre los puntos medios
        Vector3Int direction = midPointSecond - midPointFirst;
        int steps = Mathf.Max(Mathf.Abs(direction.x), Mathf.Abs(direction.y));
        Vector3Int step = new Vector3Int(direction.x / steps, direction.y / steps, 0);

        for (int i = 1; i < steps; i++) // Comienza en 1 para evitar superponer el punto medio
        {
            Vector3Int tilePosition = midPointFirst + step * i;
            tilemap.SetTile(tilePosition, tileToDraw);
        }
    }
}
