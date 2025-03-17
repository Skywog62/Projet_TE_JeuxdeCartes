using UnityEngine;

public class EventManager : MonoBehaviour
{
    public void TriggerEvent()
    {
        int eventType = Random.Range(0, 2);

        if (eventType == 0)
        {
            GameManager.Instance.goldManager.GainGold(10);
            Debug.Log("Événement : Gain économique ! +10 or");
        }
        else
        {
            Card[] allCards = Object.FindObjectsByType<Card>(FindObjectsSortMode.None);
            foreach (Card card in allCards)
            {
                card.attack += 1;
            }
            Debug.Log("Événement : Boost offensif ! +1 attaque pour toutes les cartes.");
        }
    }
}