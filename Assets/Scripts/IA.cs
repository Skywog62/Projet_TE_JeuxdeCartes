using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class IA : MonoBehaviour
{
    public List<Card> deck = new List<Card>();
    public List<Card> hand = new List<Card>();
    public int maxHandSize = 5;
    public int gold = 10;
    public int Health = 100;
    public string playerName = "IA";

    private GridManager gridManager;

    void Start()
    {
        gridManager = FindFirstObjectByType<GridManager>();

        if (gridManager == null)
        {
            Debug.LogError("❌ GridManager non trouvé !");
            return;
        }

        InitializeDeck();
        DrawInitialCards(3);
    }

    public void PlayTurn()
    {
        Debug.Log("L'IA joue son tour...");

        DrawCard();

        if (hand.Count > 0)
        {
            Debug.Log("L'IA joue une carte...");
            // Logique pour jouer une carte ici...
        }

        Debug.Log("Fin du tour de l'IA.");
        GameManager.Instance.EndTurn();
    }

    public void DrawInitialCards(int count)
    {
        for (int i = 0; i < count; i++)
        {
            DrawCard();
        }
    }

    public void InitializeDeck()
    {
        if (GameManager.Instance == null || GameManager.Instance.cardPrefab == null)
        {
            Debug.LogError("❌ GameManager ou cardPrefab non défini !");
            return;
        }

        deck.Clear();
        string[] cardNames = { "Soldat IA", "Sniper IA", "Robot IA", "Tank IA" };
        int[] attackValues = { 2, 5, 3, 4 };
        int[] defenseValues = { 4, 2, 5, 6 };
        int[] costs = { 2, 3, 3, 4 };

        for (int i = 0; i < cardNames.Length; i++)
        {
            GameObject cardObj = Instantiate(GameManager.Instance.cardPrefab);
            Card newCard = cardObj.GetComponent<Card>();

            if (newCard == null)
            {
                Debug.LogError("❌ Le prefab de carte n'a pas de script Card attaché !");
                Destroy(cardObj);
                continue;
            }

            newCard.cardName = cardNames[i];
            newCard.attack = attackValues[i];
            newCard.defense = defenseValues[i];
            newCard.cost = costs[i];

            deck.Add(newCard);

            // ✅ Suppression de l'objet temporaire pour éviter qu'il apparaisse dans la scène
            Destroy(cardObj);
        }

        ShuffleDeck();
    }

    void ShuffleDeck()
    {
        deck = deck.OrderBy(x => Random.value).ToList();
    }

    public void DrawCard()
    {
        Debug.Log($"🃏 Deck IA contient {deck.Count} cartes avant pioche.");

        if (deck.Count == 0)
        {
            Debug.LogWarning("⚠️ L'IA ne peut plus piocher, deck vide !");
            Health -= 5;
            return;
        }

        if (hand.Count < maxHandSize)
        {
            Card drawnCard = deck[0];
            deck.RemoveAt(0);
            hand.Add(drawnCard);
            Debug.Log($"🤖 L'IA pioche {drawnCard.cardName} !");

            // ✅ Vérification avant d'instancier
            if (GameManager.Instance == null || GameManager.Instance.enemyHandUI == null)
            {
                Debug.LogWarning("⚠️ GameManager.Instance ou enemyHandUI est null, impossible d'afficher la carte !");
                return;
            }

            // ✅ Instanciation correcte du GameObject pour la carte de l'IA
            GameObject cardObj = Instantiate(GameManager.Instance.cardPrefab, GameManager.Instance.enemyHandUI);
            cardObj.transform.SetParent(GameManager.Instance.enemyHandUI, false);

            // ✅ Vérification du composant CardUI
            CardUI cardUI = cardObj.GetComponent<CardUI>();
            if (cardUI == null)
            {
                Debug.LogError("❌ Erreur : CardUI n'a pas été trouvé sur le prefab !");
                Destroy(cardObj);
                return;
            }

            // Applique les données de la carte
            cardUI.Setup(drawnCard);

            // ✅ Vérification du RectTransform avant modification
            RectTransform rectTransform = cardObj.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                rectTransform.anchoredPosition = new Vector2((hand.Count - 1) * 120, 0);
            }
            else
            {
                Debug.LogError("❌ Erreur : RectTransform introuvable sur le prefab !");
            }

            // ✅ Mise à jour de l'affichage du deck
            GameManager.Instance.UpdateDeckUI();
        }
    }
}
