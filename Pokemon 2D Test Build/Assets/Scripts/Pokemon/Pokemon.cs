using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Pokemon{

    //Used for testing, will be fixed later
    [SerializeField] PokemonBase _pokemonBase;
    public PokemonBase pokemonBase { get { return _pokemonBase; } set { _pokemonBase = value; } }
    public bool isShiny { get; private set; }
    public IndividualValues individualValues { get; set; }
    public EffortValues effortValues { get; set; }
    NatureBase _nature;
    public Gender gender { get; set; }

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
    public List<Condition> volatileStatus { get; private set; }
    public int volatileStatusTime { get; set; }
    public System.Action OnStatusChanged;

    public void Initialization()
    {
        currentName = currentName == null ? _pokemonBase.GetPokedexName() : currentName;

        if(Random.value > 0.5f)
        {
            isShiny = true;
        }
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

        gender = SetGender(_pokemonBase);
    }

    /// <summary>
    /// resets all pokemon stats and volatile status, this is for when the pokmon is sent out or 
    /// when the battle is over to prevent bugs when pokemon is being viewed in the summary
    /// </summary>
    public void Reset()
    {
        ResetStatBoosts();
        volatileStatus = new List<Condition>();
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
        baseStats.Add(StatAttribute.Accuracy, 1);
        baseStats.Add(StatAttribute.Evasion, 1);
    }

    void ResetStatBoosts()
    {
        statBoosts = new Dictionary<StatAttribute, int>()
        {
            {StatAttribute.Attack,0 },
            {StatAttribute.Defense,0 },
            {StatAttribute.SpecialAttack,0 },
            {StatAttribute.SpecialDefense,0 },
            {StatAttribute.Speed,0 },
            {StatAttribute.Accuracy,0 },
            {StatAttribute.Evasion,0 }
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

    float GetStatAfterModification(StatAttribute currentStat)
    {
        float statValue = baseStats[currentStat];

        int boost = statBoosts[currentStat];
        float[] boostValues;

        if (currentStat == StatAttribute.Accuracy || currentStat == StatAttribute.Evasion)
        {
            boostValues = new float[] { 1f, 4f/3f, 5f / 3f, 6f / 3f, 7f / 3f, 8f / 3f, 9f / 3f };
        }
        else
        {
            boostValues = new float[] { 1f, 1.5f, 2f, 2.5f, 3f, 3.5f, 4f };
        }

        if (currentStat == StatAttribute.Accuracy || currentStat == StatAttribute.Evasion)
        {
            if (boost >= 0)
            {
                statValue = statValue * boostValues[boost];
            }
            else
            {
                statValue = statValue / boostValues[-boost];
            }
        }
        else
        {
            if (boost >= 0)
            {
                statValue = Mathf.FloorToInt(statValue * boostValues[boost]);
            }
            else
            {
                statValue = Mathf.FloorToInt(statValue / boostValues[-boost]);
            }
        }

        return statValue;
    }


    public int maxHitPoints { get; private set; }

    public int attack
    {
        get { return (int)GetStatAfterModification(StatAttribute.Attack); }
    }

    public int defense
    {
        get { return (int)GetStatAfterModification(StatAttribute.Defense); }
    }

    public int specialAttack
    {
        get { return (int)GetStatAfterModification(StatAttribute.SpecialAttack); }
    }

    public int specialDefense
    {
        get { return (int)GetStatAfterModification(StatAttribute.SpecialDefense); }
    }

    public int speed
    {
        get { return (int)GetStatAfterModification(StatAttribute.Speed); }
    }

    public float accuracy
    {
        get { return GetStatAfterModification(StatAttribute.Accuracy); }
    }

    public float evasion
    {
        get { return GetStatAfterModification(StatAttribute.Evasion); }
    }

    #endregion

    #region Nature/Gender

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

    Gender SetGender(PokemonBase pokemonBase)
    {
        Gender currentGender = Gender.NA;

        if(pokemonBase.HasGender == true)
        {
            float checker = Random.Range(1, 101);

            if (checker <= pokemonBase.MaleFemaleGenderRatio)//If true then male
            {
                currentGender = Gender.Male;
            }
            else
            {
                currentGender = Gender.Female;
            }
        }

        return currentGender;
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
            string currentStatusChange = $"{currentName} {status.HasConditionMessage}";

            if(status.HasCondition(conditionID) == false)
            {
                currentStatusChange = $"It doesnt affect {currentName}";
            }
            statusChanges.Enqueue(currentStatusChange);
            return;
        }

        status = ConditionsDB.Conditions[conditionID];
        status?.OnStart?.Invoke(this);

        if(status.StartMessage != null)
        {
            statusChanges.Enqueue($"{currentName} {status.StartMessage}");
        }

        OnStatusChanged?.Invoke();
    }

    public void CureStatus()
    {
        status = null;
        OnStatusChanged?.Invoke();
    }

    public void SetVolatileStatus(ConditionID conditionID)
    {
        Condition currentCondition = ConditionsDB.Conditions[conditionID];

        if (volatileStatus.Contains(currentCondition) == true)
        {
            string currentStatusChange = $"{currentName} {currentCondition.HasConditionMessage}";

            if (currentCondition.HasCondition(conditionID) == false)
            {
                currentStatusChange = $"It doesnt affect {currentName}";
            }
            statusChanges.Enqueue(currentStatusChange);
            return;
        }

        Condition newVolatileStatus = ConditionsDB.Conditions[conditionID];
        volatileStatus.Add(newVolatileStatus);
        newVolatileStatus?.OnStart?.Invoke(this);

        if(newVolatileStatus.StartMessage != null)
        {
            statusChanges.Enqueue($"{currentName} {newVolatileStatus.StartMessage}");
        }
    }

    public void CureAllVolatileStatus()
    {
        volatileStatus = new List<Condition>();
    }

    public void CureVolatileStatus(ConditionID conditionID)
    {
        volatileStatus.Remove(ConditionsDB.Conditions[conditionID]);
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

        foreach (Condition currentVolatileStatus in volatileStatus)
        {
            if (currentVolatileStatus?.OnBeforeMove != null)
            {
                if (currentVolatileStatus.OnBeforeMove(this) == false)
                {
                    canPerformMove = false;
                }
            }
        }

        return canPerformMove;
    }

    public void OnEndTurn()
    {
        status?.OnEndTurn?.Invoke(this);

        foreach (Condition currentVolatileStatus in volatileStatus)
        {
            currentVolatileStatus?.OnEndTurn?.Invoke(this);
        }
    }
}
