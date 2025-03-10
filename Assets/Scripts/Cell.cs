using UnityEngine;

public class Cell : MonoBehaviour
{
    public bool isOccupied = false;
    public int row;
    public int column;

    private void OnMouseDown()
    {
        if (!isOccupied)
        {
            GameObject card = Instantiate(Resources.Load<GameObject>("Card"));
            card.transform.position = transform.position;
            card.GetComponent<Card>().SetGridPosition(row, column);
            isOccupied = true;
        }
    }
}