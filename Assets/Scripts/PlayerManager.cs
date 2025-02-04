using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public string playerName;
    public int economyPoints = 0;
    public int cardPoints = 0;

    public enum Strategy { Economic, Aggressive }
    public Strategy currentStrategy;

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

