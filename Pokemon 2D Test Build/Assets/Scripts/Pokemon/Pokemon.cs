using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pokemon{

    public PokemonBase pokemonBase { get; set; }
    public IndividualValues individualValues { get; set; }
    public EffortValues effortValues { get; set; }
    NatureBase _nature;

    public string currentName { get; set; }
    public int currentLevel { get; set; }
    public int currentHitPoints { get; set; }
    public int currentExperiencePoints { get; set; }

    public List<Move> moves { get; set; }


    public Pokemon(PokemonBase pBase,int pLevel)
    {
        pokemonBase = pBase;
        currentName = pBase.GetPokedexName();
        currentLevel = pLevel;
        individualValues = new IndividualValues();
        effortValues = new EffortValues();
        currentHitPoints = maxHitPoints;

        moves = new List<Move>();
        foreach(LearnableMove move in pBase.LearnableMoves)
        {
            if(move.levelLearned <= currentLevel)
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

    public int maxHitPoints
    {
        get { return Mathf.FloorToInt(((individualValues.maxHitPoints + 2 * pokemonBase.maxHitPoints + (effortValues.maxHitPoints/4)) * currentLevel/100) + 10 + currentLevel); }
    }

    public int attack
    {
        get { return Mathf.FloorToInt((((individualValues.attack + 2 * pokemonBase.attack + (effortValues.attack / 4)) * currentLevel / 100) +5 ) * nature.NatureModifier(nature,StatAttribute.Attack)); }
    }

    public int defense
    {
        get { return Mathf.FloorToInt((((individualValues.defense + 2 * pokemonBase.defense + (effortValues.defense / 4)) * currentLevel / 100) + 5) * nature.NatureModifier(nature, StatAttribute.Defense)); }
    }

    public int specialAttack
    {
        get { return Mathf.FloorToInt((((individualValues.specialAttack + 2 * pokemonBase.specialAttack + (effortValues.specialAttack / 4)) * currentLevel / 100) + 5) * nature.NatureModifier(nature, StatAttribute.SpecialAttack)); }
    }

    public int specialDefense
    {
        get { return Mathf.FloorToInt((((individualValues.specialDefense + 2 * pokemonBase.specialDefense + (effortValues.specialDefense / 4)) * currentLevel / 100) + 5) * nature.NatureModifier(nature, StatAttribute.SpecialDefense)); }
    }

    public int speed
    {
        get { return Mathf.FloorToInt((((individualValues.speed + 2 * pokemonBase.speed + (effortValues.speed / 4)) * currentLevel / 100) + 5) * nature.NatureModifier(nature, StatAttribute.Speed)); }
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
