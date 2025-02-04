using UnityEngine;
using System.Collections; // Ajout pour utiliser IEnumerator

public class GameManager : MonoBehaviour
{
    public GameObject cardPrefab; // Assigner un Prefab dans l'Inspector
    private Card cardA;
    private Card cardB;

    void Start()
    {
        // Trouver le GridManager en utilisant la méthode correcte
        GridManager gridManager = FindFirstObjectByType<GridManager>();

        if (gridManager == null)
        {
            Debug.LogError("GameManager: Impossible de trouver GridManager !");
            return;
        }

        // Démarrer la coroutine pour attendre que la grille soit prête
        StartCoroutine(WaitForGridInitialization(gridManager));
    }

    IEnumerator WaitForGridInitialization(GridManager gridManager)
    {
        // Attendre que la grille soit générée
        while (gridManager.gridCells == null)
        {
            yield return null; // Attendre une frame
        }

        // Placer les cartes sur des cellules spécifiques de la grille
        Vector3 posCardA = gridManager.GetCellPosition(0, 0); // Cellule (0,0)
        Vector3 posCardB = gridManager.GetCellPosition(0, 1); // Cellule (0,1)

        // Instancier les cartes sans redéclarer les variables
        GameObject cardAObj = Instantiate(cardPrefab, posCardA, Quaternion.identity);
        GameObject cardBObj = Instantiate(cardPrefab, posCardB, Quaternion.identity);

        // Récupérer les scripts des cartes
        cardA = cardAObj.GetComponent<Card>();
        cardB = cardBObj.GetComponent<Card>();

        // Configurer les cartes
        cardA.cardName = "Guerrier Revive";
        cardA.attack = 3;
        cardA.defense = 5;
        cardA.cardEffect = Card.EffectType.Revive;

        cardB.cardName = "Archer Implacable";
        cardB.attack = 2;
        cardB.defense = 4;
        cardB.cardEffect = Card.EffectType.Implacable;

        // Lancer le combat
        cardA.Attack(cardB);
        cardB.Attack(cardA);
    }
}


