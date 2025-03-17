using UnityEngine;
using TMPro;

public class GoldManager : MonoBehaviour
{
    public int currentGold = 0;
    public int goldPerTurn = 5; // Gain d'or par tour
    public int startingGold = 10; // Or de départ

    public TextMeshProUGUI goldText;

    void Start()
    {
        if (goldText == null)
        {
            goldText = FindFirstObjectByType<TextMeshProUGUI>();
            if (goldText == null)
            {
                Debug.LogError("GoldText n'est pas assigné dans GoldManager !");
                return;
            }
        }

        GainGold(startingGold);
    }

    public void GainGold(int amount)
    {
        currentGold += amount;
        UpdateGoldUI();
    }

    public bool SpendGold(int amount)
    {
        if (currentGold >= amount)
        {
            currentGold -= amount;
            UpdateGoldUI();
            return true;
        }
        return false;
    }

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

    public void EndTurn()
    {
        GainGold(goldPerTurn);
    }
}
