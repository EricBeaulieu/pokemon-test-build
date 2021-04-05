using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability
{
    public AbilityID Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    //public string StartMessage { get; set; }
    public Func<ConditionID, StatAttribute,float> BoostsAStatWhenAffectedWithAStatusCondition { get; set; }
    public Func<ConditionID, StatAttribute, bool> NegatesStatusEffectStatDropFromCondition { get; set; }
    public Func<StatAttribute, bool> PreventStatFromBeingLowered { get; set; }
    public WeatherEffectID OnStartWeatherEffect { get; set; }
    /// <summary>
    /// Checks the pokemons health if it is 1/3 or less
    ///Checks the type of move it is, if true then give it a bonus of 50%
    /// </summary>
    public Func<Pokemon, ElementType, float> BoostACertainTypeInAPinch { get; set; }
    public Func<WeatherEffectID, float> DoublesSpeedInAWeatherEffect { get; set; }
    public Action<Pokemon> OnEndTurn { get; set; }
}
