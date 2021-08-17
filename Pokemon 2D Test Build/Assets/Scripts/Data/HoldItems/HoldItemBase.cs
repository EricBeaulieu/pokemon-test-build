using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HoldItemBase
{
    public abstract HoldItemID Id { get; }
    public abstract HoldItemBase ReturnDerivedClassAsNew();
    public bool RemoveItem { get; protected set; }
    public virtual bool PlayAnimationWhenUsed() { return true; }
    public virtual float AlterDamageTaken(MoveBase move) { return 1f; }
    public virtual float PowersUpSuperEffectiveAttacks(bool superEffective) { return 1f; }
    public virtual void OnTurnEnd(Pokemon defendingPokemon) { }
    public virtual List<StatBoost> AlterStatAfterTakingDamageFromCertainType(ElementType attackType,bool superEffective) { return null; }
    public virtual MoveBase AlterUserMoveDetails(MoveBase move) { return move; }
    public virtual MoveBase AlterOpposingMoveDetails(MoveBase move) { return move; }
    public virtual bool DoublesPrizeMoneyRecieved() { return false; }//not implimented
    public virtual bool PreventTheUseOfCertainMoves(BattleUnit battleUnit,MoveBase move) { return false; }
    public virtual string EntryMessage(Pokemon holder) { return ""; }
    public virtual string SpecializedMessage(Pokemon holder,Pokemon opposingPokemon) { return ""; }
    public virtual bool BindingDamageIncreased() { return false; }
    public virtual float AlterStat(Pokemon Holder,StatAttribute statAffected)
    {
        if (statAffected == StatAttribute.CriticalHitRatio)
        {
            return 0;
        }
        return 1f;
    }
    public virtual int IncreasedWeatherEffectDuration(WeatherEffectID currentWeatherEffect) { return 0; }
    public virtual bool PreventsPokemonFromEvolving() { return false; }//not implimented
    public virtual bool ExperienceShared() { return false; }
    public virtual ConditionID InflictConditionAtTurnEnd() { return ConditionID.NA; }
    public virtual bool EndureOHKOAttack(Pokemon defendingPokemon) { return false; }
    public virtual bool ExtendsBindToMaxPotential() { return false; }
    public virtual bool PreventsEffectsOfEntryHazards() { return false; }
    /// <summary>
    /// checks if the item adjust this pokemons attack order if all Pokémon use are within the same Speed Priority level.
    /// </summary>
    /// <returns>1 means this pokemon is going first, 0 means no changes and -1 means its always last</returns>
    public virtual int AdjustSpeedPriorityTurn() { return 0; }//not implimented
    public virtual int AlterUserHPAfterAttack(Pokemon holder,MoveBase move,int damageDealt) { return 0; }
    public virtual int ReflectLightScreenDuration(MoveBase move) { return 0; }
    public virtual float ExperienceModifier() { return 1; }
    public virtual List<EarnableEV> AdditionalEffortValues(List<EarnableEV> earnedEv) { return earnedEv; }
    public virtual bool RemovesMoveBindingEffectsAfterMoveUsed(Pokemon defendingPokemon) { return false; }
    public virtual bool ProtectHolderFromEffectsCausedByMakingDirectContact() { return false; }
    public virtual bool ProtectsHolderFromWeatherConditions() { return false; }
    public virtual bool AlwaysAllowsToSwitchOut() { return false; }
    public virtual bool FleeWithoutFail() { return false; }
    public virtual StatBoost AlterStatAfterUsingSpecificMove(MoveBase move) { return null; }
    public virtual bool RestoresAllLoweredStatsToNormalAfterAttackFinished(Pokemon holder) { return false; }
    public virtual StatBoost RaisesStatUponMissing() { return null; }
    public virtual bool TransferToPokemon(MoveBase move) { return false; }
}
