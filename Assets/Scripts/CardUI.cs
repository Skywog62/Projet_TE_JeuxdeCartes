using UnityEngine;
using TMPro;

public class CardUI : MonoBehaviour
{
    public TextMeshProUGUI cardNameText;
    public TextMeshProUGUI attackText;

    private void Awake()
    {
        // V�rifier si les r�f�rences UI sont bien assign�es
        if (cardNameText == null || attackText == null)
        {
            Debug.LogError("CardUI : R�f�rences UI non assign�es sur " + gameObject.name);
        }
    }

    public void Setup(Card card)
    {
        if (card == null)
        {
            Debug.LogError("CardUI : La carte re�ue est NULL !");
            return;
        }

        // V�rifier que les r�f�rences existent avant d�assigner les valeurs
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

