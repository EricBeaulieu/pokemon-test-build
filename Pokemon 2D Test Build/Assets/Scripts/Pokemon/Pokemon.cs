using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pokemon{

    PokemonBase _base;
    IndividualValues _individualValues;
    EffortValues _effortValues;
    NatureBase _nature;

    int _level;
    int _currHitPoints;

    public List<Move> moves { get; set; }


    Pokemon(PokemonBase pBase,int pLevel)
    {
        _base = pBase;
        _level = pLevel;
        _individualValues = new IndividualValues();
        _effortValues = new EffortValues();

        moves = new List<Move>();
        foreach(LearnableMove move in pBase.LearnableMoves)
        {
            if(move.levelLearned <= _level)
            {
                if(moves.Count >=4)
                {
                    moves.RemoveAt(0);
                }
                moves.Add(new Move(move.moveBase));
            }
        }
    }

    #region Stats

    public int currentHitPoints
    {
        get { return _currHitPoints; }
        set
        {
            _currHitPoints = value;
        }
    }

    public int maxHitPoints
    {
        get { return Mathf.FloorToInt(((_individualValues.maxHitPoints + 2 * _base.maxHitPoints + (_effortValues.maxHitPoints/4)) * _level/100) + 10 + _level); }
    }

    public int attack
    {
        get { return Mathf.FloorToInt((((_individualValues.attack + 2 * _base.attack + (_effortValues.attack / 4)) * _level/100) +5 ) * nature.NatureModifier(nature,StatAttribute.Attack)); }
    }

    public int defense
    {
        get { return Mathf.FloorToInt((((_individualValues.defense + 2 * _base.defense + (_effortValues.defense / 4)) * _level / 100) + 5) * nature.NatureModifier(nature, StatAttribute.Defense)); }
    }

    public int specialAttack
    {
        get { return Mathf.FloorToInt((((_individualValues.specialAttack + 2 * _base.specialAttack + (_effortValues.specialAttack / 4)) * _level / 100) + 5) * nature.NatureModifier(nature, StatAttribute.SpecialAttack)); }
    }

    public int specialDefense
    {
        get { return Mathf.FloorToInt((((_individualValues.specialDefense + 2 * _base.specialDefense + (_effortValues.specialDefense / 4)) * _level / 100) + 5) * nature.NatureModifier(nature, StatAttribute.SpecialDefense)); }
    }

    public int speed
    {
        get { return Mathf.FloorToInt((((_individualValues.speed + 2 * _base.speed + (_effortValues.speed / 4)) * _level / 100) + 5) * nature.NatureModifier(nature, StatAttribute.Speed)); }
    }

    #endregion

    public NatureBase nature
    {
        get { return _nature; }
        set
        {
            _nature = value;
        }
    }
}
