using UnityEngine;
using NUnit.Framework;

public class UnitTests
{
    [Test]
    public void Card_TakesDamage()
    {
        Card card = new Card();
        card.defense = 10;
        card.TakeDamage(3);
        Assert.AreEqual(7, card.defense);
    }

    [Test]
    public void Card_DealsDamage()
    {
        Card attacker = new Card();
        Card target = new Card();
        attacker.attack = 5;
        target.defense = 10;

        attacker.Attack(target);
        Assert.AreEqual(5, target.defense);
    }

    [Test]
    public void Card_Revives()
    {
        Card card = new Card();
        card.cardEffect = Card.EffectType.Revive;
        card.defense = 0;
        card.TakeDamage(1);
        Assert.AreEqual(5, card.defense);
    }

    [Test]
    public void Card_AttacksTwice()
    {
        Card attacker = new Card();
        Card target = new Card();
        attacker.attack = 3;
        target.defense = 10;
        attacker.cardEffect = Card.EffectType.Relentless;

        attacker.Attack(target);
        Assert.AreEqual(4, target.defense);
    }

    [Test]
    public void Card_BlocksFirstDamage()
    {
        Card card = new Card();
        card.cardEffect = Card.EffectType.Absorb;
        card.defense = 10;

        card.TakeDamage(5);
        Assert.AreEqual(10, card.defense);

        card.TakeDamage(5);
        Assert.AreEqual(5, card.defense);
    }
}
