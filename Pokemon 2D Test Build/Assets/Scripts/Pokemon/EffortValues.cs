using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EarnableEV
{
    [SerializeField] StatAttribute _statAttribute;
    [SerializeField] int _statValue;

    public StatAttribute statAttribute
    {
        get { return _statAttribute; }
    }

    public int statValue
    {
        get { return _statValue; }
    }
}

[System.Serializable]
public class EffortValues : SpecifiedValues
{
    const int MAXIMUM_EV_Value = 255;
    const int MAXIMUM_EV_Total = 510;

    public override void SetValues(SpecifiedValues effortValues)
    {
        if (effortValues == null) { return; }
        if (IsCorrectClass(effortValues) == false) { return; }

        hitPoints = effortValues.hitPoints;
        attack = effortValues.attack;
        defense = effortValues.defense;
        specialAttack = effortValues.specialAttack;
        specialDefense = effortValues.specialDefense;
        speed = effortValues.speed;
    }

    protected override bool IsCorrectClass(SpecifiedValues passedValue)
    {
        return (passedValue is EffortValues);
    }

    public void AddEffortValue(EarnableEV earnedEV)
    {
        if (TotalsEvEarned >= MAXIMUM_EV_Total)
        {
            return;
        }

        switch (earnedEV.statAttribute)
        {
            case StatAttribute.HitPoints:
                hitPoints += earnedEV.statValue;
                break;
            case StatAttribute.Attack:
                attack += earnedEV.statValue;
                break;
            case StatAttribute.Defense:
                defense += earnedEV.statValue;
                break;
            case StatAttribute.SpecialAttack:
                specialAttack += earnedEV.statValue;
                break;
            case StatAttribute.SpecialDefense:
                specialDefense += earnedEV.statValue;
                break;
            case StatAttribute.Speed:
                speed += earnedEV.statValue;
                break;
        }
    }

    int TotalsEvEarned
    {
        get { return hitPoints + attack + defense + specialAttack + specialDefense + speed; }
    }

    public override int hitPoints
    {
        get { return base.hitPoints; }
        protected set
        {
            base.hitPoints = value;
            if(base.hitPoints > MAXIMUM_EV_Value)
            {
                base.hitPoints = MAXIMUM_EV_Value;
            }
        }
    }

    public override int attack
    {
        get { return base.attack; }
        protected set
        {
            base.attack = value;
            if (base.attack > MAXIMUM_EV_Value)
            {
                base.attack = MAXIMUM_EV_Value;
            }
        }
    }

    public override int defense
    {
        get { return base.defense; }
        protected set
        {
            base.defense = value;
            if (base.defense > MAXIMUM_EV_Value)
            {
                base.defense = MAXIMUM_EV_Value;
            }
        }
    }

    public override int specialAttack
    {
        get { return base.specialAttack; }
        protected set
        {
            base.specialAttack = value;
            if (base.specialAttack > MAXIMUM_EV_Value)
            {
                base.specialAttack = MAXIMUM_EV_Value;
            }
        }
    }

    public override int specialDefense
    {
        get { return base.specialDefense; }
        protected set
        {
            base.specialDefense = value;
            if (base.specialDefense > MAXIMUM_EV_Value)
            {
                base.specialDefense = MAXIMUM_EV_Value;
            }
        }
    }

    public override int speed
    {
        get { return base.speed; }
        protected set
        {
            base.speed = value;
            if (base.speed > MAXIMUM_EV_Value)
            {
                base.speed = MAXIMUM_EV_Value;
            }
        }
    }
}
