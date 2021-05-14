using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ConditionBase
{
    public abstract ConditionID Id { get; }
    public string Name { get { return Id.ToString(); } }
    public virtual string StartMessage(Pokemon pokemon) { return ""; }
    public virtual bool HasCondition(ConditionID conditionID) { return (conditionID == Id); }
    public virtual string HasConditionMessage(Pokemon pokemon) { return ""; }
    public virtual float StatEffectedByCondition(StatAttribute statAttribute) { return 1f; }
    /// <summary>
    /// checks to see if the source pokemon can attack
    /// </summary>
    /// <param name="source">needs reference to pokemon name</param>
    /// <param name="target">needs reference to pokemon name</param>
    /// <returns>if the pokemons move will be successful</returns>
    public virtual bool OnBeforeMove(Pokemon source, Pokemon target = null) { return true; }
    public virtual void OnEndTurn(Pokemon pokemon) { }
    public int StatusTime { get; set; }
    public virtual bool PreventsEscape() { return false; }
    public virtual bool PreventsItemUse() { return false; }
    public virtual bool PreventsHealing() { return false; }
    public virtual ElementType IdentifiedAndRemovesImmunityFromType(Pokemon pokemon) { return ElementType.NA; }
    public virtual bool RequiredConditionToWork(ConditionID iD) { return true; }
}
