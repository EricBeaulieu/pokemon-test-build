using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class IndividualValues
{

    [SerializeField]
    int _maxHitPoints;
    [SerializeField]
    int _attack;
    [SerializeField]
    int _defense;
    [SerializeField]
    int _specialAttack;
    [SerializeField]
    int _specialDefense;
    [SerializeField]
    int _speed;

    const int MINIMUM_IV_VALUE = 1;
    const int MAXIMUM_IV_VALUE = 31;

    public IndividualValues()
    {
        GenerateIVs();
    }

    void GenerateIVs()
    {
        maxHitPoints = Random.Range(MINIMUM_IV_VALUE, MAXIMUM_IV_VALUE);
        attack = Random.Range(MINIMUM_IV_VALUE, MAXIMUM_IV_VALUE);
        defense = Random.Range(MINIMUM_IV_VALUE, MAXIMUM_IV_VALUE);
        specialAttack = Random.Range(MINIMUM_IV_VALUE, MAXIMUM_IV_VALUE);
        specialDefense = Random.Range(MINIMUM_IV_VALUE, MAXIMUM_IV_VALUE);
        speed = Random.Range(MINIMUM_IV_VALUE, MAXIMUM_IV_VALUE);
    }

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
