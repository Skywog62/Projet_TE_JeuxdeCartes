using UnityEngine;
using TMPro;

public class GoldManager : MonoBehaviour
{
    public int currentGold = 0;
    public int goldPerTurn = 5; // Pièces gagnées par tour
    public int startingGold = 10; // Pièces de départ

    public TextMeshPro goldText; // Assurez-vous que c'est bien assigné dans l'Inspector

    void Start()
    {
        if (goldText == null)
        {
            Debug.LogError("GoldText n'est pas assigné dans GoldManager !");
            return;
        }

        // Donne les pièces de départ au joueur
        GainGold(startingGold);
    }

    // Gagne des pièces
    public void GainGold(int amount)
    {
        currentGold += amount;
        UpdateGoldUI(); // Met à jour l'UI après chaque gain
    }

    // Dépense des pièces
    public bool SpendGold(int amount)
    {
        if (currentGold >= amount)
        {
            currentGold -= amount;
            UpdateGoldUI(); // Met à jour l'UI après chaque dépense
            return true; // Dépense réussie
        }
        return false; // Pas assez de pièces
    }

    // Met à jour l'UI des pièces
    private void UpdateGoldUI()
    {
        if (goldText != null)
        {
            goldText.text = $"Or : {currentGold}";
        }
        else
        {
            Debug.LogWarning("GoldText n'est pas assigné !");
        }
    }

    // Gain de pièces à la fin du tour
    public void EndTurn()
    {
        GainGold(goldPerTurn);
    }
}