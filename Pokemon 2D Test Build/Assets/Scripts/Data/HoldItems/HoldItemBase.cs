using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HoldItemBase
{
    public string Id
    {
        get
        {
            if(HoldItemId != HoldItemID.NA)
            {
                return HoldItemId.ToString();
            }
            if(BerryId != BerryID.NA)
            {
                return BerryId.ToString();
            }
            return HoldItemID.NA.ToString();
        }
    }
    public virtual HoldItemID HoldItemId { get; }
    public virtual bool PlayAnimationWhenUsed() { return true; }
    public virtual float AlterDamageTaken(BattleUnit holder,MoveBase move, bool superEffective) { return 1f; }
    public virtual float PowersUpSuperEffectiveAttacks(bool superEffective) { return 1f; }
    public virtual void OnTurnEnd(Pokemon defendingPokemon) { }
    public virtual List<StatBoost> AlterStatAfterTakingDamageFromCertainType(BattleUnit holder,MoveBase move,bool superEffective) { return null; }
    public virtual MoveBase AlterUserMoveDetails(BattleUnit holder,MoveBase move) { return move; }
    public virtual MoveBase AlterOpposingMoveDetails(MoveBase move) { return move; }
    public virtual bool DoublesPrizeMoneyRecieved() { return false; }//not implimented
    public virtual bool PreventTheUseOfCertainMoves(BattleUnit battleUnit,MoveBase move) { return false; }
    public virtual string EntryMessage(Pokemon holder) { return ""; }
    public virtual string SpecializedMessage(BattleUnit holder,Pokemon opposingPokemon) { return ""; }
    public virtual bool BindingDamageIncreased() { return false; }
    public virtual float AlterStat(Pokemon Holder,StatAttribute statAffected)
    {
        {
            if (statAffected == StatAttribute.CriticalHitRatio)
            {
                return 0;
            }
            return 1f;
        }
    }
    public virtual int IncreasedWeatherEffectDuration(WeatherEffectID currentWeatherEffect) { return 0; }
    public virtual bool PreventsPokemonFromEvolving() { return false; }//not implimented
    public virtual bool ExperienceShared() { return false; }
    public virtual ConditionID InflictConditionAtTurnEnd() { return ConditionID.NA; }
    public virtual bool EndureOHKOAttack(BattleUnit holder) { return false; }
    public virtual bool ExtendsBindToMaxPotential() { return false; }
    public virtual bool PreventsEffectsOfEntryHazards() { return false; }
    /// <summary>
    /// checks if the item adjust this pokemons attack order if all Pokémon use are within the same Speed Priority level.
    /// </summary>
    /// <returns>1 means this pokemon is going first, 0 means no changes and -1 means its always last</returns>
    public virtual int AdjustSpeedPriorityTurn(BattleUnit holder) { return 0; }
    public virtual bool HurtsAttacker() { return false; }
    public virtual int AlterUserHPAfterAttack(BattleUnit holder,MoveBase move,int damageDealt) { return 0; }
    public virtual int ShieldDurationBonus() { return 0; }
    public virtual float ExperienceModifier() { return 1; }
    public virtual List<EarnableEV> AdditionalEffortValues(List<EarnableEV> earnedEv) { return earnedEv; }
    public virtual bool RemovesMoveBindingEffectsAfterMoveUsed(BattleUnit holder) { return false; }
    public virtual bool ProtectHolderFromEffectsCausedByMakingDirectContact() { return false; }
    public virtual bool ProtectsHolderFromWeatherConditions() { return false; }
    public virtual bool AlwaysAllowsToSwitchOut() { return false; }
    public virtual bool FleeWithoutFail() { return false; }
    public virtual StatBoost AlterStatAfterUsingSpecificMove(BattleUnit holder,MoveBase move) { return null; }
    public virtual bool RestoresAllLoweredStatsToNormalAfterAttackFinished(BattleUnit holder) { return false; }
    public virtual StatBoost RaisesStatUponMissing(BattleUnit holder) { return null; }
    public virtual bool TransferToPokemon(MoveBase move) { return false; }
    public virtual float HpDrainModifier() { return 0; }
    public virtual bool ExecuteMoveWithChargingTurn() { return false; }
    public virtual bool Levitates() { return false; }

    //Berries effects
    public virtual BerryID BerryId { get; }
    public virtual bool UseInInventory() { return false; }
    public virtual bool UsedInInventoryEffect(Pokemon pokemon) { return false; }
    protected float StandardBerryUseage(Pokemon pokemon) { if (pokemon.ability.UseBerryEarly() == true) return adjustedHealthRequirement; else return standardHealthRequirement; }
    float standardHealthRequirement = 0.25f;
    float adjustedHealthRequirement = 0.5f;
    public virtual bool HealsPokemonAfterTakingDamage(BattleUnit pokemon, bool superEffective) { return false; }
    public virtual ConditionID AdditionalEffects() { return ConditionID.NA; }
    public virtual bool HealConditionAfterTakingDamage(BattleUnit holder) { return false; }
    public virtual bool AdjustAccuracyTo100(Pokemon holder) { return false; }
}
