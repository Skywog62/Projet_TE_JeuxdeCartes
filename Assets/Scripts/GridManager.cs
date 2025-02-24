using UnityEngine;

public class GridManager : MonoBehaviour
{
    public int rows = 4;
    public int columns = 4;
    public float cellSize = 1.5f;
    public GameObject cellPrefab;

    public GameObject[,] gridCells { get; private set; }

    public bool IsGridReady()
    {
        return gridCells != null;
    }

    void Start()
    {
        CreateGrid();
    }

    public Vector3 GetCellPosition(int row, int column)
    {
        if (gridCells == null)
        {
            Debug.LogError("GridManager: La grille n'a pas encore été initialisée !");
            return Vector3.zero;
        }

        if (row >= 0 && row < rows && column >= 0 && column < columns && gridCells[row, column] != null)
        {
            return gridCells[row, column].transform.position + new Vector3(0, 0.5f, 0);
        }

        Debug.LogError($"GridManager: Position invalide demandée ({row}, {column}) !");
        return Vector3.zero; // Position par défaut si la cellule est invalide
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
                cell.transform.SetParent(transform);
                gridCells[x, y] = cell;
            }
        }
    }
}

