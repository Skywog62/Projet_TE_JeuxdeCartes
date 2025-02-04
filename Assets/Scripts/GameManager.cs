using UnityEngine;
using System.Collections; // Ajout pour utiliser IEnumerator

public class GameManager : MonoBehaviour
{
    public GameObject cardPrefab; // Assigner un Prefab dans l'Inspector
    private Card cardA;
    private Card cardB;

    void Start()
    {
        // Trouver le GridManager en utilisant la m�thode correcte
        GridManager gridManager = FindFirstObjectByType<GridManager>();

        if (gridManager == null)
        {
            Debug.LogError("GameManager: Impossible de trouver GridManager !");
            return;
        }

        // D�marrer la coroutine pour attendre que la grille soit pr�te
        StartCoroutine(WaitForGridInitialization(gridManager));
    }

    IEnumerator WaitForGridInitialization(GridManager gridManager)
    {
        // Attendre que la grille soit g�n�r�e
        while (gridManager.gridCells == null)
        {
            yield return null; // Attendre une frame
        }

        // Placer les cartes sur des cellules sp�cifiques de la grille
        Vector3 posCardA = gridManager.GetCellPosition(0, 0); // Cellule (0,0)
        Vector3 posCardB = gridManager.GetCellPosition(0, 1); // Cellule (0,1)

        // Instancier les cartes sans red�clarer les variables
        GameObject cardAObj = Instantiate(cardPrefab, posCardA, Quaternion.identity);
        GameObject cardBObj = Instantiate(cardPrefab, posCardB, Quaternion.identity);

        // R�cup�rer les scripts des cartes
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


