using UnityEngine;
using TMPro;

public class ScoutUI : MonoBehaviour
{
    public TextMeshProUGUI economyText;

    public void UpdateScoutInfo(PlayerManager player)
    {
        if (economyText == null)
        {
            Debug.LogError("EconomyText n'est pas assigné dans ScoutUI !");
            return;
        }

        string strength = player.EconomyStrength > 5 ? "<color=green>FORTE</color>" : "<color=red>FAIBLE</color>";
        string strategy = player.currentStrategy == PlayerManager.Strategy.Economic ? "Économique" : "Agressive";

        economyText.text = $"{player.playerName}\nStratégie : {strategy}\nÉconomie : {strength}";
    }
}

