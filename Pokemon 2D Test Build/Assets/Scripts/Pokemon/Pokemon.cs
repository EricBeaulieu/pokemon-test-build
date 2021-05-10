using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class Pokemon {

    //Used for testing, will be fixed later
    [SerializeField] PokemonBase _pokemonBase;
    public PokemonBase pokemonBase { get { return _pokemonBase; } set { _pokemonBase = value; } }
    public bool isShiny { get; private set; }
    public IndividualValues individualValues { get; set; }
    public EffortValues effortValues { get; set; }
    NatureBase _nature;
    public Ability ability { get; private set; }
    public Gender gender { get; set; }

    string _currentName;
    [SerializeField] int _level;
    public int currentLevel { get { return _level; } private set { _level = value; } }
    public int currentHitPoints { get; set; }
    public int currentExp { get; set; }
    public string originalTrainer { get; private set; }
    public string originalTrainerID { get; private set; }
    public PokeballItem pokeballCapturedIn { get; private set; }

    public List<Move> moves { get; set; }
    public Dictionary<StatAttribute, int> baseStats { get; private set; }
    public Dictionary<StatAttribute, int> statBoosts { get; private set; }
    public Queue<string> statusChanges { get; private set; }

    public Condition status { get; private set; }
    public int statusTime { get; set; }
    public List<Condition> volatileStatus { get; private set; }
    public int volatileStatusTime { get; set; }
    public System.Action OnStatusChanged;

    public Pokemon(PokemonBase pokemonBase,int level)
    {
        _pokemonBase = pokemonBase;
        currentLevel = level;

        NewInitialization();
    }

    public Pokemon(PokemonBase pokemonBase, int level,Gender savedGender,NatureBase nature, IndividualValues iV,EffortValues eV, string nickname = null,bool shiny = false)
    {
        _pokemonBase = pokemonBase;
        currentLevel = level;

        gender = savedGender;
        _nature = nature;

        statusChanges = new Queue<string>();

        individualValues = iV;
        effortValues = eV;

        currentName = currentName == null ? _pokemonBase.GetPokedexName() : nickname;
        isShiny = shiny;
        LoadedInitialization();
    }

    void NewInitialization()
    {
        _currentName = null;

        if(Random.value > 0.5f)
        {
            isShiny = true;
        }
        
        nature = SetNature();

        moves = new List<Move>();
        foreach(LearnableMove move in _pokemonBase.LearnableMoves)
        {
            if(move.levelLearned <= currentLevel)
            {
                if (moves.Exists(x => x.moveBase == move.moveBase) == true)
                {
                    continue;
                }

                if (moves.Count >=PokemonBase.MAX_NUMBER_OF_MOVES)
                {
                    moves.RemoveAt(0);
                }
                moves.Add(new Move(move.moveBase));
            }
        }

        currentExp = _pokemonBase.GetExpForLevel(currentLevel);

        individualValues = new IndividualValues();
        individualValues.GenerateIVs();
        effortValues = new EffortValues();

        SetDataStats();
        currentHitPoints = maxHitPoints;

        gender = SetGender(_pokemonBase);
        statusChanges = new Queue<string>();

        originalTrainer = null;
        originalTrainerID = null;
        pokeballCapturedIn = null;

        SetAbility();
        Reset();
    }

    /// <summary>
    /// If the pokemon was loaded this will be called instead
    /// </summary>
    void LoadedInitialization()
    {

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

    public void Obtained(PlayerController player,PokeballItem pokeball)
    {
        originalTrainer = player.TrainerName;
        originalTrainerID = player.TrainerIDNumber;
        pokeballCapturedIn = pokeball;
    }

    #region Stats

    void SetDataStats()
    {
        baseStats = new Dictionary<StatAttribute, int>();

        maxHitPoints = Mathf.FloorToInt(((individualValues.maxHitPoints + 2 * pokemonBase.maxHitPoints + (effortValues.hitPoints / 4)) * currentLevel / 100) + 10 + currentLevel);
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

            statusChanges.Enqueue(StatChangesMessage(currentName, statModified,boost));

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

        if(status != null)
        {
            bool? negated = ability?.NegatesStatusEffectStatDropFromCondition?.Invoke(status.Id, currentStat);

            float? reduction = status?.StatEffectedByCondition?.Invoke(status.Id,currentStat);
            if (reduction.HasValue == true && negated == false)
            {
                statValue *= reduction.Value;
            }

            negated = ability?.BoostsAStatWhenAffectedWithAStatusCondition != null;
            if(negated == true)
            {
                reduction = ability?.BoostsAStatWhenAffectedWithAStatusCondition?.Invoke(status.Id, currentStat);
                if(reduction.Value <= 1)
                {
                    reduction = 1;
                }
                statValue *= reduction.Value;
            }
        }

        if(ability?.DoublesSpeedInAWeatherEffect != null && currentStat == StatAttribute.Speed)
        {
            statValue *= ability.DoublesSpeedInAWeatherEffect(BattleSystem.GetCurrentWeather);
        }

        if(ability?.DoublesAStat != null)
        {
            statValue *= ability.DoublesAStat(currentStat);
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
            hasFainted = false,
            criticalHit = 1,
            typeEffectiveness = 1
        };

        if(attackingPokemon.ability?.ChangeMovesToDifferentTypeAndIncreasesTheirPower?.Invoke(move) != null)
        {
            move = attackingPokemon.ability?.ChangeMovesToDifferentTypeAndIncreasesTheirPower.Invoke(move);
        }

        damageDetails.typeEffectiveness = DamageModifiers.TypeChartEffectiveness(pokemonBase, move.Type);

        if(damageDetails.typeEffectiveness == 0)//If it doesnt effect the pokemon then just end it right here
        {
            return damageDetails;
        }

        //Critical Hit Chance
        if (Random.value * 100 <= 6.25f)
        {
            damageDetails.criticalHit = 1.5f;

            //If sniper then make 2.

            if (ability?.PreventsCriticalHits == true)
            {
                damageDetails.criticalHit = 1f;
            }
        }

        float modifier = damageDetails.criticalHit * damageDetails.typeEffectiveness;
        modifier *= DamageModifiers.StandardRandomAttackPowerModifier();
        modifier *= DamageModifiers.SameTypeAttackBonus(move, attackingPokemon.pokemonBase);

        //Ability
        float? abilityBonus = attackingPokemon.ability?.BoostACertainTypeInAPinch?.Invoke(attackingPokemon, move.Type);
        abilityBonus = (abilityBonus.HasValue == true) ? abilityBonus : attackingPokemon.ability?.PowerUpCertainMoves?.Invoke(attackingPokemon,this, move);

        if (abilityBonus.HasValue == true)
        {
            modifier *= abilityBonus.Value;
        }

        float attackPower = (move.MoveType == MoveType.Physical) ? attackingPokemon.attack : attackingPokemon.specialAttack;
        float defendersDefense = (move.MoveType == MoveType.Physical) ? defense : specialDefense;

        //If Critical hit ignore the negative effects on attack and positive effects on the defense
        if(damageDetails.criticalHit > 1)
        {
            StatAttribute currentStat = (move.MoveType == MoveType.Physical) ? StatAttribute.Attack : StatAttribute.SpecialAttack;
            if (attackingPokemon.statBoosts[currentStat] < 0)
            {
                attackPower = attackingPokemon.baseStats[currentStat];
            }

            currentStat = (move.MoveType == MoveType.Physical) ? StatAttribute.Defense : StatAttribute.SpecialDefense;
            if (statBoosts[currentStat] > 0)
            {
                defendersDefense = baseStats[currentStat];
            }
        }

        int damage = Mathf.FloorToInt((((((2 * attackingPokemon.currentLevel) / 5) + 2) * move.MovePower * attackPower / defendersDefense / 50) + 2) * modifier);

        if(damage <=0)
        {
            damage = 1;
        }

        if(DamageModifiers.LeavesTargetWithOneHP(move) == true)
        {
            if(damage >= currentHitPoints)
            {
                damage = currentHitPoints - 1;
            }
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
        List<Move> movesWithPp = moves.Where(x => x.pP > 0).ToList();

        if(movesWithPp.Count == 0)
        {
            return null;
        }

        int r = Random.Range(0, movesWithPp.Count);
        return movesWithPp[r];
    }

    public string currentName
    {
        get
        {
            if(_currentName == null)
            {
                return pokemonBase.GetPokedexName();
            }
            return _currentName;
        }
        set
        {
            _currentName = value;
        }
    }

    public void SetStatus(ConditionID conditionID,bool secondaryEffect)
    {
        if(status != null)
        {
            if (secondaryEffect == true)
            {
                return;
            }

            string currentStatusChange = $"{currentName} {status.HasConditionMessage}";

            if(status.HasCondition(conditionID) == false)
            {
                currentStatusChange = $"It doesnt affect {currentName}";
            }
            statusChanges.Enqueue(currentStatusChange);
            return;
        }

        if(CheckStatusImmunities(conditionID) == true)
        {
            if (secondaryEffect == true)
            {
                return;
            }

            string currentStatusImmunity = $"It doesnt affect {currentName}";
            statusChanges.Enqueue(currentStatusImmunity);
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

    public bool CheckStatusImmunities(ConditionID conditionID)
    {
        if(conditionID == ConditionID.Poison || conditionID == ConditionID.ToxicPoison)
        {
            if(pokemonBase.IsType(ElementType.Poison) || pokemonBase.IsType(ElementType.Steel))
            {
                return true;
            }
        }

        if (conditionID == ConditionID.Burn)
        {
            if (pokemonBase.IsType(ElementType.Fire))
            {
                return true;
            }
        }

        if (conditionID == ConditionID.Paralyzed)
        {
            if (pokemonBase.IsType(ElementType.Electric))
            {
                return true;
            }
        }

        if (conditionID == ConditionID.Frozen)
        {
            if (pokemonBase.IsType(ElementType.Ice))
            {
                return true;
            }
        }

        return false;
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

    public bool HasCurrentVolatileStatus(ConditionID conditionID)
    {
        return volatileStatus.Exists(x => x.Id == conditionID);
    }

    /// <summary>
    /// Checks to see if the pokemon can attack or not
    /// </summary>
    /// <returns></returns>
    public ConditionID OnBeforeMove()
    {

        if(status?.OnBeforeMove != null)
        {
            if (status.OnBeforeMove(this) == false)
            {
                return status.Id;
            }
        }

        //This copy is here because if it is iterating through it and removes an element while searching it shall break the for each loop
        List<Condition> copyVolatileStatus = new List<Condition>(volatileStatus);

        foreach (Condition currentVolatileStatus in copyVolatileStatus)
        {
            if (currentVolatileStatus?.OnBeforeMove != null)
            {
                if (currentVolatileStatus.OnBeforeMove(this) == false)
                {
                    return currentVolatileStatus.Id;
                }
            }
        }

        return ConditionID.NA;
    }

    /// <summary>
    /// Applies any Effects on the pokemons turn end that it may have through Status Effects or Volatile Status
    /// </summary>
    public void OnEndTurn(Condition condition)
    {
        condition?.OnEndTurn?.Invoke(this);
    }

    public ConditionID GetCurrentStatus()
    {
        if(status == null)
        {
            return ConditionID.NA;
        }
        else if(status.Id > ConditionID.ToxicPoison)
        {
            return ConditionID.NA;
        }

        return status.Id;
    }

    public bool LevelUpCheck()
    {
        if(currentLevel >=100)
        {
            currentExp = _pokemonBase.GetExpForLevel(currentLevel);
            //This is incase of changes in Ev's
            UpdateStatsUponLevel();
            return false;
        }
        else if(currentExp >= _pokemonBase.GetExpForLevel(currentLevel + 1))
        {
            currentLevel++;
            UpdateStatsUponLevel();
            return true;
        }
        return false;
    }

    public List<LearnableMove> GetLeranableMoveAtCurrentLevel()
    {
        List<LearnableMove> copyOfLearnableMoves = _pokemonBase.LearnableMoves.FindAll(x => x.levelLearned == currentLevel);
        List<LearnableMove> learnableMoves = new List<LearnableMove>();

        foreach (LearnableMove move in copyOfLearnableMoves)
        {
            if(moves.Exists(x => x.moveBase == move.moveBase)|| learnableMoves.Exists(x => x.moveBase == move.moveBase))
            {
                continue;
            }
            learnableMoves.Add(move);
        }

        return learnableMoves;
    }

    public void LearnMove(LearnableMove moveLearned)
    {
        if(moves.Count > PokemonBase.MAX_NUMBER_OF_MOVES)
        {
            Debug.LogError($"{currentName} tried to add a move when it was beyond 4");
            return;
        }
        moves.Add(new Move(moveLearned.moveBase));
    }

    public void GainEffortValue(List<EarnableEV> effortValuesEarned)
    {
        foreach (EarnableEV eV in effortValuesEarned)
        {
            effortValues.AddEffortValue(eV);
        }
    }

    void SetAbility()
    {
        List<Ability> abilities = new List<Ability>();

        if(pokemonBase.FirstAbility != AbilityID.NA)
        {
            abilities.Add(AbilityDB.AbilityDex[pokemonBase.FirstAbility]);
        }

        if (pokemonBase.SecondAbility != AbilityID.NA)
        {
            abilities.Add(AbilityDB.AbilityDex[pokemonBase.SecondAbility]);
        }

        if (pokemonBase.HiddenAbility != AbilityID.NA)
        {
            abilities.Add(AbilityDB.AbilityDex[pokemonBase.HiddenAbility]);
        }

        if(abilities.Count == 0)
        {
            Debug.LogWarning($"{this.pokemonBase.GetPokedexName()} doesnt have any abilities set");
            return;
        }
        ability = abilities[Random.Range(0, abilities.Count)];
    }

    public StandardStats GetStandardStats()
    {
        return new StandardStats(maxHitPoints, baseStats[StatAttribute.Attack], baseStats[StatAttribute.Defense], 
            baseStats[StatAttribute.SpecialAttack], baseStats[StatAttribute.SpecialDefense], baseStats[StatAttribute.Speed]);
    }

    void UpdateStatsUponLevel()
    {
        int diffInHP = maxHitPoints;

        maxHitPoints = Mathf.FloorToInt(((individualValues.maxHitPoints + 2 * pokemonBase.maxHitPoints + (effortValues.hitPoints / 4)) * currentLevel / 100) + 10 + currentLevel);
        baseStats[StatAttribute.Attack] = Mathf.FloorToInt((((individualValues.attack + 2 * pokemonBase.attack + (effortValues.attack / 4)) * currentLevel / 100) + 5) * nature.NatureModifier(nature, StatAttribute.Attack));
        baseStats[StatAttribute.Defense] = Mathf.FloorToInt((((individualValues.defense + 2 * pokemonBase.defense + (effortValues.defense / 4)) * currentLevel / 100) + 5) * nature.NatureModifier(nature, StatAttribute.Defense));
        baseStats[StatAttribute.SpecialAttack] =Mathf.FloorToInt((((individualValues.specialAttack + 2 * pokemonBase.specialAttack + (effortValues.specialAttack / 4)) * currentLevel / 100) + 5) * nature.NatureModifier(nature, StatAttribute.SpecialAttack));
        baseStats[StatAttribute.SpecialDefense] =Mathf.FloorToInt((((individualValues.specialDefense + 2 * pokemonBase.specialDefense + (effortValues.specialDefense / 4)) * currentLevel / 100) + 5) * nature.NatureModifier(nature, StatAttribute.SpecialDefense));
        baseStats[StatAttribute.Speed] = Mathf.FloorToInt((((individualValues.speed + 2 * pokemonBase.speed + (effortValues.speed / 4)) * currentLevel / 100) + 5) * nature.NatureModifier(nature, StatAttribute.Speed));

        currentHitPoints += maxHitPoints - diffInHP;
    }

    public void FullyHeal()
    {
        currentHitPoints = maxHitPoints;
        status = null;
        for (int i = 0; i < moves.Count; i++)
        {
            moves[i].pP = moves[i].moveBase.PowerPoints;
        }
    }

    string StatChangesMessage(string currentPokemonName,StatAttribute statAttribute,int boost)
    {
        string statMessage;

        statMessage = $"{currentName}'s {GlobalTools.SplitCamelCase(statAttribute.ToString())} ";

        if (Mathf.Abs(boost) > 1)
        {
            statMessage += "sharply ";
        }

        statMessage += (boost > 0) ? "rose!": "fell!";

        return statMessage;
    }
}
