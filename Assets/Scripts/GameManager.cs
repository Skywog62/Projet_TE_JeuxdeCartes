using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // Singleton pour accéder au GameManager

    public GameObject cardPrefab;
    public ScoutUI scoutUI;
    public GoldManager goldManager;

    private Card cardA;
    private Card cardB;
    private List<PlayerManager> opponents = new List<PlayerManager>();
    private int currentOpponentIndex = 0;

    private Card selectedCard; // Carte sélectionnée pour attaquer

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        goldManager.GainGold(goldManager.startingGold);

        // Initialisation des adversaires
        for (int i = 0; i < 3; i++)
        {
            GameObject opponentObj = new GameObject($"Opponent{i}");
            PlayerManager opponent = opponentObj.AddComponent<PlayerManager>();
            opponent.playerName = $"Opponent{i}";
            opponent.Health = 100;
            opponents.Add(opponent);
        }

        GridManager gridManager = FindFirstObjectByType<GridManager>();
        scoutUI.UpdateScoutInfo(opponents[currentOpponentIndex]);
        StartCoroutine(WaitForGridInitialization(gridManager));
    }

    IEnumerator WaitForGridInitialization(GridManager gridManager)
    {
        while (gridManager.gridCells == null) yield return null;

        Vector3 posCardA = gridManager.GetCellPosition(0, 0);
        Vector3 posCardB = gridManager.GetCellPosition(0, 1);

        GameObject cardAObj = Instantiate(cardPrefab, posCardA, Quaternion.identity);
        GameObject cardBObj = Instantiate(cardPrefab, posCardB, Quaternion.identity);

        cardA = cardAObj.GetComponent<Card>();
        cardB = cardBObj.GetComponent<Card>();

        // Configuration des cartes
        cardA.cardName = "Guerrier Revive";
        cardA.attack = 3;
        cardA.defense = 5;
        cardA.cost = 3;
        cardA.cardEffect = Card.EffectType.Revive;

        cardB.cardName = "Archer Relentless";
        cardB.attack = 2;
        cardB.defense = 4;
        cardB.cost = 2;
        cardB.cardEffect = Card.EffectType.Relentless;

        // Démarrez le combat contre les adversaires
        StartCoroutine(FightNextOpponent());
    }

    // Sélectionne une carte
    public void SelectCard(Card card)
    {
        if (selectedCard != null)
        {
            selectedCard.SetSelected(false); // Désélectionne la carte précédente
        }

        selectedCard = card;
        selectedCard.SetSelected(true); // Sélectionne la nouvelle carte
    }

    // Attaque une cible avec la carte sélectionnée
    public void AttackWithSelectedCard(Card target)
    {
        if (selectedCard != null && target != null)
        {
            selectedCard.Attack(target);
            selectedCard.SetSelected(false); // Désélectionne la carte après l'attaque
            selectedCard = null;
        }
        else
        {
            Debug.Log("Aucune carte sélectionnée ou cible invalide !");
        }
    }

    IEnumerator FightNextOpponent()
    {
        while (currentOpponentIndex < opponents.Count)
        {
            yield return StartCoroutine(CombatRoutine(opponents[currentOpponentIndex]));
            currentOpponentIndex++;
        }
    }

    IEnumerator CombatRoutine(PlayerManager opponent)
    {
        Debug.Log($"Combat contre {opponent.playerName} !");
        yield return new WaitForSeconds(1);
        opponent.Health -= 10;
        Debug.Log($"{opponent.playerName} a {opponent.Health} PV restants.");
        CheckWinCondition();
    }

    public void CheckWinCondition()
    {
        if (opponents.All(opponent => opponent.Health <= 0))
            Debug.Log("Victoire !");
        else if (FindFirstObjectByType<PlayerManager>().Health <= 0)
            Debug.Log("Défaite...");
    }

    // Gain de pièces à la fin du tour
    public void EndTurn()
    {
        goldManager.EndTurn();
    }
}