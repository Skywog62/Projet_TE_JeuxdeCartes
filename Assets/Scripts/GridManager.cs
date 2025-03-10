using UnityEngine;

public class GridManager : MonoBehaviour
{
    public int rows = 4;
    public int columns = 4;
    public float cellSize = 3.0f;
    public GameObject cellPrefab;

    public GameObject[,] gridCells { get; private set; }

    void Start()
    {
        CreateGrid();
    }

    public Vector3 GetCellPosition(int row, int column)
    {
        if (row >= 0 && row < rows && column >= 0 && column < columns)
        {
            return gridCells[row, column].transform.position;
        }
        return Vector3.zero;
    }

    public Vector2Int GetEmptyCell()
    {
        for (int x = 0; x < rows; x++)
        {
            for (int y = 0; y < columns; y++)
            {
                if (!gridCells[x, y].GetComponent<Cell>().isOccupied)
                {
                    return new Vector2Int(x, y);
                }
            }
        }
        return new Vector2Int(-1, -1); // Aucune cellule libre
    }

    void CreateGrid()
    {
        gridCells = new GameObject[rows, columns];
        for (int x = 0; x < rows; x++)
        {
            for (int y = 0; y < columns; y++)
            {
                Vector3 position = new Vector3(x * cellSize, 0, y * cellSize);
                GameObject cell = Instantiate(cellPrefab, position, Quaternion.identity);
                cell.GetComponent<Cell>().row = x;
                cell.GetComponent<Cell>().column = y;
                gridCells[x, y] = cell;
            }
        }
    }
}