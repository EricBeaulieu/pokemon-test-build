using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class Pokemon {

    //variables
    [SerializeField] PokemonBase _pokemonBase;
    [Range(1,100)]
    [SerializeField] int _level = 1;
    [SerializeField] List<MoveBase> _presetMoves;
    [SerializeField] bool _isShiny;
    [SerializeField] Gender _gender;
    [SerializeField] NatureBase _nature;
    [SerializeField] IndividualValues _individualValues = new IndividualValues();
    [SerializeField] EffortValues _effortValues = new EffortValues();
    [SerializeField] string _currentName;
    [SerializeField] AbilityID abilityID = AbilityID.NA;
    [SerializeField] ItemBase currentHeldItem;
    int hitPoints;
    AbilityBase _ability;
    public System.Action OnStatusChanged;

    //properties
    public PokemonBase pokemonBase { get { return _pokemonBase; } private set { _pokemonBase = value; } }
    public int currentLevel { get { return _level; } private set { _level = value; } }
    public int currentHitPoints { get { return hitPoints; } set { if (value <= 0) status = null; hitPoints = value; } }
    public int currentExp { get; set; }
    public NatureBase nature { get { return _nature; } set { _nature = value; } }
    public string originalTrainer { get; private set; }
    public string originalTrainerID { get; private set; }
    public PokeballItem pokeballCapturedIn { get; private set; }
    public AbilityBase ability { get { return _ability; } private set { _ability = value; } }
    public AbilityID startingAbilityID { get { return abilityID; } private set { abilityID = value; } }
    public Gender gender { get { return _gender; } private set { _gender = value; } }
    public bool isShiny { get { return _isShiny; } private set { _isShiny = value; } }
    public IndividualValues individualValues { get { return _individualValues; } private set { _individualValues = value; } }
    public EffortValues effortValues { get { return _effortValues; } private set { _effortValues = value; } }
    public ItemBase GetCurrentItem { get { return currentHeldItem; } }
    public bool evolvePokemonAfterBattle { get; private set; } = false;
    public ElementType pokemonType1 { get; private set; }
    public ElementType pokemonType2 { get; private set; }
    public bool IsType(ElementType elementType)
    {
        if (elementType == pokemonType1 || elementType == pokemonType2)
        {
            return true;
        }
        return false;
    }

    public List<Move> moves { get; set; }
    public List<MoveBase> presetMoves { get { return _presetMoves; } }
    public Dictionary<StatAttribute, int> baseStats { get; private set; }
    public Dictionary<StatAttribute, int> statBoosts { get; private set; }
    public Queue<string> statusChanges { get; private set; } = new Queue<string>();

    public ConditionBase status { get; private set; }
    public List<ConditionBase> volatileStatus { get; private set; } = new List<ConditionBase>();

    #region Constructors

    public Pokemon(PokemonBase pokemonBase,int level,IndividualValues iV = null,EffortValues eV = null, Gender specifiedgender = Gender.NA, 
        bool? shiny = null,NatureBase specifiedNature = null,string nickname = null, List<MoveBase> presetMoveList = null, 
        AbilityID presetAbilityID = AbilityID.NA,ItemBase item = null)
    {
        _pokemonBase = pokemonBase;

        if(level <= 0 || level > 100)
        {
            level = 5;
        }
        currentLevel = level;
        currentExp = pokemonBase.GetExpForLevel(level);

        _individualValues.SetValues(iV);
        _effortValues.SetValues(eV);
        gender = SetGender(specifiedgender);

        nature = specifiedNature == null ? SetNature() : specifiedNature;
        isShiny = shiny.HasValue ? shiny.Value: (Random.value > 0.5f);
        currentName = nickname;

        SetDataStats();
        currentHitPoints = maxHitPoints;

        if(presetAbilityID != AbilityID.NA)
        {
            ability = SetAbility(AbilityDB.GetAbilityBase(presetAbilityID));
        }
        else
        {
            ability = SetAbility();
        }
        
        GivePokemonItemToHold(item);

        SetMoves(presetMoveList);
    }

    public Pokemon(PokemonSaveData saveData)
    {
        _pokemonBase = Resources.Load<PokemonBase>(saveData.currentBase);
        currentLevel = saveData.currentLevel;

        _individualValues.SetValues(saveData.currentIndividualValues);
        _effortValues.SetValues(saveData.currentEffortValues);
        gender = SetGender(saveData.currentGender);

        nature = Resources.Load<NatureBase>(saveData.currentNature);
        isShiny = saveData.isShiny; 
        currentName = string.IsNullOrEmpty(saveData.currentNickname) ? _pokemonBase.GetPokedexName() : saveData.currentNickname;

        currentExp = saveData.currentExp;

        SetDataStats();
        currentHitPoints = saveData.currentHitPoints;

        originalTrainer = saveData.currentOT;
        originalTrainerID = saveData.currentOTId;
        pokeballCapturedIn = Resources.Load<PokeballItem>(saveData.currentPokeball);

        AbilityBase savedAbility = AbilityDB.GetAbilityBase(saveData.currentAbilityID);
        ability = SetAbility(savedAbility);

        moves = saveData.currentMoves.Select(x => new Move(x)).ToList();
        SetStatus(saveData.currentCondition, false);

        if(string.IsNullOrEmpty(saveData.currentItem) == false)
        {
            GivePokemonItemToHold(Resources.Load<ItemBase>(saveData.currentItem));
        }
    }

    public Pokemon(WildPokemon wildPokemon)
    {
        _pokemonBase = wildPokemon.PokemonBase;
        currentLevel = wildPokemon.Level;
        currentExp = pokemonBase.GetExpForLevel(wildPokemon.Level);

        _individualValues.SetValues();
        _effortValues.SetValues();
        gender = SetGender();

        nature = SetNature();
        isShiny = (Random.value > 0.5f);

        SetDataStats();
        currentHitPoints = maxHitPoints;

        ability = SetAbility();

        GivePokemonItemToHold(wildPokemon.HoldItemUponBeingFound());

        SetMoves();
    }

    #endregion

    void SetMoves(List<MoveBase> presetMoves = null)
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

    /// <summary>
    /// resets all pokemon stats and volatile status, this is for when the pokmon is sent out or 
    /// when the battle is over to prevent bugs when pokemon is being viewed in the summary
    /// </summary>
    public void Reset()
    {
        ResetStatBoosts();
        volatileStatus.Clear();
        statusChanges.Clear();
        evolvePokemonAfterBattle = false;
        SetPokemonTypeToStandard();
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
        baseStats.Add(StatAttribute.CriticalHitRatio, 0);

        statBoosts = new Dictionary<StatAttribute, int>();
        statBoosts.Add(StatAttribute.Attack, 0);
        statBoosts.Add(StatAttribute.Defense, 0);
        statBoosts.Add(StatAttribute.SpecialAttack, 0);
        statBoosts.Add(StatAttribute.SpecialDefense, 0);
        statBoosts.Add(StatAttribute.Speed, 0);
        statBoosts.Add(StatAttribute.Accuracy, 0);
        statBoosts.Add(StatAttribute.Evasion, 0);
        statBoosts.Add(StatAttribute.CriticalHitRatio, 0);

        SetPokemonTypeToStandard();
    }
    

    public void ResetStatBoosts()//clear smog/haze calls this
    {
        statBoosts[StatAttribute.Attack] = 0;
        statBoosts[StatAttribute.Defense] = 0;
        statBoosts[StatAttribute.SpecialAttack] = 0;
        statBoosts[StatAttribute.SpecialDefense] = 0;
        statBoosts[StatAttribute.Speed] = 0;
        statBoosts[StatAttribute.Accuracy] = 0;
        statBoosts[StatAttribute.Evasion] = 0;
        statBoosts[StatAttribute.CriticalHitRatio] = 0;
    }

    public void RestoreAllLoweredStatBoosts()
    {
        if(statBoosts[StatAttribute.Attack] < 0)
        {
            statBoosts[StatAttribute.Attack] = 0;
        }

        if (statBoosts[StatAttribute.Defense] < 0)
        {
            statBoosts[StatAttribute.Defense] = 0;
        }

        if (statBoosts[StatAttribute.SpecialAttack] < 0)
        {
            statBoosts[StatAttribute.SpecialAttack] = 0;
        }

        if (statBoosts[StatAttribute.SpecialDefense] < 0)
        {
            statBoosts[StatAttribute.SpecialDefense] = 0;
        }

        if (statBoosts[StatAttribute.Speed] < 0)
        {
            statBoosts[StatAttribute.Speed] = 0;
        }

        if (statBoosts[StatAttribute.Evasion] < 0)
        {
            statBoosts[StatAttribute.Evasion] = 0;
        }

        if (statBoosts[StatAttribute.Accuracy] < 0)
        {
            statBoosts[StatAttribute.Accuracy] = 0;
        }
    }

    public bool ApplyStatModifier(List<StatBoost> currentBoostModifiers)
    {
        bool showAnimation = false;
        foreach (StatBoost modifier in currentBoostModifiers)
        {
            if(modifier.Stat == StatAttribute.NA)
            {
                continue;
            }

            StatAttribute statModified = modifier.Stat;
            int boost = modifier.Boost;

            if(ability.StatChangesHaveOppositeEffect() == true)
            {
                boost = -boost;
            }

            if(modifier.Boost > 0)
            {
                if(statBoosts[statModified] == 6)
                {
                    statusChanges.Enqueue($"{currentName} {modifier.Stat.ToString()} can't go any higher");
                    continue;
                }
            }
            else
            {
                if (statBoosts[statModified] == -6)
                {
                    statusChanges.Enqueue($"{currentName} {modifier.Stat.ToString()} can't go any lower");
                    continue;
                }
            }
            statBoosts[statModified] = Mathf.Clamp(statBoosts[statModified] + boost, -6, 6);

            statusChanges.Enqueue(StatChangesMessage(currentName, statModified,boost));

            Debug.Log($"{currentName} {statModified} has been changed to {statBoosts[statModified]}");
            showAnimation = true;
        }
        return showAnimation;
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
        else if(currentStat == StatAttribute.CriticalHitRatio)
        {
            boost += Mathf.FloorToInt(GetHoldItemEffects.AlterStat(this, currentStat));
            return boost;
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
        statValue *= GetHoldItemEffects.AlterStat(this, currentStat);

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

    public int criticalHitRatio
    {
        get { return (int)GetStatAfterModification(StatAttribute.CriticalHitRatio); }
    }

    #endregion

    #region Nature/Gender

    public NatureBase SetNature()
    {
        NatureBase[] natureBases;

        natureBases = Resources.LoadAll<NatureBase>("Natures");
        return natureBases[Random.Range(0, natureBases.Length)];
    }

    Gender SetGender(Gender serializedGender = Gender.NA)
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

    public void TakeDamage(DamageDetails damageDetails,MoveBase move,BattleUnit attackingUnit,BattleUnit selfRef, int presetDamage = 0)
    {
        damageDetails.Clear();

        if(presetDamage > 0)
        {
            UpdateHPDamage(presetDamage);
            return;
        }

        damageDetails.typeEffectiveness = DamageModifiers.TypeChartEffectiveness(selfRef, move.Type);

        if(damageDetails.typeEffectiveness == 0)//If it doesnt effect the pokemon then just end it right here
        {
            if(attackingUnit.pokemon.ability.DamageDealingMovesCutThroughNaturalImmunity(this,move.Type) == true)
            {
                damageDetails.typeEffectiveness = 1;
            }
            else
            {
                return;
            }
        }

        //Critical Hit Chance
        if (Random.value * 100 <= DamageModifiers.CriticalHitPercentage(move,attackingUnit.pokemon))
        {
            damageDetails.criticalHit = attackingUnit.pokemon.ability.AltersCriticalHitDamage();

            if (ability.PreventsCriticalHits() == true)
            {
                damageDetails.criticalHit = 1f;
            }
        }

        float modifier = damageDetails.criticalHit * damageDetails.typeEffectiveness;
        modifier *= DamageModifiers.StandardRandomAttackPowerModifier();
        modifier *= DamageModifiers.SameTypeAttackBonus(move, attackingUnit.pokemon.pokemonBase);
        modifier *= DamageModifiers.WeatherConditionModifiers(BattleSystem.GetCurrentWeather, move);

        //Ability
        float abilityBonus = attackingUnit.pokemon.ability.BoostACertainTypeInAPinch(attackingUnit.pokemon, move.Type);
        abilityBonus *= attackingUnit.pokemon.ability.PowerUpCertainMoves(attackingUnit.pokemon, selfRef, move,BattleSystem.GetCurrentWeather);
        abilityBonus *= ability.AlterDamageTaken(selfRef, move,BattleSystem.GetCurrentWeather);
        abilityBonus *= ability.LowersDamageTakeSuperEffectiveMoves(damageDetails.typeEffectiveness);

        StatBoost defenderAbilityStatBoost = ability.AlterStatAfterTakingDamageFromCertainType(move.Type);
        StatBoost attackerAbilityStatBoost = ability.AlterStatAfterTakingDamage(move);

        if(defenderAbilityStatBoost != null)
        {
            damageDetails.defendersAbilityActivation = true;
            damageDetails.defendersStatBoostByAbility.Add(defenderAbilityStatBoost);
        }

        if (attackerAbilityStatBoost != null)
        {
            damageDetails.defendersAbilityActivation = true;
            damageDetails.attackersStatBoostByDefendersAbility.Add(attackerAbilityStatBoost);
        }
        
        if (abilityBonus == 0)
        {
            damageDetails.criticalHit = 1f;
            damageDetails.typeEffectiveness = 1;
            damageDetails.defendersAbilityActivation = true;
            damageDetails.damageNullified = true;
            return;
        }

        //Item effects
        float itemBonus = GetHoldItemEffects.AlterDamageTaken(selfRef,move, (damageDetails.criticalHit > 1));
        itemBonus *= attackingUnit.pokemon.GetHoldItemEffects.PowersUpSuperEffectiveAttacks((damageDetails.criticalHit > 1));

        damageDetails.alterStatAfterTakingDamage = GetHoldItemEffects.AlterStatAfterTakingDamageFromCertainType(selfRef,move,damageDetails.typeEffectiveness > 1);

        if (itemBonus == 0)
        {
            damageDetails.criticalHit = 1f;
            damageDetails.typeEffectiveness = 0;
            return;
        }

        modifier *= abilityBonus;
        modifier *= itemBonus;
        
        float attackPower = (move.MoveType == MoveType.Physical) ? attackingUnit.pokemon.attack : attackingUnit.pokemon.specialAttack;
        float defendersDefense;

        if (move.originalMove == SpecializedMoves.foulPlay)
        {
            attackPower = (move.MoveType == MoveType.Physical) ? attack : specialAttack;
        }

        if (move.BypassesTargetsStatBoosts == false)
        {
            defendersDefense = (move.MoveType == MoveType.Physical) ? defense : specialDefense;
        }
        else
        {
            defendersDefense = (move.MoveType == MoveType.Physical) ? baseStats[StatAttribute.Defense] : baseStats[StatAttribute.SpecialDefense];
        }

        defendersDefense *= DamageModifiers.SandStormSpecialDefenseBonus(BattleSystem.GetCurrentWeather, this, move);

        //If Critical hit ignore the negative effects on attack and positive effects on the defense
        if(damageDetails.criticalHit > 1)
        {
            StatAttribute currentStat = (move.MoveType == MoveType.Physical) ? StatAttribute.Attack : StatAttribute.SpecialAttack;
            if (attackingUnit.pokemon.statBoosts[currentStat] < 0)
            {
                attackPower = attackingUnit.pokemon.baseStats[currentStat];
            }

            if(move.BypassesTargetsStatBoosts == false)
            {
                currentStat = (move.MoveType == MoveType.Physical) ? StatAttribute.Defense : StatAttribute.SpecialDefense;
                if (statBoosts[currentStat] > 0)
                {
                    defendersDefense = baseStats[currentStat];
                }
            }
        }
        else
        {
            StatAttribute currentStat = (move.MoveType == MoveType.Physical) ? StatAttribute.Defense : StatAttribute.SpecialDefense;

            if(attackingUnit.pokemon.ability.CutsThroughProtections() == false)
            {
                //Shield bonuses
                foreach (ShieldBase shield in selfRef.shields)
                {
                    if (shield.ProtectedStat(currentStat) > 1)
                    {
                        attackPower /= shield.ProtectedStat(currentStat);
                        break;
                    }
                }
            }
        }

        int damage = Mathf.FloorToInt((((((2 * attackingUnit.pokemon.currentLevel) / 5) + 2) * move.MovePower * attackPower / defendersDefense / 50) + 2) * modifier);

        if(damage <=0)
        {
            damage = 1;
        }

        if(move.LeavesTargetWith1HP == true || GetHoldItemEffects.EndureOHKOAttack(selfRef) == true)
        {
            if (damage >= currentHitPoints)
            {
                damage = currentHitPoints - 1;
            }
        }

        if(ability.PreventsOneHitKO(this,damage) == true)
        {
            damage = maxHitPoints - 1;
            damageDetails.defendersAbilityActivation = true;
        }

        UpdateHPDamage(damage);
        selfRef.damagedThisTurn = true;
        selfRef.damagedReceived += damage;

        if (GetHoldItemEffects.TransferToPokemon(move) && attackingUnit.pokemon.GetCurrentItem == null)
        {
            attackingUnit.pokemon.GivePokemonItemToHold(GetCurrentItem);
            ItemUsed();
        }

        if(currentHitPoints <= 0)
        {
            attackerAbilityStatBoost = attackingUnit.pokemon.ability.BoostStatUponKO(attackingUnit.pokemon);
            if (attackerAbilityStatBoost != null)
            {
                damageDetails.attackersAbilityActivation = true;
                damageDetails.attackersStatBoostByDefendersAbility.Add(attackerAbilityStatBoost);
            }
        }

        if(currentHitPoints > 0)
        {
            defenderAbilityStatBoost = ability.BoostStatUponCertainConditions(this);

            if (defenderAbilityStatBoost != null)
            {
                damageDetails.defendersAbilityActivation = true;
                damageDetails.defendersStatBoostByAbility.Add(defenderAbilityStatBoost);
            }

            ElementType elementType = ability.ChangePokemonToCurrentType(this,move);

            if(elementType != ElementType.NA)
            {
                damageDetails.defendersAbilityActivation = true;
                AlterPokemonTyping(elementType);
                statusChanges.Enqueue(ability.OnAbilitityActivation(this));
            }
        }
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
            if(string.IsNullOrEmpty(_currentName))
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

    public void SetStatusByItem(ConditionID conditionID, string itemMessage)
    {
        if (conditionID == ConditionID.NA || status != null|| CheckStatusImmunities(conditionID) == true) { return; }

        status = ConditionsDB.GetConditionBase(conditionID);
        statusChanges.Enqueue(itemMessage);

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
        else if(newVolatileStatus.Id == ConditionID.ChargingTurn)
        {
            ((ChargingTurn)newVolatileStatus).chargingMove = currentMove;
            ((ChargingTurn)newVolatileStatus).SetInvulnerableType(SpecializedMoves.ReturnSemiInvulnerableType(currentMove));
            Debug.Log($"{sourceBattleUnit.pokemon.currentName} is currently {SpecializedMoves.ReturnSemiInvulnerableType(currentMove)}");
        }

        statusChanges.Enqueue(newVolatileStatus.StartMessage(this,sourceBattleUnit.pokemon));
    }

    public void SetVolatileStatus(ConditionID conditionID)
    {
        if(HasCurrentVolatileStatus(conditionID) == true)
        {
            return;
        }

        volatileStatus.Add(ConditionsDB.GetConditionBase(conditionID));
    }

    public void CureAllVolatileStatus()
    {
        volatileStatus.Clear();
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
    /// Applies any Effects on the pokemons turn end that it may have through Status Effects/ Volatile Status/ Volatile Battle Status
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
            UpdateStats();
            return false;
        }
        else if(currentExp >= _pokemonBase.GetExpForLevel(currentLevel + 1))
        {
            currentLevel++;
            UpdateStats();

            if (_pokemonBase.EvolveLevelBased != null)
            {
                foreach (EvolveLevelBased evolution in _pokemonBase.EvolveLevelBased)
                {
                    if (evolution.CanEvolve(this, GetCurrentItem) == true && currentHitPoints > 0)
                    {
                        evolvePokemonAfterBattle = true;
                    }
                }
            }

            return true;
        }
        return false;
    }

    public List<LearnableMove> GetLearnableMoveAtCurrentLevel()
    {
        List<LearnableMove> learnableMoves = new List<LearnableMove>();

        foreach (LearnableMove move in _pokemonBase.LearnableMoves.FindAll(x => x.levelLearned == currentLevel))
        {
            if(moves.Exists(x => x.moveBase == move.moveBase)|| learnableMoves.Exists(x => x.moveBase == move.moveBase))
            {
                continue;
            }
            learnableMoves.Add(move);
        }

        return learnableMoves;
    }

    public List<LearnableMove> GetLearnableMoveUponEvolution()
    {
        List<LearnableMove> learnableMoves = new List<LearnableMove>();

        foreach (LearnableMove move in _pokemonBase.LearnableMoves.FindAll(x => x.levelLearned < 0))
        {
            if (moves.Exists(x => x.moveBase == move.moveBase) || learnableMoves.Exists(x => x.moveBase == move.moveBase))
            {
                continue;
            }
            learnableMoves.Add(move);
        }

        return learnableMoves;
    }

    public void LearnMove(MoveBase moveLearned)
    {
        if(moves.Count > PokemonBase.MAX_NUMBER_OF_MOVES)
        {
            Debug.LogError($"{currentName} tried to add a move when it was beyond 4");
            return;
        }
        moves.Add(new Move(moveLearned));
    }

    public void GainEffortValue(List<EarnableEV> effortValuesEarned)
    {
        effortValuesEarned = GetHoldItemEffects.AdditionalEffortValues(effortValuesEarned);
        foreach (EarnableEV eV in effortValuesEarned)
        {
            _effortValues.AddEffortValue(eV);
        }
    }

    public bool RemoveEffortValue(EarnableEV effortValuesRemoved)
    {
        return effortValues.RemoveEffortValue(effortValuesRemoved);
    }

    AbilityBase SetAbility(AbilityBase presetAbility = null)
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

    void UpdateStats()
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

    public void GivePokemonItemToHold(ItemBase item)
    {
        currentHeldItem = item;
    }

    public HoldItemBase GetHoldItemEffects
    {
        get
        {
            if (currentHeldItem == null)
            {
                return HoldItemDB.GetHoldItem();
            }
            else
            {
                HoldItemBase holdItem = currentHeldItem.HoldItemAffects();
                if (ability.CantUseAnyHeldItems(holdItem) == true)
                {
                    return HoldItemDB.GetHoldItem();
                }
                return holdItem;
            }
        }
    }

    public void ItemUsed()
    {
        currentHeldItem = null;
    }

    public void NewEvolution(PokemonBase newEvolution)
    {
        if(currentName == pokemonBase.GetPokedexName())
        {
            currentName = newEvolution.GetPokedexName();
        }
        int currentAbilityIndex = PreviousAbilityIndex();
        pokemonBase = newEvolution;
        UpdateStats();
        ability = UpdateAbility(currentAbilityIndex);
    }

    int PreviousAbilityIndex()
    {
        if(ability.Id == pokemonBase.FirstAbility)
        {
            return 1;
        }
        else if (ability.Id == pokemonBase.SecondAbility)
        {
            return 2;
        }
        else if (ability.Id == pokemonBase.HiddenAbility)
        {
            return 3;
        }
        return 0;
    }

    AbilityBase UpdateAbility(int previousIndex)
    {
        switch (previousIndex)
        {
            case 1:
                if(pokemonBase.FirstAbility != AbilityID.NA)
                {
                    return AbilityDB.GetAbilityBase(pokemonBase.FirstAbility);
                }
                break;
            case 2:
                if (pokemonBase.SecondAbility != AbilityID.NA)
                {
                    return AbilityDB.GetAbilityBase(pokemonBase.SecondAbility);
                }
                break;
            case 3:
                if (pokemonBase.HiddenAbility != AbilityID.NA)
                {
                    return AbilityDB.GetAbilityBase(pokemonBase.HiddenAbility);
                }
                break;
            default:
                break;
        }
        return SetAbility();
    }

    public PokemonSaveData GetSaveData()
    {
        PokemonSaveData pokemonSaveData = new PokemonSaveData();

        pokemonSaveData.currentBase = SavingSystem.GetAssetPath(_pokemonBase);
        Debug.Log(pokemonSaveData.currentBase);
        pokemonSaveData.currentLevel = currentLevel;
        pokemonSaveData.currentExp = currentExp;
        pokemonSaveData.currentHitPoints = currentHitPoints;
        pokemonSaveData.currentMoves = moves.Select(x => x.GetSaveData()).ToList();
        pokemonSaveData.isShiny = isShiny;
        pokemonSaveData.currentGender = gender;
        pokemonSaveData.currentNature = SavingSystem.GetAssetPath(nature);
        pokemonSaveData.currentIndividualValues = individualValues;
        pokemonSaveData.currentEffortValues = effortValues;
        pokemonSaveData.currentNickname = currentName;
        pokemonSaveData.currentAbilityID = ability.Id;
        if(status == null)
        {
            pokemonSaveData.currentCondition = ConditionID.NA;
        }
        else
        {
            pokemonSaveData.currentCondition = status.Id;
        }
        pokemonSaveData.currentOT = originalTrainer;
        pokemonSaveData.currentOTId = originalTrainerID;
        pokemonSaveData.currentPokeball = SavingSystem.GetAssetPath(pokeballCapturedIn);
        if(currentHeldItem != null)
        {
            pokemonSaveData.currentItem = SavingSystem.GetAssetPath(currentHeldItem);
        }
        
        return pokemonSaveData;
    }

    public void AlterPokemonTyping(ElementType newType)
    {
        pokemonType1 = newType;
        pokemonType2 = ElementType.NA;
    }

    void SetPokemonTypeToStandard()
    {
        pokemonType1 = _pokemonBase.pokemonType1;
        pokemonType2 = _pokemonBase.pokemonType2;
    }
}