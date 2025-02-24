using System.Collections.Generic;
using UnityEngine;

public class ChampionManager : MonoBehaviour
{
    public static ChampionManager instance; // Singleton pour le suivi des champions

    private List<string> takenChampions = new List<string>(); // Liste des champions déjà pris

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public bool IsChampionAvailable(string championName)
    {
        return !takenChampions.Contains(championName);
    }

    public bool AssignChampionToPlayer(PlayerManager player, string championName)
    {
        if (IsChampionAvailable(championName))
        {
            takenChampions.Add(championName);
            player.AssignChampion(championName);
            Debug.Log($"{player.playerName} a choisi {championName} comme Champion !");
            return true;
        }
        else
        {
            Debug.Log($"{championName} est déjà pris !");
            return false;
        }
    }
}

