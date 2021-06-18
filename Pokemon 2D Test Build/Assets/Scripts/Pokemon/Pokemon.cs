using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class Pokemon {

    //variables
    [SerializeField] PokemonBase _pokemonBase;
    [SerializeField] int _level;
    [SerializeField] List<MoveBase> _presetMoves;
    [SerializeField] bool _isShiny;
    [SerializeField] Gender _gender;
    [SerializeField] NatureBase _nature;
    [SerializeField] IndividualValues _individualValues = new IndividualValues();
    [SerializeField] EffortValues _effortValues = new EffortValues();
    [SerializeField] string _currentName;
    [SerializeField] AbilityBase _ability;
    public System.Action OnStatusChanged;

    //properties
    public PokemonBase pokemonBase { get { return _pokemonBase; } private set { _pokemonBase = value; } }
    public int currentLevel { get { return _level; } private set { _level = value; } }
    public int currentHitPoints { get; set; }
    public int currentExp { get; set; }
    public NatureBase nature { get { return _nature; } set { _nature = value; } }
    public string originalTrainer { get; private set; }
    public string originalTrainerID { get; private set; }
    public PokeballItem pokeballCapturedIn { get; private set; }
    public AbilityBase ability { get { return _ability; } private set { _ability = value; } }
    public Gender gender { get { return _gender; } private set { _gender = value; } }
    public bool isShiny { get { return _isShiny; } private set { _isShiny = value; } }
    public IndividualValues individualValues { get { return _individualValues; } private set { _individualValues = value; } }
    public EffortValues effortValues { get { return _effortValues; } private set { _effortValues = value; } }

    public List<Move> moves { get; set; }
    public List<MoveBase> presetMoves { get { return _presetMoves; } }
    public Dictionary<StatAttribute, int> baseStats { get; private set; }
    public Dictionary<StatAttribute, int> statBoosts { get; private set; }
    public Queue<string> statusChanges { get; private set; } = new Queue<string>();

    public ConditionBase status { get; private set; }
    public int statusTime { get; set; }
    public List<ConditionBase> volatileStatus { get; private set; }
    public int volatileStatusTime { get; set; }

    #region Constructors

    public Pokemon(PokemonBase pokemonBase,int level,IndividualValues iV = null,EffortValues eV = null, Gender specifiedgender = Gender.NA, 
        bool? shiny = null,NatureBase specifiedNature = null,string nickname = null, List<MoveBase> presetMoveList = null, 
        AbilityBase abilityBase = null)
    {
        _pokemonBase = pokemonBase;
        currentLevel = level;
        currentExp = pokemonBase.GetExpForLevel(level);

        _individualValues.SetValues(iV);
        _effortValues.SetValues(eV);
        gender = SetGender(specifiedgender);

        nature = specifiedNature == null ? SetNature() : specifiedNature;
        isShiny = shiny.HasValue ? shiny.Value: (Random.value > 0.5f);
        currentName = nickname == null|| nickname == "" ? _pokemonBase.GetPokedexName() : nickname;

        SetDataStats();
        currentHitPoints = maxHitPoints;
        ability = SetAbility(abilityBase);

        SetMoves(presetMoveList);

        Reset();
    }

    public Pokemon(PokemonSaveData saveData)
    {
        _pokemonBase = saveData.currentBase;
        currentLevel = saveData.currentLevel;

        _individualValues.SetValues(saveData.currentIndividualValues);
        _effortValues.SetValues(saveData.currentEffortValues);
        gender = SetGender(saveData.currentGender);

        nature = saveData.currentNature;
        isShiny = saveData.isShiny;
        currentName = saveData.currentNickname == null || saveData.currentNickname == "" ? _pokemonBase.GetPokedexName() : saveData.currentNickname;

        currentExp = saveData.currentExp;

        SetDataStats();
        currentHitPoints = saveData.currentHitPoints;

        originalTrainer = saveData.currentOT;
        originalTrainerID = saveData.currentOTId;
        pokeballCapturedIn = saveData.currentPokeball;

        AbilityBase savedAbility = AbilityDB.GetAbilityBase(saveData.currentAbilityID);
        ability = SetAbility(savedAbility);

        LoadedMoves(saveData.currentMoves);
        SetStatus(saveData.currentCondition, false);

        Reset();
    }

    #endregion

    void SetMoves(List<MoveBase> presetMoves)
    {
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

        if(presetMoves == null) { return; }

        foreach (MoveBase move in presetMoves)
        {
            if (moves.Exists(x => x.moveBase == move) == true)
            {
                continue;
            }
            if (moves.Count >= PokemonBase.MAX_NUMBER_OF_MOVES)
            {
                moves.RemoveAt(0);
            }
            moves.Add(new Move(move));
        }
    }

    void LoadedMoves(List<Move> savedMoves)
    {
        moves = new List<Move>();
        foreach (Move move in savedMoves)
        {
            if (moves.Exists(x => x == move) == true)
            {
                continue;
            }
            if (moves.Count >= PokemonBase.MAX_NUMBER_OF_MOVES)
            {
                moves.RemoveAt(0);
            }
            moves.Add(move);
        }
    }


    /// <summary>
    /// resets all pokemon stats and volatile status, this is for when the pokmon is sent out or 
    /// when the battle is over to prevent bugs when pokemon is being viewed in the summary
    /// </summary>
    public void Reset()
    {
        ResetStatBoosts();
        volatileStatus = new List<ConditionBase>();
        statusChanges = new Queue<string>();
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

        maxHitPoints = Mathf.FloorToInt(((_individualValues.hitPoints + 2 * pokemonBase.maxHitPoints + (_effortValues.hitPoints / 4)) * currentLevel / 100) + 10 + currentLevel);
        baseStats.Add(StatAttribute.Attack, Mathf.FloorToInt((((_individualValues.attack + 2 * pokemonBase.attack + (_effortValues.attack / 4)) * currentLevel / 100) + 5) * nature.NatureModifier(nature, StatAttribute.Attack)));
        baseStats.Add(StatAttribute.Defense, Mathf.FloorToInt((((_individualValues.defense + 2 * pokemonBase.defense + (_effortValues.defense / 4)) * currentLevel / 100) + 5) * nature.NatureModifier(nature, StatAttribute.Defense)));
        baseStats.Add(StatAttribute.SpecialAttack, Mathf.FloorToInt((((_individualValues.specialAttack + 2 * pokemonBase.specialAttack + (_effortValues.specialAttack / 4)) * currentLevel / 100) + 5) * nature.NatureModifier(nature, StatAttribute.SpecialAttack)));
        baseStats.Add(StatAttribute.SpecialDefense, Mathf.FloorToInt((((_individualValues.specialDefense + 2 * pokemonBase.specialDefense + (_effortValues.specialDefense / 4)) * currentLevel / 100) + 5) * nature.NatureModifier(nature, StatAttribute.SpecialDefense)));
        baseStats.Add(StatAttribute.Speed, Mathf.FloorToInt((((_individualValues.speed + 2 * pokemonBase.speed + (_effortValues.speed / 4)) * currentLevel / 100) + 5) * nature.NatureModifier(nature, StatAttribute.Speed)));
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
            bool negated = ability.NegatesStatusEffectStatDropFromCondition(status.Id, currentStat);

            float? reduction = status?.StatEffectedByCondition(currentStat);
            if(reduction.HasValue == true && negated == false)
            {
                statValue *= reduction.Value;
            }

            reduction = ability.BoostsAStatWhenAffectedWithAStatusCondition(status.Id, currentStat);
            statValue *= reduction.Value;
        }

        statValue *= ability.AlterStat(BattleSystem.GetCurrentWeather,currentStat);

        statValue *= ability.DoublesAStat(currentStat);

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

    public NatureBase SetNature()
    {
        NatureBase[] natureBases;

        natureBases = Resources.LoadAll<NatureBase>("Natures");
        return natureBases[Random.Range(0, natureBases.Length)];
    }

    Gender SetGender(Gender serializedGender)
    {
        if (pokemonBase.HasGender == true)
        {
            if(serializedGender != Gender.NA)
            {
                return serializedGender;
            }

            float checker = Random.Range(1, 101);

            if (checker <= pokemonBase.MaleFemaleGenderRatio)//If true then male
            {
                serializedGender = Gender.Male;
            }
            else
            {
                serializedGender = Gender.Female;
            }
        }

        return serializedGender;
    }

    #endregion

    public DamageDetails TakeDamage(MoveBase move,Pokemon attackingPokemon)
    {
        DamageDetails damageDetails = new DamageDetails();

        damageDetails.typeEffectiveness = DamageModifiers.TypeChartEffectiveness(pokemonBase, move.Type);

        if(damageDetails.typeEffectiveness == 0)//If it doesnt effect the pokemon then just end it right here
        {
            if(attackingPokemon.ability.DamageDealingMovesCutThroughNaturalImmunity(this,move.Type) == true)
            {
                damageDetails.typeEffectiveness = 1;
            }
            else
            {
                return damageDetails;
            }
        }

        //Critical Hit Chance
        if (Random.value * 100 <= move.BaseCriticalHitRate)
        {
            damageDetails.criticalHit = attackingPokemon.ability.AltersCriticalHitDamage();

            if (ability.PreventsCriticalHits() == true)
            {
                damageDetails.criticalHit = 1f;
            }
        }

        float modifier = damageDetails.criticalHit * damageDetails.typeEffectiveness;
        modifier *= DamageModifiers.StandardRandomAttackPowerModifier();
        modifier *= DamageModifiers.SameTypeAttackBonus(move, attackingPokemon.pokemonBase);
        modifier *= DamageModifiers.WeatherConditionModifiers(BattleSystem.GetCurrentWeather, move);

        //Ability
        float abilityBonus = attackingPokemon.ability.BoostACertainTypeInAPinch(attackingPokemon, move.Type);
        abilityBonus *= attackingPokemon.ability.PowerUpCertainMoves(attackingPokemon,this, move,BattleSystem.GetCurrentWeather);
        abilityBonus *= ability.AlterDamageTaken(this, move,BattleSystem.GetCurrentWeather);
        abilityBonus *= ability.LowersDamageTakeSuperEffectiveMoves(damageDetails.typeEffectiveness);

        StatBoost defenderAbilityStatBoost = ability.AlterStatAfterTakingDamageFromCertainType(move.Type);
        StatBoost attackerAbilityStatBoost = ability.AlterStatAfterTakingDamage(move);

        if(defenderAbilityStatBoost != null)
        {
            damageDetails.abilityActivation = true;
            damageDetails.defendersStatBoostByAbility.Add(defenderAbilityStatBoost);
        }

        if (attackerAbilityStatBoost != null)
        {
            damageDetails.abilityActivation = true;
            damageDetails.attackersStatBoostByDefendersAbility.Add(attackerAbilityStatBoost);
        }


        if (abilityBonus == 0)
        {
            damageDetails.criticalHit = 1f;
            damageDetails.typeEffectiveness = 1;
            damageDetails.abilityActivation = true;
            damageDetails.damageNullified = true;
            return damageDetails;
        }

        modifier *= abilityBonus;

        float attackPower = (move.MoveType == MoveType.Physical) ? attackingPokemon.attack : attackingPokemon.specialAttack;
        float defendersDefense = (move.MoveType == MoveType.Physical) ? defense : specialDefense;

        defendersDefense *= DamageModifiers.SandStormSpecialDefenseBonus(BattleSystem.GetCurrentWeather, this, move);

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

        if(move.LeavesTargetWith1HP == true)
        {
            if(damage >= currentHitPoints)
            {
                damage = currentHitPoints - 1;
            }
        }

        if(ability.PreventsOneHitKO(this,damage) == true)
        {
            damage = maxHitPoints - 1;
            damageDetails.abilityActivation = true;
        }

        UpdateHPDamage(damage);

        return damageDetails;
    }

    public void UpdateHPDamage(int damage)
    {
        currentHitPoints = Mathf.Clamp(currentHitPoints - damage, 0, maxHitPoints);
    }

    public void UpdateHPRestored(int hpRestored)
    {
        currentHitPoints = Mathf.Clamp(currentHitPoints + hpRestored, 0, maxHitPoints);
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
        if(conditionID == ConditionID.NA) { return; }

        if(status != null)
        {
            if (secondaryEffect == true)
            {
                return;
            }

            string currentStatusChange = $"{status.HasConditionMessage(this)}";

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
            
            statusChanges.Enqueue($"It doesnt affect {currentName}");
            return;
        }

        status = ConditionsDB.GetConditionBase(conditionID);
        statusChanges.Enqueue(status.StartMessage(this));

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

    public void SetVolatileStatus(ConditionID conditionID,MoveBase currentMove,BattleUnit sourceBattleUnit)
    {
        ConditionBase currentCondition = ConditionsDB.GetConditionBase(conditionID);

        if (volatileStatus.Exists(x => x.Id == conditionID) == true)
        {
            string currentStatusChange = null;

            currentStatusChange = $"{currentCondition.HasConditionMessage(this)}";

            if (currentCondition.HasCondition(conditionID) == false)
            {
                currentStatusChange = $"It doesnt affect {currentName}";
            }

            if(currentStatusChange != null || currentStatusChange != "")
            {
                statusChanges.Enqueue(currentStatusChange);
            }
            return;
        }

        ConditionBase newVolatileStatus = ConditionsDB.GetConditionBase(conditionID);
        volatileStatus.Add(newVolatileStatus);

        if(newVolatileStatus.Id == ConditionID.Bound)
        {
            ((Bound)newVolatileStatus).SetBoundMove = currentMove;
        }

        statusChanges.Enqueue(newVolatileStatus.StartMessage(this,sourceBattleUnit.pokemon));
    }

    public void CureAllVolatileStatus()
    {
        volatileStatus = new List<ConditionBase>();
    }

    public void CureVolatileStatus(ConditionID conditionID)
    {
        volatileStatus.Remove(volatileStatus.Find(x => x.Id == conditionID));
    }

    public bool HasCurrentVolatileStatus(ConditionID conditionID)
    {
        return volatileStatus.Exists(x => x.Id == conditionID);
    }

    /// <summary>
    /// Checks to see if the pokemon can attack or not
    /// </summary>
    /// <returns></returns>
    public ConditionID OnBeforeMove(Pokemon targetPokemon)
    {

        if (status?.OnBeforeMove(this, targetPokemon) == false)
        {
            return status.Id;
        }

        //This copy is here because if it is iterating through it and removes an element while searching it shall break the for each loop
        List<ConditionBase> copyVolatileStatus = new List<ConditionBase>(volatileStatus);

        foreach (ConditionBase currentVolatileStatus in copyVolatileStatus)
        {
            if (currentVolatileStatus.OnBeforeMove(this, targetPokemon) == false)
            {
                return currentVolatileStatus.Id;
            }
        }

        return ConditionID.NA;
    }

    /// <summary>
    /// Applies any Effects on the pokemons turn end that it may have through Status Effects or Volatile Status
    /// </summary>
    public bool OnEndTurn(ConditionBase condition)
    {
        return condition.OnEndTurn(this);
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

    public List<LearnableMove> GetLearnableMoveAtCurrentLevel()
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
            _effortValues.AddEffortValue(eV);
        }
    }

    AbilityBase SetAbility(AbilityBase presetAbility)
    {
        List<AbilityBase> abilities = new List<AbilityBase>();

        if(pokemonBase.FirstAbility != AbilityID.NA)
        {
            abilities.Add(AbilityDB.GetAbilityBase(pokemonBase.FirstAbility));
        }

        if (pokemonBase.SecondAbility != AbilityID.NA)
        {
            abilities.Add(AbilityDB.GetAbilityBase(pokemonBase.SecondAbility));
        }

        if (pokemonBase.HiddenAbility != AbilityID.NA)
        {
            abilities.Add(AbilityDB.GetAbilityBase(pokemonBase.HiddenAbility));
        }

        if(abilities.Count == 0)
        {
            Debug.LogWarning($"{this.pokemonBase.GetPokedexName()} doesnt have any abilities set");
            return null;
        }

        if(presetAbility != null)
        {
            if (abilities.Exists(x => x.Id == presetAbility.Id))
            {
                return presetAbility;
            }
        }

        return abilities[Random.Range(0, abilities.Count)];
    }

    public StandardStats GetStandardStats()
    {
        return new StandardStats(maxHitPoints, baseStats[StatAttribute.Attack], baseStats[StatAttribute.Defense], 
            baseStats[StatAttribute.SpecialAttack], baseStats[StatAttribute.SpecialDefense], baseStats[StatAttribute.Speed]);
    }

    void UpdateStatsUponLevel()
    {
        int diffInHP = maxHitPoints;

        maxHitPoints = Mathf.FloorToInt(((_individualValues.hitPoints + 2 * pokemonBase.maxHitPoints + (_effortValues.hitPoints / 4)) * currentLevel / 100) + 10 + currentLevel);
        baseStats[StatAttribute.Attack] = Mathf.FloorToInt((((_individualValues.attack + 2 * pokemonBase.attack + (_effortValues.attack / 4)) * currentLevel / 100) + 5) * nature.NatureModifier(nature, StatAttribute.Attack));
        baseStats[StatAttribute.Defense] = Mathf.FloorToInt((((_individualValues.defense + 2 * pokemonBase.defense + (_effortValues.defense / 4)) * currentLevel / 100) + 5) * nature.NatureModifier(nature, StatAttribute.Defense));
        baseStats[StatAttribute.SpecialAttack] =Mathf.FloorToInt((((_individualValues.specialAttack + 2 * pokemonBase.specialAttack + (_effortValues.specialAttack / 4)) * currentLevel / 100) + 5) * nature.NatureModifier(nature, StatAttribute.SpecialAttack));
        baseStats[StatAttribute.SpecialDefense] =Mathf.FloorToInt((((_individualValues.specialDefense + 2 * pokemonBase.specialDefense + (_effortValues.specialDefense / 4)) * currentLevel / 100) + 5) * nature.NatureModifier(nature, StatAttribute.SpecialDefense));
        baseStats[StatAttribute.Speed] = Mathf.FloorToInt((((_individualValues.speed + 2 * pokemonBase.speed + (_effortValues.speed / 4)) * currentLevel / 100) + 5) * nature.NatureModifier(nature, StatAttribute.Speed));

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