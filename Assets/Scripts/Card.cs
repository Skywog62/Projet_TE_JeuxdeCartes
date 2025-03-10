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
        if (gameObject.CompareTag("Untagged")) // Vérifie si le tag est vide
        {
            gameObject.tag = "Player"; // Définit les cartes du joueur comme "Player"
            Debug.Log($"{cardName} a maintenant le tag 'Player'");
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
        Debug.Log($"{cardName} subit {damage} dégâts AVANT : {defense} PV");

        if (cardEffect == EffectType.Absorb && !hasAbsorbedDamage)
        {
            Debug.Log($"{cardName} ignore les premiers dégâts grâce à Absorb !");
            hasAbsorbedDamage = true;
            return;
        }

        defense -= damage;
        Debug.Log($"{cardName} subit {damage} dégâts APRES : {defense} PV");

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
                    Debug.Log($"{cardName} est détruite !");
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
        if (!hasRevived) // ✅ Vérifie si la carte n’a pas déjà été ressuscitée
        {
            Debug.Log($"{cardName} ressuscite grâce à Revive !");
            defense = 5;
            hasRevived = true; // ✅ Marque la carte comme ayant déjà été ressuscitée
        }
        else
        {
            Debug.Log($"{cardName} ne peut pas ressusciter à nouveau !");
            Destroy(gameObject); // ✅ Supprime la carte si elle doit mourir définitivement
        }
    }

    private void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0)) // Clic gauche
        {
            GameManager.Instance.SelectCard(this);
        }
    }

    public void SetSelected(bool selected)
    {
        isSelected = selected;
        MeshRenderer mr = GetComponent<MeshRenderer>();
        if (mr != null)
        {
            mr.material.color = selected ? Color.yellow : Color.white;
        }
        else
        {
            Debug.LogWarning($"{cardName} n'a pas de MeshRenderer attaché !");
        }
    }
}
