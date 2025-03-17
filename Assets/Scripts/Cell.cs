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
            GameObject cardPrefab = GameManager.Instance.cardPrefab;
            if (cardPrefab == null)
            {
                Debug.LogError("cardPrefab non trouvé dans GameManager !");
                return;
            }

            GameObject card = Instantiate(cardPrefab, transform.position, Quaternion.identity);
            card.GetComponent<Card>().SetGridPosition(row, column);
            isOccupied = true;
        }
    }
}