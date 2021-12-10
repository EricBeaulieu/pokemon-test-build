using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EarnableEV
{
    [SerializeField] StatAttribute _statAttribute;
    [Range(1,3)]
    [SerializeField] int _statValue;

    public StatAttribute statAttribute
    {
        get { return _statAttribute; }
    }

    public int statValue
    {
        get { return _statValue; }
    }

    public EarnableEV(StatAttribute stat,int value)
    {
        _statAttribute = stat;
        _statValue = value;
    }
}

[System.Serializable]
public class EffortValues : SpecifiedValues
{
    const int MAXIMUM_EV_Value = 255;
    const int MAXIMUM_EV_Total = 510;

    public void SetValues(SpecifiedValues effortValues = null)
    {
        if(effortValues == null)
        {
            return;
        }
        hitPoints = effortValues.hitPoints;
        attack = effortValues.attack;
        defense = effortValues.defense;
        specialAttack = effortValues.specialAttack;
        specialDefense = effortValues.specialDefense;
        speed = effortValues.speed;
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

    public bool RemoveEffortValue(EarnableEV removedEV)
    {
        if (TotalsEvEarned <= 0)
        {
            return false;
        }

        if(removedEV.statValue <= 0)
        {
            return false;
        }

        switch (removedEV.statAttribute)
        {
            case StatAttribute.HitPoints:
                if(hitPoints >0)
                {
                    hitPoints -= removedEV.statValue;
                    return true;
                }
                return false;
            case StatAttribute.Attack:
                if (attack > 0)
                {
                    attack -= removedEV.statValue;
                    return true;
                }
                return false;
            case StatAttribute.Defense:
                if (defense > 0)
                {
                    defense -= removedEV.statValue;
                    return true;
                }
                return false;
            case StatAttribute.SpecialAttack:
                if (specialAttack > 0)
                {
                    specialAttack -= removedEV.statValue;
                    return true;
                }
                return false;
            case StatAttribute.SpecialDefense:
                if (specialDefense > 0)
                {
                    specialDefense -= removedEV.statValue;
                    return true;
                }
                return false;
            case StatAttribute.Speed:
                if (speed > 0)
                {
                    speed -= removedEV.statValue;
                    return true;
                }
                return false;
        }
        return false;
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
            Mathf.Clamp(base.hitPoints, 0, MAXIMUM_EV_Value);
        }
    }

    public override int attack
    {
        get { return base.attack; }
        protected set
        {
            base.attack = value;
            Mathf.Clamp(base.attack, 0, MAXIMUM_EV_Value);
        }
    }

    public override int defense
    {
        get { return base.defense; }
        protected set
        {
            base.defense = value;
            Mathf.Clamp(base.defense, 0, MAXIMUM_EV_Value);
        }
    }

    public override int specialAttack
    {
        get { return base.specialAttack; }
        protected set
        {
            base.specialAttack = value;
            Mathf.Clamp(base.specialAttack, 0, MAXIMUM_EV_Value);
        }
    }

    public override int specialDefense
    {
        get { return base.specialDefense; }
        protected set
        {
            base.specialDefense = value;
            Mathf.Clamp(base.specialDefense, 0, MAXIMUM_EV_Value);
        }
    }

    public override int speed
    {
        get { return base.speed; }
        protected set
        {
            base.speed = value;
            Mathf.Clamp(base.speed, 0, MAXIMUM_EV_Value);
        }
    }
}
