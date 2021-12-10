using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class SpecifiedValues
{
    [SerializeField] int _hitPoints = 0;
    [SerializeField] int _attack = 0;
    [SerializeField] int _defense = 0;
    [SerializeField] int _specialAttack = 0;
    [SerializeField] int _specialDefense = 0;
    [SerializeField] int _speed = 0;

    public void LoadValues(int hp,int att,int def, int spAtt,int spDef,int spd)
    {
        hitPoints = hp;
        attack = att;
        defense = def;
        specialAttack = spAtt;
        specialDefense = spDef;
        speed = spd;
    }

    public virtual int hitPoints
    {
        get { return _hitPoints; }
        protected set
        {
            _hitPoints = value;
        }
    }

    public virtual int attack
    {
        get { return _attack; }
        protected set
        {
            _attack = value;
        }
    }

    public virtual int defense
    {
        get { return _defense; }
        protected set
        {
            _defense = value;
        }
    }

    public virtual int specialAttack
    {
        get { return _specialAttack; }
        protected set
        {
            _specialAttack = value;
        }
    }

    public virtual int specialDefense
    {
        get { return _specialDefense; }
        protected set
        {
            _specialDefense = value;
        }
    }

    public virtual int speed
    {
        get { return _speed; }
        protected set
        {
            _speed = value;
        }
    }

    public int ReturnValueAtIndex(int index)
    {
        switch (index)
        {
            case 0:
                return hitPoints;
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
