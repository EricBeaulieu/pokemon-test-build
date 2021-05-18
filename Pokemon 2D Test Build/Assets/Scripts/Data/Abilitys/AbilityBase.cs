using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbilityBase
{
    public abstract AbilityID Id { get; }
    public abstract AbilityBase ReturnDerivedClassAsNew();
    public string Name { get { return GlobalTools.SplitCamelCase(Id.ToString()); } }
    public abstract string Description();
    public virtual float BoostsAStatWhenAffectedWithAStatusCondition(ConditionID iD, StatAttribute benefitialStat) { return 1f; }
    public virtual bool NegatesStatusEffectStatDropFromCondition(ConditionID iD, StatAttribute stat) { return false; }
    public virtual bool PreventStatFromBeingLowered(StatAttribute stat) { return false; }
    public virtual bool IgnoreStatIncreases(StatAttribute stat) { return false; }//Only used on Keen Eye and only implimented in Check if move Hits
    public virtual string OnAbilitityActivation(Pokemon pokemon) { return ""; }
    /// <summary>
    /// Activates a weather effect for five turns upon entry
    /// </summary>
    /// <returns></returns>
    public virtual WeatherEffectID OnStartWeatherEffect() { return WeatherEffectID.NA; }
    public virtual StatBoost OnEntryLowerStat() { return null; }
    public virtual bool PreventsCriticalHits() { return false; }
    /// <summary>
    /// Checks the pokemons health if it is 1/3 or less
    ///Checks the type of move it is, if true then give it a bonus of 50%
    /// </summary>
    public virtual float BoostACertainTypeInAPinch(Pokemon attackingPokemon, ElementType attackType) { return 1f; }
    protected const float HpRequiredToActivatePinch = 1 / 3;
    public virtual float DoublesSpeedInAWeatherEffect(WeatherEffectID iD) { return 1f; }
    public virtual StatBoost BoostStatSharplyIfAnyStatLowered() { return null; }
    public virtual int DoublesAStat(StatAttribute stat) { return 1; }
    public virtual bool NegatesWeatherEffects() { return false; }
    public virtual ConditionID ContactMoveMayCauseStatusEffect(Pokemon defendingPokemon, Pokemon attackingPokemon, MoveBase currentAttack) { return ConditionID.NA; }
    public virtual float PowerUpCertainMoves(Pokemon attackingPokemon, Pokemon defendingPokemon, MoveBase currentMove) { return 1f; }
    public virtual bool MaximizeMultistrikeMovesHit() { return false; }
    public virtual MoveBase ChangeMovesToDifferentTypeAndIncreasesTheirPower(MoveBase move) { return move; }
    public virtual bool PreventCertainStatusCondition(ConditionID iD) { return false; }
    public virtual bool PreventFoeFromEscapingBattle() { return false; }
}
