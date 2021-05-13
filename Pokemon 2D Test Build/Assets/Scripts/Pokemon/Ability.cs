using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability
{
    public AbilityID Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public Func<ConditionID, StatAttribute,float> BoostsAStatWhenAffectedWithAStatusCondition { get; set; }
    public Func<ConditionID, StatAttribute, bool> NegatesStatusEffectStatDropFromCondition { get; set; }
    public Func<StatAttribute, bool> PreventStatFromBeingLowered { get; set; }
    public Func<StatAttribute, bool> IgnoreStatIncreases { get; set; }//Only used on Keen Eye and only implimented in Check if move Hits
    public Func<Pokemon,string> OnAbilitityActivation { get; set; }
    public WeatherEffectID OnStartWeatherEffect { get; set; }
    public StatBoost OnEntryLowerStat { get; set; }
    public bool PreventsCriticalHits { get; set; }
    /// <summary>
    /// Checks the pokemons health if it is 1/3 or less
    ///Checks the type of move it is, if true then give it a bonus of 50%
    /// </summary>
    public Func<Pokemon, ElementType, float> BoostACertainTypeInAPinch { get; set; }
    public Func<WeatherEffectID, float> DoublesSpeedInAWeatherEffect { get; set; }
    public StatBoost BoostStatSharplyIfAnyStatLowered { get; set; }
    public Func<StatAttribute, int> DoublesAStat { get; set; }
    public bool NegatesWeatherEffects { get; set; }
    public Func<Pokemon,Pokemon, MoveBase, ConditionID> ContactMoveMayCauseStatusEffect { get; set; }
    public Func<Pokemon,Pokemon,MoveBase,float> PowerUpCertainMoves { get; set; }
    public bool MaximizeMultistrikeMovesHit { get; set; }
    public Func<MoveBase,MoveBase> ChangeMovesToDifferentTypeAndIncreasesTheirPower { get; set; }
    public Func<ConditionID,bool> PreventCertainStatusCondition { get; set; }
    public bool PreventFoeFromEscapingBattle { get; set; }
}
