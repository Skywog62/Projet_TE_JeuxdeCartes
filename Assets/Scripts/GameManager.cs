using UnityEngine;
using System.Collections;
using System; // Import pour les événements

public class GameManager : MonoBehaviour
{
    public GameObject cardPrefab;
    private Card cardA;
    private Card cardB;

    public event Action<PlayerManager> OnEventTriggered; // Déclare un événement global

    void Start()
    {
        Debug.Log("GameManager: Initialisation du jeu...");

        // Vérifier si ChampionManager est bien présent
        if (ChampionManager.instance == null)
        {
            Debug.LogError("ChampionManager n'est pas trouvé dans la scène !");
            return;
        }

        // Récupérer les joueurs
        PlayerManager[] players = FindObjectsByType<PlayerManager>(FindObjectsSortMode.None);
        if (players.Length == 0)
        {
            Debug.LogError("Aucun joueur trouvé dans la scène !");
            return;
        }

        // Liste des champions disponibles
        string[] availableChampions = { "Dragon", "Titan", "Phénix", "Samouraï" };

        foreach (PlayerManager player in players)
        {
            foreach (string champ in availableChampions)
            {
                if (ChampionManager.instance.AssignChampionToPlayer(player, champ))
                {
                    Debug.Log($"{player.playerName} a choisi {champ} comme champion !");
                    break; // Dès qu'un champion est attribué, on sort de la boucle
                }
            }
        }

        // Vérifier si le GridManager est disponible
        GridManager gridManager = FindFirstObjectByType<GridManager>();
        if (gridManager == null)
        {
            Debug.LogError("GridManager n'est pas trouvé dans la scène !");
            return;
        }

        // Démarrer l'initialisation de la grille avant de placer les cartes
        StartCoroutine(WaitForGridInitialization(gridManager));

        // Lancer la routine des événements aléatoires
        StartCoroutine(RandomEventRoutine());

        Debug.Log("GameManager: Jeu initialisé avec succès !");
    }

    IEnumerator WaitForGridInitialization(GridManager gridManager)
    {
        Debug.Log("GameManager: Attente de l'initialisation de la grille...");

        while (!gridManager.IsGridReady())
        {
            yield return null; // Attendre une frame jusqu'à ce que la grille soit prête
        }

        Debug.Log("GameManager: Grille prête, placement des cartes...");

        if (cardPrefab == null)
        {
            Debug.LogError("cardPrefab n'est pas assigné dans l'Inspector !");
            yield break;
        }

        Vector3 posCardA = gridManager.GetCellPosition(0, 0);
        Vector3 posCardB = gridManager.GetCellPosition(0, 1);

        GameObject cardAObj = Instantiate(cardPrefab, posCardA, Quaternion.identity);
        GameObject cardBObj = Instantiate(cardPrefab, posCardB, Quaternion.identity);

        cardA = cardAObj.GetComponent<Card>();
        cardB = cardBObj.GetComponent<Card>();

        cardA.cardName = "Guerrier Revive";
        cardA.attack = 3;
        cardA.defense = 5;
        cardA.cardEffect = Card.EffectType.Revive;

        cardB.cardName = "Archer Implacable";
        cardB.attack = 2;
        cardB.defense = 4;
        cardB.cardEffect = Card.EffectType.Implacable;

        cardA.Attack(cardB);
        cardB.Attack(cardA);

        Debug.Log("GameManager: Cartes placées avec succès !");
    }

    IEnumerator RandomEventRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(10f); // Attendre 10 secondes avant le prochain événement

            PlayerManager[] allPlayers = FindObjectsByType<PlayerManager>(FindObjectsSortMode.None);
            if (allPlayers.Length > 0)
            {
                PlayerManager randomPlayer = allPlayers[UnityEngine.Random.Range(0, allPlayers.Length)];

                if (randomPlayer != null)
                {
                    TriggerRandomEvent(randomPlayer);
                }
            }
        }
    }

    void TriggerRandomEvent(PlayerManager player)
    {
        int eventType = UnityEngine.Random.Range(0, 2); // 0 = économique, 1 = agressif

        if (eventType == 0)
        {
            player.AddEconomyPoints(5);
            Debug.Log($"{player.playerName} a choisi un bonus Économique ! (+5 économie)");
        }
        else
        {
            player.AddCardPoints(5);
            Debug.Log($"{player.playerName} a choisi un bonus Agressif ! (+5 attaque)");
        }

        // Déclencher l'événement pour tous les abonnés
        OnEventTriggered?.Invoke(player);
    }
}
