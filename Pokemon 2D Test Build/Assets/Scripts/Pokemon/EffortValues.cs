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

    int _maxHitPoints = 0;
    int _attack = 0;
    int _defense = 0;
    int _specialAttack = 0;
    int _specialDefense = 0;
    int _speed = 0;

    const int MAXIMUM_EV_Value = 255;
    const int MAXIMUM_EV_Total = 510;

    public int maxHitPoints
    {
        get { return _maxHitPoints; }
        set
        {
            _maxHitPoints = value;
        }
    }

    public int attack
    {
        get { return _attack; }
        set
        {
            _attack = value;
        }
    }

    public int defense
    {
        get { return _defense; }
        set
        {
            _defense = value;
        }
    }

    public int specialAttack
    {
        get { return _specialAttack; }
        set
        {
            _specialAttack = value;
        }
    }

    public int specialDefense
    {
        get { return _specialDefense; }
        set
        {
            _specialDefense = value;
        }
    }

    public int speed
    {
        get { return _speed; }
        set
        {
            _speed = value;
        }
    }
}
