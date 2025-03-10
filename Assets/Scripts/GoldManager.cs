using UnityEngine;
using TMPro;

public class GoldManager : MonoBehaviour
{
    public int currentGold = 0;
    public int goldPerTurn = 5; // Gain d'or par tour
    public int startingGold = 10; // Or de départ

    public TextMeshPro goldText;

    void Start()
    {
        if (goldText == null)
        {
            Debug.LogError("GoldText n'est pas assigné dans GoldManager !");
            return;
        }

        // Donne l'or de départ au joueur
        GainGold(startingGold);
    }

    // Ajoute de l'or au joueur
    public void GainGold(int amount)
    {
        currentGold += amount;
        UpdateGoldUI();
    }

    // Dépense de l'or
    public bool SpendGold(int amount)
    {
        if (currentGold >= amount)
        {
            currentGold -= amount;
            UpdateGoldUI();
            return true;
        }
        return false; // Pas assez d'or
    }

    // Met à jour l'affichage de l'or
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

    // Gagne de l'or à la fin du tour
    public void EndTurn()
    {
        GainGold(goldPerTurn);
    }
}