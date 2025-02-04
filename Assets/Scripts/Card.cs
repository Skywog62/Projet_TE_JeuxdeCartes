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
            Debug.Log($"{cardName} ignore les premiers dégâts grâce à Absorbe !");
            hasAbsorbedDamage = true;  // Annule le premier dégât
            return;
        }

        defense -= damage;
        Debug.Log($"{cardName} subit {damage} dégâts, PV restants : {defense}");

        if (defense <= 0)
        {
            if (cardEffect == EffectType.Revive)
            {
                Revive();
            }
            else
            {
                if (gameObject.scene.IsValid())  // Vérifie si l'objet est dans la scène
                {
                    Destroy(gameObject);
                }
                else
                {
                    Debug.LogWarning($"{cardName} ne peut pas être détruite car elle est un asset !");
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
            Debug.Log($"{cardName} attaque une seconde fois grâce à Implacable !");
            target.TakeDamage(attack);
        }
    }

    private void Revive()
    {
        Debug.Log($"{cardName} ressuscite grâce à Revive !");
        defense = 5;  // On lui redonne des PV (ajuste selon ton équilibrage)
    }
}


