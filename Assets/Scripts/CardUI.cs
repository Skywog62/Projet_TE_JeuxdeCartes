using UnityEngine;
using TMPro;

public class CardUI : MonoBehaviour
{
    public TextMeshProUGUI cardNameText;
    public TextMeshProUGUI attackText;

    private void Awake()
    {
        // Vérifier si les références UI sont bien assignées
        if (cardNameText == null || attackText == null)
        {
            Debug.LogError("CardUI : Références UI non assignées sur " + gameObject.name);
        }
    }

    public void Setup(Card card)
    {
        if (card == null)
        {
            Debug.LogError("CardUI : La carte reçue est NULL !");
            return;
        }

        // Vérifier que les références existent avant d’assigner les valeurs
        if (cardNameText != null)
            cardNameText.text = card.cardName;
        else
            Debug.LogError("CardUI : 'cardNameText' est NULL pour " + gameObject.name);

        if (attackText != null)
            attackText.text = card.attack.ToString();
        else
            Debug.LogError("CardUI : 'attackText' est NULL pour " + gameObject.name);
    }
}

