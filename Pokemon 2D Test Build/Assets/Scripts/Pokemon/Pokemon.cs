using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Pokemon{

    //Used for testing, will be fixed later
    [SerializeField] PokemonBase _pokemonBase;
    public PokemonBase pokemonBase { get { return _pokemonBase; } set { _pokemonBase = value; } }
    public IndividualValues individualValues { get; set; }
    public EffortValues effortValues { get; set; }
    NatureBase _nature;

    string _currentName;
    //Used for testing, will be fixed later
    [SerializeField] int _level;
    public int currentLevel { get { return _level; } set { _level = value; } }
    public int currentHitPoints { get; set; }
    public int currentExperiencePoints { get; set; }

    public List<Move> moves { get; set; }

    public void Initialization()
    {

        currentName = currentName == null ? _pokemonBase.GetPokedexName() : currentName;

        individualValues = new IndividualValues();
        effortValues = new EffortValues();
        currentHitPoints = maxHitPoints;
        nature = SetNature();

        moves = new List<Move>();
        foreach(LearnableMove move in _pokemonBase.LearnableMoves)
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

    public NatureBase SetNature()
    {
        NatureBase[] natureBases;

        natureBases = Resources.LoadAll<NatureBase>("Natures");
        return natureBases[Random.Range(0, natureBases.Length)];
    }

    public DamageDetails TakeDamage(MoveBase move,Pokemon attackingPokemon)
    {
        DamageDetails damageDetails = new DamageDetails()
        {
            hasFainted =false,
            criticalHit = 1,
            typeEffectiveness = 1
        };

        damageDetails.typeEffectiveness = DamageModifiers.TypeChartEffectiveness(pokemonBase, move.type);

        if(damageDetails.typeEffectiveness == 0)//If it doesnt effect the pokemon then just end it right here
        {
            return damageDetails;
        }

        if(Random.value * 100 <= 6.25f)
        {
            damageDetails.criticalHit = 1.5f;
            //If sniper then make 2.25
        }

        float modifier = damageDetails.criticalHit * damageDetails.typeEffectiveness;
        modifier *= DamageModifiers.StandardRandomAttackPowerModifier();
        modifier *= DamageModifiers.SameTypeAttackBonus(move, attackingPokemon.pokemonBase);

        float attackPower = 0;
        float defendersDefense = 0;

        switch (move.moveType)
        {
            case MoveType.Physical:
                attackPower = attackingPokemon.attack;
                defendersDefense = defense;
                break;
            case MoveType.Special:
                attackPower = attackingPokemon.specialAttack;
                defendersDefense = specialDefense;
                break;
            case MoveType.Status:
                break;
            default:
                Debug.Log("BROKEN FORMULA");
                break;
        }

        int damage = Mathf.FloorToInt((((((2 * attackingPokemon.currentLevel) / 5) + 2) * move.power * attackPower / defendersDefense / 50) + 2) * modifier);

        if(damage <=0)
        {
            damage = 1;
        }

        currentHitPoints -= damage;

        if(currentHitPoints <= 0)
        {
            currentHitPoints = 0;
            damageDetails.hasFainted = true;
        }

        return damageDetails;
    }

    public Move ReturnRandomMove()
    {
        int r = Random.Range(0, moves.Count);
        return moves[r];
    }

    public string currentName
    {
        get { return _currentName; }
        set
        {
            _currentName = value;
        }
    }
}
