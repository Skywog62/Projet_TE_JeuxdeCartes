using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class PlayerManager : MonoBehaviour
{
    public string playerName = "Joueur";
    public int Health = 100;
    public List<Card> deck;
    public List<Card> hand;
    public int maxHandSize = 5;
    public int economyPoints = 0;

    public int EconomyStrength => economyPoints;
    public Strategy currentStrategy { get; private set; }
    public enum Strategy { Economic, Aggressive }

    void Start()
    {
        deck = new List<Card>(); // Assure l'initialisation
        hand = new List<Card>(); // Assure l'initialisation
        InitializeDeck();
        DrawInitialCards(3);
    }

    public void DrawInitialCards(int count)
    {
        for (int i = 0; i < count; i++)
        {
            if (deck.Count > 0)
                DrawCard();
        }
    }

    void InitializeDeck()
    {
        if (GameManager.Instance == null || GameManager.Instance.cardPrefab == null)
        {
            Debug.LogError("❌ GameManager ou cardPrefab non défini !");
            return;
        }

        deck.Clear();
        string[] cardNames = { "Guerrier", "Mage", "Archer", "Paladin" };
        int[] attackValues = { 3, 4, 2, 5 };
        int[] defenseValues = { 5, 3, 4, 6 };
        int[] costs = { 3, 2, 2, 4 };

        for (int i = 0; i < cardNames.Length; i++)
        {
            GameObject cardObj = Instantiate(GameManager.Instance.cardPrefab);
            Card newCard = cardObj.GetComponent<Card>();

            if (newCard != null)
            {
                newCard.cardName = cardNames[i];
                newCard.attack = attackValues[i];
                newCard.defense = defenseValues[i];
                newCard.cost = costs[i];
                deck.Add(newCard);

                // ✅ Supprime immédiatement l'objet temporaire pour ne pas l'afficher dans la scène
                Destroy(cardObj);
            }
            else
            {
                Debug.LogError("❌ Erreur : le prefab de carte n'a pas le script Card attaché !");
                Destroy(cardObj);
            }
        }

        ShuffleDeck();
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;
        Debug.Log($"{playerName} subit {damage} dégâts. PV restants : {Health}");

        if (Health <= 0)
        {
            Debug.Log($"{playerName} a perdu la partie !");
            GameManager.Instance.EndGame(false);
        }
    }

    void ShuffleDeck()
    {
        deck = deck.OrderBy(x => Random.value).ToList();
    }

    public void DrawCard()
    {
        Debug.Log($"🃏 Deck contient {deck.Count} cartes avant pioche.");

        if (deck.Count == 0)
        {
            Debug.LogWarning($"⚠️ {playerName} ne peut plus piocher, deck vide !");
            Health -= 5;
            return;
        }

        if (hand.Count < maxHandSize)
        {
            Card drawnCard = deck[0];
            deck.RemoveAt(0);
            hand.Add(drawnCard);
            Debug.Log($"✅ {playerName} pioche {drawnCard.cardName} !");

            if (GameManager.Instance == null || GameManager.Instance.playerHandUI == null)
            {
                Debug.LogWarning("⚠️ GameManager.Instance ou playerHandUI est NULL, impossible d'afficher la carte !");
                return;
            }

            // ✅ Vérification avant instanciation
            GameObject cardObj = Instantiate(GameManager.Instance.cardPrefab, GameManager.Instance.playerHandUI);
            cardObj.transform.SetParent(GameManager.Instance.playerHandUI, false);

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

