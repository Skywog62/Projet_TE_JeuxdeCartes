using UnityEngine;
using System.Collections;
using System; // Import pour les �v�nements

public class GameManager : MonoBehaviour
{
    public GameObject cardPrefab;
    private Card cardA;
    private Card cardB;

    public event Action<PlayerManager> OnEventTriggered; // D�clare un �v�nement global

    void Start()
    {
        Debug.Log("GameManager: Initialisation du jeu...");

        // V�rifier si ChampionManager est bien pr�sent
        if (ChampionManager.instance == null)
        {
            Debug.LogError("ChampionManager n'est pas trouv� dans la sc�ne !");
            return;
        }

        // R�cup�rer les joueurs
        PlayerManager[] players = FindObjectsByType<PlayerManager>(FindObjectsSortMode.None);
        if (players.Length == 0)
        {
            Debug.LogError("Aucun joueur trouv� dans la sc�ne !");
            return;
        }

        // Liste des champions disponibles
        string[] availableChampions = { "Dragon", "Titan", "Ph�nix", "Samoura�" };

        foreach (PlayerManager player in players)
        {
            foreach (string champ in availableChampions)
            {
                if (ChampionManager.instance.AssignChampionToPlayer(player, champ))
                {
                    Debug.Log($"{player.playerName} a choisi {champ} comme champion !");
                    break; // D�s qu'un champion est attribu�, on sort de la boucle
                }
            }
        }

        // V�rifier si le GridManager est disponible
        GridManager gridManager = FindFirstObjectByType<GridManager>();
        if (gridManager == null)
        {
            Debug.LogError("GridManager n'est pas trouv� dans la sc�ne !");
            return;
        }

        // D�marrer l'initialisation de la grille avant de placer les cartes
        StartCoroutine(WaitForGridInitialization(gridManager));

        // Lancer la routine des �v�nements al�atoires
        StartCoroutine(RandomEventRoutine());

        Debug.Log("GameManager: Jeu initialis� avec succ�s !");
    }

    IEnumerator WaitForGridInitialization(GridManager gridManager)
    {
        Debug.Log("GameManager: Attente de l'initialisation de la grille...");

        while (!gridManager.IsGridReady())
        {
            yield return null; // Attendre une frame jusqu'� ce que la grille soit pr�te
        }

        Debug.Log("GameManager: Grille pr�te, placement des cartes...");

        if (cardPrefab == null)
        {
            Debug.LogError("cardPrefab n'est pas assign� dans l'Inspector !");
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

        Debug.Log("GameManager: Cartes plac�es avec succ�s !");
    }

    IEnumerator RandomEventRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(10f); // Attendre 10 secondes avant le prochain �v�nement

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
        int eventType = UnityEngine.Random.Range(0, 2); // 0 = �conomique, 1 = agressif

        if (eventType == 0)
        {
            player.AddEconomyPoints(5);
            Debug.Log($"{player.playerName} a choisi un bonus �conomique ! (+5 �conomie)");
        }
        else
        {
            player.AddCardPoints(5);
            Debug.Log($"{player.playerName} a choisi un bonus Agressif ! (+5 attaque)");
        }

        // D�clencher l'�v�nement pour tous les abonn�s
        OnEventTriggered?.Invoke(player);
    }
}
