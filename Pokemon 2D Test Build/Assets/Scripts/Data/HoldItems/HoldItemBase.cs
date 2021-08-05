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
    public virtual int IncreasedWeatherEffectDuration(WeatherEffectID currentWeatherEffect) { return 0; }
    public virtual bool PreventsPokemonFromEvolving() { return false; }
    public virtual bool ExperienceShared() { return false; }
    public virtual ConditionID InflictConditionAtTurnEnd() { return ConditionID.NA; }
    public virtual bool EndureOHKOAttack(Pokemon defendingPokemon) { return false; }
    public virtual bool ExtendsBindToMaxPotential() { return false; }
    public virtual bool PreventsEffectsOfEntryHazards() { return false; }
    public virtual bool AlwaysLastInSpeedPriorityTurn() { return false; }
    public virtual int EndTurnHolderAlterHp(Pokemon holder) { return 0; }
    public virtual int AlterUserHPAfterAttack(Pokemon holder,MoveBase move,int damageDealt) { return 0; }
    public virtual int ReflectLightScreenDuration(MoveBase move) { return 0; }
    public virtual float ExperienceModifier() { return 1; }
    public virtual List<EarnableEV> AdditionalEffortValues(List<EarnableEV> earnedEv) { return earnedEv; }
    public virtual bool RemovesMoveBindingEffectsAfterMoveUsed(Pokemon defendingPokemon) { return false; }
}
