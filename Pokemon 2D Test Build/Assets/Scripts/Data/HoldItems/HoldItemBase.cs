using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HoldItemBase
{
    public abstract HoldItemID Id { get; }
    public abstract HoldItemBase ReturnDerivedClassAsNew();
    public bool RemoveItem { get; protected set; }
    public virtual float AlterDamageTaken(bool superEffective,MoveBase move) { return 1f; }
    public virtual void OnTurnEnd(Pokemon defendingPokemon) { }
    public virtual StatBoost AlterStatAfterTakingDamageFromCertainType(ElementType attackType) { return null; }
    public virtual MoveBase AlterUserMoveDetails(MoveBase move) { return move; }
    public virtual MoveBase AlterOpposingMoveDetails(MoveBase move) { return move; }
    public virtual bool DoublesPrizeMoneyRecieved() { return false; }
    public virtual bool PreventTheUseOfCertainMoves(BattleUnit battleUnit,MoveBase move) { return false; }
    public virtual string PreventTheUseOfCertainMoveMessage() { return ""; }
    public virtual bool BindingDamageIncreased() { return false; }
    public virtual float AlterStat(Pokemon Holder,StatAttribute statAffected) { return 1f; }
    virtual protected WeatherEffectID WeatherEffectAmplified() { return WeatherEffectID.NA; }
    public int IncreasedWeatherEffectDuration(WeatherEffectID currentWeatherEffect)
    {
        if(WeatherEffectAmplified() != WeatherEffectID.NA && WeatherEffectAmplified() == currentWeatherEffect)
        {
            return 3;
        }
        return 0;
    }
    public virtual bool PreventsPokemonFromEvolving() { return false; }
    public virtual bool ExperienceShared() { return false; }
}
