using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject cardPrefab;
    public ScoutUI scoutUI;
    public GoldManager goldManager;
    public EventManager eventManager;
    private IA enemyAI;

    public List<Card> availableChampions = new List<Card>();

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
        goldManager = FindFirstObjectByType<GoldManager>();
        eventManager = FindFirstObjectByType<EventManager>();
        enemyAI = FindFirstObjectByType<IA>();

        if (goldManager == null) Debug.LogError("GoldManager n'est pas trouvé dans la scène !");
        if (eventManager == null) Debug.LogError("EventManager n'est pas trouvé dans la scène !");
        if (enemyAI == null) Debug.LogError("L'IA (enemyAI) n'est pas trouvée !");

        goldManager.GainGold(goldManager.startingGold);
        StartCoroutine(WaitForGridInitialization());
    }

    IEnumerator WaitForGridInitialization()
    {
        GridManager gridManager = FindFirstObjectByType<GridManager>();
        while (gridManager.gridCells == null) yield return null;

        // Placement du joueur (en bas)
        Vector3 posCardPlayer = gridManager.GetCellPosition(3, 1);
        GameObject cardPlayerObj = Instantiate(cardPrefab, posCardPlayer, Quaternion.identity);
        Card cardPlayer = cardPlayerObj.GetComponent<Card>();
        cardPlayer.cardName = "Guerrier Revive";
        cardPlayer.attack = 3;
        cardPlayer.defense = 5;
        cardPlayer.cost = 3;
        cardPlayer.cardEffect = Card.EffectType.Revive;
        cardPlayerObj.tag = "Player";

        // Placement de l’IA (en haut)
        Vector3 posCardIA = gridManager.GetCellPosition(0, 1);
        GameObject cardIAObj = Instantiate(cardPrefab, posCardIA, Quaternion.identity);
        Card cardIA = cardIAObj.GetComponent<Card>();
        cardIA.cardName = "Archer Relentless";
        cardIA.attack = 2;
        cardIA.defense = 4;
        cardIA.cost = 2;
        cardIA.cardEffect = Card.EffectType.Relentless;
        cardIAObj.tag = "Enemy";
    }

    public void SelectCard(Card card)
    {
        if (!isPlayerTurn) return;

        if (selectedCard != null)
            selectedCard.SetSelected(false);

        selectedCard = card;
        selectedCard.SetSelected(true);
    }

    public void AttackWithSelectedCard(Card target)
    {
        if (!isPlayerTurn || selectedCard == null || target == null) return;

        List<Card> enemyCards = FindObjectsByType<Card>(FindObjectsSortMode.None)
            .Where(c => c.CompareTag("Enemy")).ToList();

        if (enemyCards.Count > 0 && !target.CompareTag("Enemy"))
        {
            Debug.Log("⚠️ Vous devez attaquer les cartes ennemies en premier !");
            return;
        }

        selectedCard.Attack(target);
        selectedCard.SetSelected(false);
        selectedCard = null;

        CheckWinCondition();
    }

    public void EndTurn()
    {
        if (!isPlayerTurn || gameOver) return;

        CheckWinCondition();

        if (!gameOver)
        {
            isPlayerTurn = false;
            Debug.Log("Tour du joueur terminé. Passage à l'IA...");
            StartCoroutine(EnemyTurn());
        }
    }

    IEnumerator EnemyTurn()
    {
        yield return new WaitForSeconds(1);

        Debug.Log("Tour de l'IA !");
        enemyAI.PlayTurn();

        yield return new WaitForSeconds(1.5f);

        CheckWinCondition();

        if (!gameOver)
        {
            isPlayerTurn = true;
            Debug.Log("Tour du joueur !");
        }
    }

    public void CheckWinCondition()
    {
        List<Card> playerCards = FindObjectsByType<Card>(FindObjectsSortMode.None)
            .Where(c => c.CompareTag("Player")).ToList();

        List<Card> enemyCards = FindObjectsByType<Card>(FindObjectsSortMode.None)
            .Where(c => c.CompareTag("Enemy")).ToList();

        PlayerManager player = FindFirstObjectByType<PlayerManager>();
        IA enemy = FindFirstObjectByType<IA>();

        if (playerCards.Count == 0)
        {
            Debug.Log("💥 Le joueur n'a plus de cartes, il prend 10 dégâts !");
            player.TakeDamage(10);
        }

        if (enemyCards.Count == 0)
        {
            Debug.Log("💥 L'IA n'a plus de cartes, elle prend 10 dégâts !");
            enemy.TakeDamage(10);
        }

        if (player.Health <= 0)
        {
            Debug.Log("💀 Défaite... Vous avez perdu !");
            EndGame(false);
        }

        if (enemy.Health <= 0)
        {
            Debug.Log("🎉 Victoire ! L'IA a perdu !");
            EndGame(true);
        }
    }

    public bool TryPickChampion(Card champion)
    {
        if (availableChampions.Contains(champion))
        {
            availableChampions.Remove(champion);
            return true;
        }
        return false;
    }


    public void EndGame(bool playerWon)
    {
        gameOver = true;
        isPlayerTurn = false;

        if (playerWon)
            Debug.Log("🏆 Vous avez gagné !");
        else
            Debug.Log("😵 Vous avez perdu...");

        GameObject endTurnButton = GameObject.Find("EndTurnButton");
        if (endTurnButton != null) endTurnButton.SetActive(false);
    }

    public void ForfeitGame()
    {
        Debug.Log("Le joueur a abandonné la partie.");
        EndGame(false);
    }
}
