using UnityEngine;
using System.Collections.Generic;

public class Card : MonoBehaviour
{
    public string cardName;
    public int attack;
    public int defense;
    public int cost;

    public enum EffectType { None, Revive, Absorb, Relentless }
    public EffectType cardEffect;

    [System.Serializable]
    public class PositionAbility
    {
        public int requiredRow;
        public int requiredColumn;
        public int attackBonus;
        public int defenseBonus;
    }

    public List<PositionAbility> positionAbilities = new List<PositionAbility>();
    private Vector2Int gridPosition;
    private bool hasAbsorbedDamage = false;
    private bool isSelected = false; // Pour gérer la sélection

    public bool CanPlayCard(GoldManager goldManager)
    {
        return goldManager.SpendGold(cost);
    }

    public void SetGridPosition(int row, int col)
    {
        gridPosition = new Vector2Int(row, col);
        ApplyPositionBonuses();
    }

    private void ApplyPositionBonuses()
    {
        foreach (var ability in positionAbilities)
        {
            if (gridPosition.x == ability.requiredRow && gridPosition.y == ability.requiredColumn)
            {
                attack += ability.attackBonus;
                defense += ability.defenseBonus;
            }
        }
    }

    public void TakeDamage(int damage)
    {
        if (cardEffect == EffectType.Absorb && !hasAbsorbedDamage)
        {
            Debug.Log($"{cardName} ignore les premiers dégâts grâce à Absorb !");
            hasAbsorbedDamage = true;
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
                if (gameObject.scene.IsValid())
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
        if (target == null)
        {
            Debug.LogError("La cible de l'attaque est null !");
            return;
        }

        Debug.Log($"{cardName} attaque {target.cardName} !");
        target.TakeDamage(attack);

        if (cardEffect == EffectType.Relentless)
        {
            Debug.Log($"{cardName} attaque une seconde fois grâce à Relentless !");
            target.TakeDamage(attack);
        }
    }

    private void Revive()
    {
        Debug.Log($"{cardName} ressuscite grâce à Revive !");
        defense = 5;
    }

    // Gestion de la sélection
    private void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0)) // Clic gauche
        {
            GameManager.Instance.SelectCard(this); // Sélectionne cette carte
        }
    }

    // Change l'apparence lorsqu'une carte est sélectionnée
    public void SetSelected(bool selected)
    {
        isSelected = selected;
        // Changez la couleur ou l'apparence de la carte pour indiquer la sélection
        GetComponent<SpriteRenderer>().color = selected ? Color.yellow : Color.white;
    }
}