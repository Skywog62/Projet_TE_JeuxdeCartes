using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.XR;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public ScoutUI scoutUI;
    public GoldManager goldManager;
    public EventManager eventManager;
    public IA enemyAI;
    public PlayerManager player;

    public Transform playerHandUI;
    public Transform enemyHandUI;
    public TextMeshProUGUI playerDeckCounter;
    public TextMeshProUGUI enemyDeckCounter;
    public GameObject cardPrefab; // Assurez-vous que CardPrefab_UI est assigné ici !

    private Card selectedCard;
    private bool gameOver = false;
    private bool isPlayerTurn = true;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        playerHandUI = GameObject.Find("PlayerHand")?.transform;
        enemyHandUI = GameObject.Find("EnemyHand")?.transform;
        playerDeckCounter = GameObject.Find("PlayerDeckCounter")?.GetComponent<TextMeshProUGUI>();
        enemyDeckCounter = GameObject.Find("EnemyDeckCounter")?.GetComponent<TextMeshProUGUI>();

        player = FindFirstObjectByType<PlayerManager>();
        enemyAI = FindFirstObjectByType<IA>();

        if (player == null) Debug.LogError("❌ PlayerManager n'est pas trouvé !");
        if (enemyAI == null) Debug.LogError("❌ IA (enemyAI) non trouvée !");
        if (cardPrefab == null) Debug.LogError("❌ CardPrefab_UI n'est pas assigné dans l'Inspector !");

        if (goldManager != null)
            goldManager.GainGold(goldManager.startingGold);

        StartCoroutine(WaitForGridInitialization());
        UpdateDeckUI();
    }

    IEnumerator WaitForGridInitialization()
    {
        GridManager gridManager = FindFirstObjectByType<GridManager>();
        while (gridManager == null || gridManager.gridCells == null) yield return null;
    }

    public void SelectCard(Card card)
    {
        if (card == null)
        {
            Debug.LogError("❌ Sélection de carte invalide !");
            return;
        }
        selectedCard = card;
        Debug.Log($"✅ Carte sélectionnée : {selectedCard.cardName}");
    }

    public void CheckWinCondition()
    {
        if (player.Health <= 0)
        {
            EndGame(false);
        }
        else if (enemyAI.Health <= 0)
        {
            EndGame(true);
        }
    }

    public void EndGame(bool playerWon)
    {
        if (gameOver) return;

        gameOver = true;

        string result = playerWon ? "🏆 Victoire du Joueur !" : "☠️ Défaite du Joueur...";
        Debug.Log(result);
    }

    public void EndTurn()
    {
        if (!isPlayerTurn || gameOver) return;

        isPlayerTurn = false;
        enemyAI.PlayTurn();

        StartCoroutine(WaitForEnemyTurn());
    }

    IEnumerator WaitForEnemyTurn()
    {
        yield return new WaitForSeconds(2);
        isPlayerTurn = true;
    }

    public void UpdateDeckUI()
    {
        if (playerDeckCounter != null && enemyDeckCounter != null)
        {
            playerDeckCounter.text = player != null ? $"Deck : {player.deck.Count} cartes" : "Deck : N/A";
            enemyDeckCounter.text = enemyAI != null ? $"Deck IA : {enemyAI.deck.Count} cartes" : "Deck IA : N/A";
        }
    }

    public void DrawCard(PlayerManager player)
    {
        if (player == null || player.deck.Count == 0)
        {
            Debug.LogWarning("⚠️ Impossible de piocher, deck vide !");
            return;
        }

        Card drawnCard = player.deck[0];
        player.deck.RemoveAt(0);
        player.hand.Add(drawnCard);
        Debug.Log($"🃏 {player.playerName} pioche {drawnCard.cardName} !");

        if (GameManager.Instance == null || GameManager.Instance.playerHandUI == null)
        {
            Debug.LogWarning("⚠️ GameManager.Instance ou playerHandUI est NULL, impossible d'afficher la carte !");
            return;
        }

        GameObject cardObj = Instantiate(cardPrefab, playerHandUI);
        cardObj.transform.SetParent(playerHandUI, false);

        CardUI cardUI = cardObj.GetComponent<CardUI>();
        if (cardUI == null)
        {
            Debug.LogError("❌ CardUI n'est pas trouvé sur le prefab !");
            Destroy(cardObj);
            return;
        }

        cardUI.Setup(drawnCard);

        RectTransform rectTransform = cardObj.GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            float cardSpacing = 120; // Espacement entre les cartes
            rectTransform.anchoredPosition = new Vector2((player.hand.Count - 1) * cardSpacing, 0);
        }


        else
        {
            Debug.LogError("❌ Erreur : RectTransform introuvable sur le prefab !");
        }

        UpdateDeckUI();
    }
}
