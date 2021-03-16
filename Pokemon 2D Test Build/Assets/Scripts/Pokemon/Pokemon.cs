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
    public Dictionary<StatAttribute, int> baseStats { get; private set; }
    public Dictionary<StatAttribute, int> statBoosts { get; private set; }
    public Queue<string> statusChanges { get; private set; } = new Queue<string>();

    public Condition status { get; private set; }
    public int statusTime { get; set; }
    public Condition volatileStatus { get; private set; }
    public int volatileStatusTime { get; set; }
    public System.Action OnStatusChanged;

    public void Initialization()
    {

        currentName = currentName == null ? _pokemonBase.GetPokedexName() : currentName;

        individualValues = new IndividualValues();
        effortValues = new EffortValues();
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

        SetDataStats();
        currentHitPoints = maxHitPoints;

        

    }

    /// <summary>
    /// resets all pokemon stats and volatile status, this is for when the pokmon is sent out or 
    /// when the battle is over to prevent bugs when pokemon is being viewed in the summary
    /// </summary>
    public void Reset()
    {
        ResetStatBoosts();
        volatileStatus = null;
    }

    #region Stats

    void SetDataStats()
    {
        baseStats = new Dictionary<StatAttribute, int>();

        maxHitPoints = Mathf.FloorToInt(((individualValues.maxHitPoints + 2 * pokemonBase.maxHitPoints + (effortValues.maxHitPoints / 4)) * currentLevel / 100) + 10 + currentLevel);
        baseStats.Add(StatAttribute.Attack, Mathf.FloorToInt((((individualValues.attack + 2 * pokemonBase.attack + (effortValues.attack / 4)) * currentLevel / 100) + 5) * nature.NatureModifier(nature, StatAttribute.Attack)));
        baseStats.Add(StatAttribute.Defense, Mathf.FloorToInt((((individualValues.defense + 2 * pokemonBase.defense + (effortValues.defense / 4)) * currentLevel / 100) + 5) * nature.NatureModifier(nature, StatAttribute.Defense)));
        baseStats.Add(StatAttribute.SpecialAttack, Mathf.FloorToInt((((individualValues.specialAttack + 2 * pokemonBase.specialAttack + (effortValues.specialAttack / 4)) * currentLevel / 100) + 5) * nature.NatureModifier(nature, StatAttribute.SpecialAttack)));
        baseStats.Add(StatAttribute.SpecialDefense, Mathf.FloorToInt((((individualValues.specialDefense + 2 * pokemonBase.specialDefense + (effortValues.specialDefense / 4)) * currentLevel / 100) + 5) * nature.NatureModifier(nature, StatAttribute.SpecialDefense)));
        baseStats.Add(StatAttribute.Speed, Mathf.FloorToInt((((individualValues.speed + 2 * pokemonBase.speed + (effortValues.speed / 4)) * currentLevel / 100) + 5) * nature.NatureModifier(nature, StatAttribute.Speed)));
    }

    void ResetStatBoosts()
    {
        statBoosts = new Dictionary<StatAttribute, int>()
        {
            {StatAttribute.Attack,0 },
            {StatAttribute.Defense,0 },
            {StatAttribute.SpecialAttack,0 },
            {StatAttribute.SpecialDefense,0 },
            {StatAttribute.Speed,0 }
        };
    }

    public void ApplyStatModifier(List<StatBoost> currentBoostModifiers)
    {
        foreach (var modifier in currentBoostModifiers)
        {
            if(modifier.stat == StatAttribute.NA)
            {
                continue;
            }

            StatAttribute statModified = modifier.stat;
            int boost = modifier.boost;

            statBoosts[statModified] = Mathf.Clamp(statBoosts[statModified] + boost, -6, 6);

            if(boost > 0)
            {
                statusChanges.Enqueue($"{currentName}'s {statModified} rose!");
            }
            else
            {
                statusChanges.Enqueue($"{currentName}'s {statModified} fell!");
            }

            Debug.Log($"{currentName} {statModified} has been changed to {statBoosts[statModified]}");
        }
    }

    int GetStatAfterModification(StatAttribute currentStat)
    {
        int statValue = baseStats[currentStat];

        int boost = statBoosts[currentStat];
        float[] boostValues = new float[] { 1f, 1.5f, 2f, 2.5f, 3f, 3.5f, 4f };

        if(boost >= 0)
        {
            statValue = Mathf.FloorToInt(statValue * boostValues[boost]);
        }
        else
        {
            statValue = Mathf.FloorToInt(statValue / boostValues[-boost]);
        }

        return statValue;
    }

    public int maxHitPoints { get; private set; }

    public int attack
    {
        get { return GetStatAfterModification(StatAttribute.Attack); }
    }

    public int defense
    {
        get { return GetStatAfterModification(StatAttribute.Defense); }
    }

    public int specialAttack
    {
        get { return GetStatAfterModification(StatAttribute.SpecialAttack); }
    }

    public int specialDefense
    {
        get { return GetStatAfterModification(StatAttribute.SpecialDefense); }
    }

    public int speed
    {
        get { return GetStatAfterModification(StatAttribute.Speed); }
    }

    #endregion

    #region Nature

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

    #endregion

    public DamageDetails TakeDamage(MoveBase move,Pokemon attackingPokemon)
    {
        DamageDetails damageDetails = new DamageDetails()
        {
            hasFainted =false,
            criticalHit = 1,
            typeEffectiveness = 1
        };

        damageDetails.typeEffectiveness = DamageModifiers.TypeChartEffectiveness(pokemonBase, move.Type);

        if(damageDetails.typeEffectiveness == 0)//If it doesnt effect the pokemon then just end it right here
        {
            return damageDetails;
        }


        //Critical Hit Chance
        if(Random.value * 100 <= 6.25f)
        {
            damageDetails.criticalHit = 1.5f;
            //If sniper then make 2.25
        }

        float modifier = damageDetails.criticalHit * damageDetails.typeEffectiveness;
        modifier *= DamageModifiers.StandardRandomAttackPowerModifier();
        modifier *= DamageModifiers.SameTypeAttackBonus(move, attackingPokemon.pokemonBase);

        float attackPower = (move.MoveType == MoveType.Physical) ? attackingPokemon.attack: attackingPokemon.specialAttack;
        float defendersDefense = (move.MoveType == MoveType.Physical) ? defense : specialDefense;

        int damage = Mathf.FloorToInt((((((2 * attackingPokemon.currentLevel) / 5) + 2) * move.MovePower * attackPower / defendersDefense / 50) + 2) * modifier);

        if(damage <=0)
        {
            damage = 1;
        }

        UpdateHP(damage);

        return damageDetails;
    }

    public void UpdateHP(int damage)
    {
        currentHitPoints = Mathf.Clamp(currentHitPoints - damage, 0, maxHitPoints);
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

    public void SetStatus(ConditionID conditionID)
    {
        if(status != null)
        {
            statusChanges.Enqueue($"It doesnt affect {currentName}");
            return;
        }

        status = ConditionsDB.Conditions[conditionID];
        status?.OnStart?.Invoke(this);
        statusChanges.Enqueue($"{currentName} {status.StartMessage}");

        OnStatusChanged?.Invoke();
    }

    public void CureStatus()
    {
        status = null;
        OnStatusChanged?.Invoke();
    }

    public void SetVolatileStatus(ConditionID conditionID)
    {
        if (volatileStatus != null)
        {
            statusChanges.Enqueue($"It doesnt affect {currentName}");
            return;
        }

        volatileStatus = ConditionsDB.Conditions[conditionID];
        volatileStatus?.OnStart?.Invoke(this);
        statusChanges.Enqueue($"{currentName} {volatileStatus.StartMessage}");
    }

    public void CureVolatileStatus()
    {
        volatileStatus = null;
    }

    /// <summary>
    /// Checks to see if the pokemon can attack or not
    /// </summary>
    /// <returns></returns>
    public bool OnBeforeMove()
    {
        bool canPerformMove = true;

        if(status?.OnBeforeMove != null)
        {
            if (status.OnBeforeMove(this) == false)
            {
                canPerformMove = false;
            }
        }

        if (volatileStatus?.OnBeforeMove != null)
        {
            if (volatileStatus.OnBeforeMove(this) == false)
            {
                canPerformMove = false;
            }
        }

        return canPerformMove;
    }

    public void OnEndTurn()
    {
        status?.OnEndTurn?.Invoke(this);
    }
}
