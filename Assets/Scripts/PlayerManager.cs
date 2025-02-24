using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public string playerName;
    public int economyPoints = 0;
    public int cardPoints = 0;

    public enum Strategy { Economic, Aggressive }
    public Strategy currentStrategy;

    void Start()
    {
        GameManager gameManager = FindFirstObjectByType<GameManager>();// Remplacement de FindFirstObjectByType

        if (gameManager != null)
        {
            gameManager.OnEventTriggered += OnGameEvent;
        }
    }

    void OnGameEvent(PlayerManager player)
    {
        if (player == this)
        {
            Debug.Log($"{gameObject.name} ({player.playerName}) a reçu un événement spécial !");
        }
    }

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

    public string championName = ""; // Stocke le nom du champion du joueur

    public bool AssignChampion(string champion)
    {
        if (championName == "") // Vérifie que le joueur n’a pas déjà un champion
        {
            championName = champion;
            Debug.Log($"{playerName} a maintenant {champion} comme Champion !");
            return true;
        }
        else
        {
            Debug.Log($"{playerName} a déjà un champion et ne peut pas changer !");
            return false;
        }
    }

}




