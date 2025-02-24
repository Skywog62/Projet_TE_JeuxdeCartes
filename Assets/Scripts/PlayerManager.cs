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
            Debug.Log($"{gameObject.name} ({player.playerName}) a re�u un �v�nement sp�cial !");
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
        if (championName == "") // V�rifie que le joueur n�a pas d�j� un champion
        {
            championName = champion;
            Debug.Log($"{playerName} a maintenant {champion} comme Champion !");
            return true;
        }
        else
        {
            Debug.Log($"{playerName} a d�j� un champion et ne peut pas changer !");
            return false;
        }
    }

}




