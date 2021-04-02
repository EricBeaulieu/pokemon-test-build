using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EarnableEV
{
    [SerializeField]
    StatAttribute _statAttribute;
    [SerializeField]
    int _statValue;

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
public class EffortValues {

    int _hitPoints = 0;
    int _attack = 0;
    int _defense = 0;
    int _specialAttack = 0;
    int _specialDefense = 0;
    int _speed = 0;

    const int MAXIMUM_EV_Value = 255;
    const int MAXIMUM_EV_Total = 510;

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

    public int hitPoints
    {
        get { return _hitPoints; }
        private set
        {
            _hitPoints = value;
            if(_hitPoints > MAXIMUM_EV_Value)
            {
                _hitPoints = MAXIMUM_EV_Value;
            }
        }
    }

    public int attack
    {
        get { return _attack; }
        private set
        {
            _attack = value;
            if (_attack > MAXIMUM_EV_Value)
            {
                _attack = MAXIMUM_EV_Value;
            }
        }
    }

    public int defense
    {
        get { return _defense; }
        private set
        {
            _defense = value;
            if (_defense > MAXIMUM_EV_Value)
            {
                _defense = MAXIMUM_EV_Value;
            }
        }
    }

    public int specialAttack
    {
        get { return _specialAttack; }
        private set
        {
            _specialAttack = value;
            if (_specialAttack > MAXIMUM_EV_Value)
            {
                _specialAttack = MAXIMUM_EV_Value;
            }
        }
    }

    public int specialDefense
    {
        get { return _specialDefense; }
        private set
        {
            _specialDefense = value;
            if (_specialDefense > MAXIMUM_EV_Value)
            {
                _specialDefense = MAXIMUM_EV_Value;
            }
        }
    }

    public int speed
    {
        get { return _speed; }
        private set
        {
            _speed = value;
            if (_speed > MAXIMUM_EV_Value)
            {
                _speed = MAXIMUM_EV_Value;
            }
        }
    }
}
