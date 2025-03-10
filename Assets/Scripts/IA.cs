using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class IA : MonoBehaviour
{
    public List<Card> deck = new List<Card>();
    public List<Card> hand = new List<Card>();
    public int gold = 10;
    public int maxHandSize = 5;
    public int Health = 100; // HP de l'IA

    private GridManager gridManager;
    private PlayerManager player;

    void Start()
    {
        gridManager = FindFirstObjectByType<GridManager>();
        player = FindFirstObjectByType<PlayerManager>(); 

        for (int i = 0; i < 3; i++)
        {
            DrawCard();
        }
    }

    public void PlayTurn()
    {
        Debug.Log("L'IA commence son tour...");

        if (hand.Count > 0)
        {
            PlayBestCard();
        }

        AttackPlayer();

        Debug.Log("Tour de l'IA terminé.");
        GameManager.Instance.EndTurn();
    }

    void DrawCard()
    {
        if (deck.Count > 0 && hand.Count < maxHandSize)
        {
            Card newCard = deck[0];
            deck.RemoveAt(0);
            hand.Add(newCard);
            Debug.Log($"IA pioche {newCard.cardName}");
        }
    }

    void PlayBestCard()
    {
        Card bestCard = hand.OrderByDescending(c => c.attack + c.defense).FirstOrDefault();

        if (bestCard != null && gold >= bestCard.cost)
        {
            Vector2Int emptyCell = gridManager.GetEmptyCell();
            if (emptyCell != Vector2Int.one * -1)
            {
                GameObject cardObj = Instantiate(GameManager.Instance.cardPrefab, gridManager.GetCellPosition(emptyCell.x, emptyCell.y), Quaternion.identity);
                Card newCard = cardObj.GetComponent<Card>();
                newCard.cardName = bestCard.cardName;
                newCard.attack = bestCard.attack;
                newCard.defense = bestCard.defense;
                newCard.cost = bestCard.cost;
                newCard.cardEffect = bestCard.cardEffect;
                newCard.SetGridPosition(emptyCell.x, emptyCell.y);
                newCard.gameObject.tag = "Enemy"; // ✅ Ajout du tag "Enemy"

                gold -= bestCard.cost;
                hand.Remove(bestCard);
                Debug.Log($"IA joue {bestCard.cardName} !");
            }
        }
    }

    void AttackPlayer()
    {
        List<Card> myCards = FindObjectsByType<Card>(FindObjectsSortMode.None)
            .Where(c => c != null && c.gameObject.activeInHierarchy).ToList();

        List<Card> enemyCards = FindObjectsByType<Card>(FindObjectsSortMode.None)
            .Where(c => c != null && c.gameObject.activeInHierarchy && c.CompareTag("Player")).ToList();

        Debug.Log($"Cartes IA : {myCards.Count}, Cartes adverses trouvées : {enemyCards.Count}");

        foreach (Card myCard in myCards)
        {
            if (enemyCards.Count > 0)
            {
                Card target = enemyCards.OrderBy(c => c.defense).First();
                myCard.Attack(target);
                Debug.Log($"IA attaque {target.cardName} avec {myCard.cardName}");

                // ✅ Ajout d'un délai pour éviter que tout s'enchaîne trop vite
                StartCoroutine(WaitBeforeNextAttack());
            }
            else
            {
                GameManager.Instance.goldManager.GainGold(5);
                Debug.Log("L'IA ne trouve pas de cible, elle économise pour le prochain tour.");
            }
        }
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;
        Debug.Log($"L'IA subit {damage} dégâts. PV restants : {Health}");

        if (Health <= 0)
        {
            Debug.Log("L'IA a été vaincue !");
            GameManager.Instance.EndGame(true);
        }
    }

    // ✅ Nouvelle fonction pour ralentir l'attaque
    IEnumerator WaitBeforeNextAttack()
    {
        yield return new WaitForSeconds(0.5f);
    }
}
