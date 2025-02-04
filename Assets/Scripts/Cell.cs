using UnityEngine;

public class Cell : MonoBehaviour
{
    public bool isOccupied = false;

    private void OnMouseDown()
    {
        if (!isOccupied)
        {
            GameObject card = Instantiate(Resources.Load<GameObject>("Card"));
            card.transform.position = transform.position + Vector3.up * 0.5f;
            isOccupied = true;
        }
    }
}

