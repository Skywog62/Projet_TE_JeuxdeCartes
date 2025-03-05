using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public string playerName;
    public int economyPoints = 0;
    public int cardPoints = 0;
    public int Health = 100; // Ajout de la propriété Health

    public enum Strategy { Economic, Aggressive }
    public Strategy currentStrategy;

    public int EconomyStrength => economyPoints; // Pour le scout

    public void AddEconomyPoints(int amount)
    {
        economyPoints += amount;
        UpdateStrategy();
    }

    public void AddCardPoints(int amount)
    {
        cardPoints += amount;
        UpdateStrategy();
    }

    void UpdateStrategy()
    {
        currentStrategy = economyPoints > cardPoints ? Strategy.Economic : Strategy.Aggressive;
    }
}