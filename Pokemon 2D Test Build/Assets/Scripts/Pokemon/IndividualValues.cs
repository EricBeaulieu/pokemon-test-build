using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class IndividualValues
{
    public static int VALUES_SAVED_LENGTH = 6;

    [SerializeField] int _maxHitPoints = 0;
    [SerializeField] int _attack = 0;
    [SerializeField] int _defense = 0;
    [SerializeField] int _specialAttack = 0;
    [SerializeField] int _specialDefense = 0;
    [SerializeField] int _speed = 0;

    const int MINIMUM_IV_VALUE = 1;
    const int MAXIMUM_IV_VALUE = 31;

    public void SetIVs(IndividualValues individualValues)
    {
        if(individualValues == null)
        {
            GenerateIVs();
            return;
        }
        _maxHitPoints = individualValues.maxHitPoints == 0 ? Random.Range(MINIMUM_IV_VALUE, MAXIMUM_IV_VALUE) : individualValues.maxHitPoints;
        _attack = individualValues.attack == 0 ? Random.Range(MINIMUM_IV_VALUE, MAXIMUM_IV_VALUE) : individualValues.attack;
        _defense = individualValues.defense == 0 ? Random.Range(MINIMUM_IV_VALUE, MAXIMUM_IV_VALUE) : individualValues.defense;
        _specialAttack = individualValues.specialAttack == 0 ? Random.Range(MINIMUM_IV_VALUE, MAXIMUM_IV_VALUE) : individualValues.specialAttack;
        _specialDefense = individualValues.specialDefense == 0 ? Random.Range(MINIMUM_IV_VALUE, MAXIMUM_IV_VALUE) : individualValues.specialDefense;
        _speed = individualValues.speed == 0 ? Random.Range(MINIMUM_IV_VALUE, MAXIMUM_IV_VALUE) : individualValues.speed;
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

    public int ReturnValueAtIndex(int index)
    {
        switch (index)
        {
            case 0:
                return maxHitPoints;
            case 1:
                return attack;
            case 2:
                return defense;
            case 3:
                return specialAttack;
            case 4:
                return specialDefense;
            case 5:
                return speed;
            default:
                Debug.LogError("Index out of range");
                return 0;
        }
    }
}
