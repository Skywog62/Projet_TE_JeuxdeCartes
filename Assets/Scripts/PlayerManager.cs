using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public string playerName;
    public int economyPoints = 0;
    public int cardPoints = 0;
    public int Health = 100; // Points de vie du joueur
    public Card championCard = null;

    public enum Strategy { Economic, Aggressive }
    public Strategy currentStrategy;

    public int EconomyStrength => economyPoints; // Pour l'affichage du scout

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

    public bool PickChampion(Card champion)
    {
        if (GameManager.Instance.TryPickChampion(champion))
        {
            championCard = champion;
            Debug.Log($"{playerName} a choisi le champion {champion.cardName}.");
            return true;
        }
        return false;
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;
        Debug.Log($"{playerName} subit {damage} dégâts. PV restants : {Health}");

        if (Health <= 0)
        {
            Debug.Log($"{playerName} a été vaincu !");
            GameManager.Instance.EndGame(false);
        }
    }
}
