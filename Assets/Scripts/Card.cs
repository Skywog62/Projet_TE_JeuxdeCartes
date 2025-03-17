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
    private bool isSelected = false;
    private bool hasRevived = false;

    void Start()
    {
        if (gameObject.CompareTag("Untagged"))
        {
            gameObject.tag = "Player";
        }
    }

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
            hasAbsorbedDamage = true;
            return;
        }

        defense -= damage;
        if (defense <= 0)
        {
            if (cardEffect == EffectType.Revive && !hasRevived)
            {
                defense = 5;
                hasRevived = true;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    public void Attack(Card target)
    {
        if (target == null) return;

        target.TakeDamage(attack);
        if (cardEffect == EffectType.Relentless)
        {
            target.TakeDamage(attack);
        }
    }

    private void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameManager.Instance.SelectCard(this);
        }
    }

    public void SetSelected(bool selected)
    {
        isSelected = selected;
    }
}

