using UnityEngine;

public class Card : MonoBehaviour
{
    public string cardName;
    public int attack;
    public int defense;

    public enum EffectType { None, Revive, Absorbe, Implacable }
    public EffectType cardEffect;

    private bool hasAbsorbedDamage = false;  // Pour l'effet Absorbe

    public void TakeDamage(int damage)
    {
        if (cardEffect == EffectType.Absorbe && !hasAbsorbedDamage)
        {
            Debug.Log($"{cardName} ignore les premiers d�g�ts gr�ce � Absorbe !");
            hasAbsorbedDamage = true;  // Annule le premier d�g�t
            return;
        }

        defense -= damage;
        Debug.Log($"{cardName} subit {damage} d�g�ts, PV restants : {defense}");

        if (defense <= 0)
        {
            if (cardEffect == EffectType.Revive)
            {
                Revive();
            }
            else
            {
                if (gameObject.scene.IsValid())  // V�rifie si l'objet est dans la sc�ne
                {
                    Destroy(gameObject);
                }
                else
                {
                    Debug.LogWarning($"{cardName} ne peut pas �tre d�truite car elle est un asset !");
                }
            }
        }
    }

    public void Attack(Card target)
    {
        Debug.Log($"{cardName} attaque {target.cardName} !");
        target.TakeDamage(attack);

        if (cardEffect == EffectType.Implacable)
        {
            Debug.Log($"{cardName} attaque une seconde fois gr�ce � Implacable !");
            target.TakeDamage(attack);
        }
    }

    private void Revive()
    {
        Debug.Log($"{cardName} ressuscite gr�ce � Revive !");
        defense = 5;  // On lui redonne des PV (ajuste selon ton �quilibrage)
    }
}


