using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SpecializedMoves
{
    public static void Initialization()
    {
        struggle = Resources.Load<MoveBase>("Moveset/Physical/Struggle");
    }
    public static MoveBase struggle { get; private set; }
    public static int magnitudeNumber { get; private set; }

    public const int MAX_MULTIPLIER_STACKABLE_MOVES = 3;

    public static bool CheckIfMoveHasSpecializedConditionAndSuccessful(BattleUnit sourceUnit, BattleUnit targetUnit, MoveBase originalMove)
    {
        //Specialized conditions
        if (originalMove.SoundType == true)
        {
            return (SoundMoveSuccessful(sourceUnit));
        }
        else if (sourceUnit.pokemon.HasCurrentVolatileStatus(ConditionID.ChargingTurn) == true)
        {
            return (ChargedMoveSuccessful(sourceUnit));
        }//Physical Attack
        else if (originalMove.MoveName == "Counter")
        {
            return (sourceUnit.damagedThisTurn == true && sourceUnit.damagedReceived > 0 && targetUnit.lastMoveUsed?.moveBase.MoveType == MoveType.Physical);
        }
        else if (originalMove.MoveName == "Dream Eater")
        {
            return (DreamEaterSuccessful(targetUnit));
        }
        else if (originalMove.MoveName == "Endeavor")
        {
            return (sourceUnit.pokemon.currentHitPoints > targetUnit.pokemon.currentHitPoints);
        }
        else if (originalMove.MoveName == "Fake Out" || originalMove.MoveName == "First Impression")
        {
            return (FirstTurnOnlyMoveSuccessful(sourceUnit));
        }
        else if (originalMove.MoveName == "Focus Punch")
        {
            return (FailsIfHurtSuccessful(sourceUnit));
        }
        else if (originalMove.MoveName == "Last Resort")
        {
            return (LastResortSuccessful(sourceUnit));
        }
        else if (originalMove.MoveName == "Sucker Punch")
        {
            return (SuckerPunchSuccessful(targetUnit));
        }//Special
        else if (originalMove.MoveName == "Shell Trap")
        {
            return (ShellTrapSuccessful(sourceUnit, targetUnit));
        }
        else if (originalMove.MoveName == "Snore")
        {
            return (sourceUnit.pokemon.status?.Id == ConditionID.Sleep);
        }
        else if (originalMove.MoveName == "Synchronoise")
        {
            return (SynchronoiseSuccessfull(sourceUnit, targetUnit));
        }//Status
        else if (originalMove.MoveName == "Aurora Veil" || originalMove.MoveName == "Light Screen" 
            || originalMove.MoveName == "Mist" || originalMove.MoveName == "Reflect")
        {
            return (ShieldSuccessful(sourceUnit, targetUnit, originalMove));
        }
        else if (originalMove.MoveName == "Belly Drum")
        {
            return BellyDrumSuccessfull(sourceUnit);
        }
        else if (originalMove.MoveName == "Captivate")
        {
            return WorksOnlyWithOppositeGender(sourceUnit, targetUnit);
        }
        else if (originalMove.MoveName == "Disable")
        {
            return (DisableSuccessful(targetUnit));
        }
        else if (originalMove.MoveName == "Encore")
        {
            return EncoreSuccessful(targetUnit);
        }
        else if (originalMove.MoveName == "No Retreat")
        {
            return (NoRetreatSuccessful(sourceUnit));
        }
        else if (originalMove.MoveName == "Purify")
        {
            return (PurifySuccessful(sourceUnit));
        }
        else if (originalMove.MoveName == "Rest")
        {
            return (RestSuccessful(sourceUnit));
        }
        return true;
    }

    #region Specialized Move Functions
    static bool NoRetreatSuccessful(BattleUnit sourceUnit)
    {
        if (sourceUnit.cantEscapeGivenToSelf == true)
        {
            return false;
        }
        else
        {
            if (sourceUnit.pokemon.volatileStatus.Exists(x => x.Id == ConditionID.CantEscape))
            {
                return true;
            }
        }
        sourceUnit.cantEscapeGivenToSelf = true;
        return true;
    }

    static bool EncoreSuccessful(BattleUnit targetUnit)
    {
        if (targetUnit.lastMoveUsed == null)
        {
            return false;
        }
        else if (targetUnit.lastMoveUsed.moveBase == struggle)
        {
            return false;
        }
        else if (targetUnit.lastMoveUsed.moveBase.MoveName == "Encore")
        {
            return false;
        }
        else if (targetUnit.lastMoveUsed.pP > 0)
        {
            return true;
        }
        return false;
    }

    static bool DreamEaterSuccessful(BattleUnit targetUnit)
    {
        if (targetUnit.pokemon.status?.Id == ConditionID.Sleep)
        {
            return true;
        }
        return false;
    }

    static bool PurifySuccessful(BattleUnit sourceUnit)
    {
        if (sourceUnit.pokemon.status != null)
        {
            if (sourceUnit.pokemon.status.Id != ConditionID.NA)
            {
                sourceUnit.pokemon.CureStatus();
                sourceUnit.pokemon.statusChanges.Enqueue($"{sourceUnit.pokemon.currentName} cured its status condition");

                return true;
            }
        }
        return false;
    }

    static bool RestSuccessful(BattleUnit sourceUnit)
    {
        if (sourceUnit.pokemon.maxHitPoints - sourceUnit.pokemon.currentHitPoints > 0 || sourceUnit.pokemon.status != null)
        {
            if (sourceUnit.pokemon.HasCurrentVolatileStatus(ConditionID.HealBlock) == false)
            {
                sourceUnit.pokemon.CureStatus();
                sourceUnit.pokemon.SetStatus(ConditionID.Sleep, false);
                sourceUnit.pokemon.statusChanges.Clear();
                sourceUnit.pokemon.statusChanges.Enqueue($"{sourceUnit.pokemon.currentName} went to sleep to become healthy");
                sourceUnit.pokemon.status.StatusTime = 2;
                return true;
            }
        }
        return false;
    }

    static bool DisableSuccessful(BattleUnit targetUnit)
    {
        if (targetUnit.lastMoveUsed == null)
        {
            return false;
        }

        if (targetUnit.NoMovesAvailable() == true)
        {
            return false;
        }

        foreach (Move move in targetUnit.pokemon.moves)
        {
            if (move.disabled == true)
            {
                return false;
            }
        }

        targetUnit.lastMoveUsed.disabled = true;
        targetUnit.disabledDuration = 5;
        targetUnit.pokemon.statusChanges.Enqueue($"{targetUnit.pokemon.currentName}'s {targetUnit.lastMoveUsed.moveBase.MoveName} was disabled!");
        return true;
    }

    static float HealthRecoveryModifiers(MoveBase moveBase, WeatherEffectID iD)
    {
        if (moveBase.MoveName == "Moonlight" || moveBase.MoveName == "Synthesis" || moveBase.MoveName == "Morning Sun")
        {
            if (iD == WeatherEffectID.Sunshine)
            {
                return 1.33f;
            }
            else if (iD == WeatherEffectID.Rain || iD == WeatherEffectID.Sandstorm || iD == WeatherEffectID.Hail)
            {
                return 0.5f;
            }
        }

        if (moveBase.MoveName == "Shore Up")
        {
            if (iD == WeatherEffectID.Sandstorm)
            {
                return 1.33f;
            }
        }

        return 1f;
    }

    static bool ShieldSuccessful(BattleUnit sourceUnit, BattleUnit targetUnit, MoveBase shieldType)
    {
        ShieldType shield;

        switch (shieldType.MoveName)
        {
            case "Light Screen":
                shield = ShieldType.LightScreen;
                break;
            case "Reflect":
                shield = ShieldType.Reflect;
                break;
            case "Aurora Veil":
                shield = ShieldType.AuroraVeil;
                break;
            default:
                shield = ShieldType.Mist;
                break;
        }

        if (sourceUnit.shields.Exists(x => x.GetShieldType == shield) == true)
        {
            return false;
        }

        if (shield == ShieldType.AuroraVeil)
        {
            if (BattleSystem.GetCurrentWeather != WeatherEffectID.Hail)
            {
                return false;
            }

            if (sourceUnit.pokemon.ability.NegatesWeatherEffects() == true || targetUnit.pokemon.ability.NegatesWeatherEffects() == true)
            {
                return false;
            }
        }

        ShieldBase shieldBase;

        switch (shield)
        {
            case ShieldType.LightScreen:
                shieldBase = new LightScreen(sourceUnit.pokemon.GetCurrentItem);
                break;
            case ShieldType.Reflect:
                shieldBase = new Reflect(sourceUnit.pokemon.GetCurrentItem);
                break;
            case ShieldType.AuroraVeil:
                shieldBase = new AuroraVeil(sourceUnit.pokemon.GetCurrentItem);
                break;
            default:
                shieldBase = new Mist(sourceUnit.pokemon.GetCurrentItem);
                break;
        }

        sourceUnit.shields.Add(shieldBase);
        sourceUnit.pokemon.statusChanges.Clear();
        sourceUnit.pokemon.statusChanges.Enqueue(shieldBase.StartMessage(sourceUnit.isPlayerPokemon));
        return true;
    }

    static bool FirstTurnOnlyMoveSuccessful(BattleUnit attackingUnit)
    {
        return (attackingUnit.turnsOnField < 1);
    }

    static bool FailsIfHurtSuccessful(BattleUnit attackingUnit)
    {
        return !attackingUnit.damagedThisTurn;
    }

    static bool LastResortSuccessful(BattleUnit attackingUnit)
    {
        foreach (Move move in attackingUnit.pokemon.moves)
        {
            if (move.moveBase.MoveName == "Last Resort")
            {
                continue;
            }

            if (move.pP > 0)
            {
                return false;
            }
        }
        return true;
    }

    static bool SuckerPunchSuccessful(BattleUnit defendingUnit)
    {
        for (int i = 0; i < BattleSystem.currentTurnDetails.Count; i++)
        {
            if (defendingUnit == BattleSystem.currentTurnDetails[i].attackingPokemon)
            {
                return true;
            }
        }
        return false;
    }

    static bool SoundMoveSuccessful(BattleUnit attackingUnit)
    {
        return !(attackingUnit.cantUseSoundMoves > 0);
    }

    static bool ChargedMoveSuccessful(BattleUnit attackingUnit)
    {
        bool attackSucessful = !(((ChargingTurn)attackingUnit.pokemon.volatileStatus.Find(x => x.Id == ConditionID.ChargingTurn)).hitCancelsAttack == true);
        attackingUnit.pokemon.CureVolatileStatus(ConditionID.ChargingTurn);
        return attackSucessful;
    }

    static bool ShellTrapSuccessful(BattleUnit attackingUnit,BattleUnit defendingUnit)
    {
        if(attackingUnit.damagedThisTurn == true)
        {
            if(defendingUnit.lastMoveUsed.moveBase.MoveType == MoveType.Physical)
            {
                return true;
            }
        }
        return false;
    }

    static bool SynchronoiseSuccessfull(BattleUnit attackingUnit,BattleUnit defendingUnit)
    {
        if(attackingUnit.pokemon.pokemonType1 == defendingUnit.pokemon.pokemonType1)
        {
            return true;
        }
        else if(attackingUnit.pokemon.pokemonType2 != ElementType.NA)
        {
            if (attackingUnit.pokemon.pokemonType2 == defendingUnit.pokemon.pokemonType2)
            {
                return true;
            }
        }
        return false;
    }

    static bool WorksOnlyWithOppositeGender(BattleUnit sourceUnit,BattleUnit targetUnit)
    {
        if(sourceUnit.pokemon.gender.HasValue || targetUnit.pokemon.gender.HasValue)
        {
            return false;
        }

        if(sourceUnit.pokemon.gender != targetUnit.pokemon.gender)
        {
            return true;
        }
        return false;
    }

    static bool BellyDrumSuccessfull(BattleUnit sourceUnit)
    {
        if(sourceUnit.pokemon.statBoosts[StatAttribute.Attack] >= PokemonBase.MAX_STAT_BOOST_MULTIPLIER)
        {
            return false;
        }
        int currentHP = Mathf.CeilToInt((float)sourceUnit.pokemon.maxHitPoints * 0.5f);
        return (sourceUnit.pokemon.currentHitPoints > currentHP);
    }

    #endregion

    public static MoveBase SpecifiedMovesWithConditions(BattleUnit attackingUnit, BattleUnit defendingUnit, MoveBase originalMove,MoveBase alteredMove,int currentMovePP)
    {
        alteredMove = originalMove.Clone();

        //Remove the prior effects of the move
        if (originalMove.SecondaryEffects.Exists(x => x.Volatiletatus == ConditionID.ChargingTurn) == true)
        {
            if (attackingUnit.pokemon.HasCurrentVolatileStatus(ConditionID.ChargingTurn) == true)
            {
                int chargingIndex = alteredMove.SecondaryEffects.FindIndex(x => x.Volatiletatus == ConditionID.ChargingTurn);
                for (int i = alteredMove.SecondaryEffects.Count - 1; i >= 0; i--)
                {
                    if(i<= chargingIndex)
                    {
                        alteredMove.SecondaryEffects.RemoveAt(i);
                    }
                }
            }
        }

        if (originalMove.MoveName == "Acrobatics")
        {
            if (attackingUnit.pokemon.GetHoldItemEffects == null)
            {
                alteredMove.AdjustedMovePower(1);
            }
        }
        else if (originalMove.MoveName == "Assurance" || originalMove.MoveName == "Avalanche" || originalMove.MoveName == "Revenge")
        {
            if (attackingUnit.damagedThisTurn == true)
            {
                alteredMove.AdjustedMovePower(1);
            }
        }
        else if (originalMove.MoveName == "Crush Grip" || originalMove.MoveName == "Wring Out")
        {
            float adjustment = 110 * (1 - ((float)defendingUnit.pokemon.currentHitPoints / (float)defendingUnit.pokemon.maxHitPoints));
            alteredMove.AdjustedMovePower(adjustment);
        }
        else if (originalMove.MoveName == "Facade")
        {
            if (attackingUnit.pokemon.status != null)
            {
                alteredMove.AdjustedMovePower(1);
            }
        }
        else if (originalMove.MoveName == "Flail" || originalMove.MoveName == "Reversal")
        {
            float pokemonHealthPercentage = (float)attackingUnit.pokemon.currentHitPoints / (float)attackingUnit.pokemon.maxHitPoints;
            switch (pokemonHealthPercentage)
            {
                case float n when (n >= 0.6875):
                    alteredMove.AdjustedMovePower(20);
                    break;
                case float n when (n < 0.6875 && n >= 0.3542):
                    alteredMove.AdjustedMovePower(40);
                    break;
                case float n when (n < 0.3542 && n >= 0.2083):
                    alteredMove.AdjustedMovePower(80);
                    break;
                case float n when (n < 0.2083 && n >= 0.1042):
                    alteredMove.AdjustedMovePower(100);
                    break;
                case float n when (n < 0.1042 && n >= 0.0417):
                    alteredMove.AdjustedMovePower(150);
                    break;
                default:// <4.17
                    alteredMove.AdjustedMovePower(200);
                    break;
            }
        }
        else if (originalMove.MoveName == "Fury Cutter" || originalMove.MoveName == "Round")
        {
            if (attackingUnit.lastMoveUsedConsecutively < MAX_MULTIPLIER_STACKABLE_MOVES)
            {
                alteredMove.AdjustedMovePower(attackingUnit.lastMoveUsedConsecutively);
            }
            else
            {
                alteredMove.AdjustedMovePower(MAX_MULTIPLIER_STACKABLE_MOVES);
            }
        }
        else if (originalMove.MoveName == "Gyro Ball")
        {
            float adjustment = 25 * (defendingUnit.pokemon.speed / attackingUnit.pokemon.speed);
            adjustment = Mathf.Clamp(adjustment, 1, 150);
            alteredMove.AdjustedMovePower(adjustment);
        }
        else if (originalMove.MoveName == "Knock Off")
        {
            if (defendingUnit.pokemon.GetCurrentItem != null)
            {
                alteredMove.AdjustedMovePower(.5f);
            }
        }
        else if (originalMove.MoveName == "Magnitude")
        {
            magnitudeNumber = Random.Range(0, 101);
            switch (magnitudeNumber)
            {
                case int n when (n <= 5):
                    alteredMove.AdjustedMovePower(10);
                    magnitudeNumber = 4;
                    break;
                case int n when (n <= 15 && n > 5):
                    alteredMove.AdjustedMovePower(30);
                    magnitudeNumber = 5;
                    break;
                case int n when (n <= 35 && n > 15):
                    alteredMove.AdjustedMovePower(50);
                    magnitudeNumber = 6;
                    break;
                case int n when (n <= 65 && n > 35):
                    alteredMove.AdjustedMovePower(70);
                    magnitudeNumber = 7;
                    break;
                case int n when (n <= 85 && n > 65):
                    alteredMove.AdjustedMovePower(90);
                    magnitudeNumber = 8;
                    break;
                case int n when (n <= 95 && n > 85):
                    alteredMove.AdjustedMovePower(110);
                    magnitudeNumber = 9;
                    break;
                default:// > 95
                    alteredMove.AdjustedMovePower(150);
                    magnitudeNumber = 10;
                    break;
            }
        }
        else if (originalMove.MoveName == "Payback")
        {
            if (BattleSystem.currentTurnDetails.Count <= 1)
            {
                alteredMove.AdjustedMovePower(1);
            }
        }
        else if (originalMove.MoveName == "Power Trip")
        {
            int bonusMultiplier = 0;
            for (int i = (int)StatAttribute.Attack; i < (int)StatAttribute.Accuracy; i++)
            {
                StatAttribute statAttribute = (StatAttribute)i;
                if (defendingUnit.pokemon.statBoosts[statAttribute] > 0)
                {
                    bonusMultiplier += defendingUnit.pokemon.statBoosts[statAttribute];
                }
            }
            alteredMove.AdjustedMovePower(bonusMultiplier);
        }
        else if (originalMove.MoveName == "Punishment")
        {
            int bonusMultiplier = 0;
            for (int i = (int)StatAttribute.Attack; i < (int)StatAttribute.Speed; i++)
            {
                StatAttribute statAttribute = (StatAttribute)i;
                if (defendingUnit.pokemon.statBoosts[statAttribute] > 0)
                {
                    bonusMultiplier += defendingUnit.pokemon.statBoosts[statAttribute];
                }
            }
            bonusMultiplier = Mathf.Clamp((60 + (bonusMultiplier * 20)), 60, 200);

            alteredMove.AdjustedMovePower(bonusMultiplier);
        }
        else if (originalMove.MoveName == "Rage")
        {
            attackingUnit.enraged = true;
        }
        else if (originalMove.MoveName == "Smelling Salts")
        {
            if (attackingUnit.pokemon.status?.Id == ConditionID.Paralyzed)
            {
                alteredMove.AdjustedMovePower(1);
            }
        }
        else if (originalMove.MoveName == "Stomping Tantrum")
        {
            if (attackingUnit.previousMoveFailed == true)
            {
                alteredMove.AdjustedMovePower(1);
            }
        }
        else if (originalMove.MoveName == "Wake-Up Slap")
        {
            if (attackingUnit.pokemon.status?.Id == ConditionID.Sleep)
            {
                alteredMove.AdjustedMovePower(1);
            }
        }//Special Moves
        else if(originalMove.MoveName == "Blizzard")
        {
            if(BattleSystem.GetCurrentWeather == WeatherEffectID.Hail)
            {
                alteredMove.AlwaysHits = true;
            }
        }
        else if (originalMove.MoveName == "Brine")
        {
            if (defendingUnit.pokemon.currentHitPoints < (defendingUnit.pokemon.maxHitPoints / 2))
            {
                alteredMove.AdjustedMovePower(1);
            }
        }
        else if (originalMove.MoveName == "Clear Smog")
        {
            defendingUnit.pokemon.ResetStatBoosts();
        }
        else if (originalMove.MoveName == "Echoed Voice")
        {
            if (attackingUnit.lastMoveUsedConsecutively < MAX_MULTIPLIER_STACKABLE_MOVES)
            {
                if (attackingUnit.lastMoveUsedConsecutively == 2)
                {
                    alteredMove.AdjustedMovePower(attackingUnit.lastMoveUsedConsecutively + 1);
                }
                else
                {
                    alteredMove.AdjustedMovePower(attackingUnit.lastMoveUsedConsecutively);
                }
            }
            else
            {
                alteredMove.AdjustedMovePower(MAX_MULTIPLIER_STACKABLE_MOVES + 1);
            }
        }
        else if (originalMove.MoveName == "Electro Ball")
        {
            float adjustment = (defendingUnit.pokemon.speed / attackingUnit.pokemon.speed);
            if (adjustment > 0.5f)
            {
                adjustment = 60;
            }
            else if (adjustment <= 0.5f && adjustment > 0.33f)
            {
                adjustment = 80;
            }
            else if (adjustment <= 0.33f && adjustment > 0.25f)
            {
                adjustment = 120;
            }
            else
            {
                adjustment = 150;
            }

            alteredMove.AdjustedMovePower(adjustment);
        }
        else if (originalMove.MoveName == "Eruption" || originalMove.MoveName == "Water Spout")
        {
            float adjustment = (1 - ((float)defendingUnit.pokemon.currentHitPoints / (float)defendingUnit.pokemon.maxHitPoints));
            alteredMove.AdjustedMovePower(adjustment, true);
        }
        else if(originalMove.MoveName == "Hex")
        {
            if(defendingUnit.pokemon.status != null)
            {
                alteredMove.AdjustedMovePower(1);
            }
        }
        else if(originalMove.MoveName == "Hurricane" || originalMove.MoveName == "Thunder")
        {
            if(BattleSystem.GetCurrentWeather == WeatherEffectID.Rain)
            {
                alteredMove.AdjustedMoveAccuracy(100);
            }
            else if(BattleSystem.GetCurrentWeather == WeatherEffectID.Sunshine)
            {
                alteredMove.AdjustedMoveAccuracy(50);
            }
        }
        else if (originalMove.MoveName == "Psywave")
        {//Inflicts damage equal to 0.5 to 1.5 x user's level.
            float adjustment = Random.Range(0.5f,1.5f) * attackingUnit.pokemon.currentLevel;
            alteredMove.AdjustedMovePower(adjustment);
        }
        else if (originalMove.MoveName == "Stored Power")
        {
            int bonusMultiplier = 0;
            for (int i = (int)StatAttribute.Attack; i < (int)StatAttribute.Accuracy; i++)
            {
                StatAttribute statAttribute = (StatAttribute)i;
                if (attackingUnit.pokemon.statBoosts[statAttribute] > 0)
                {
                    bonusMultiplier += attackingUnit.pokemon.statBoosts[statAttribute];
                }
            }
            alteredMove.AdjustedMovePower(bonusMultiplier);
        }
        else if(originalMove.MoveName == "Trump Card")
        {
            switch (currentMovePP)
            {
                case int n when (n >= 5):
                    alteredMove.AdjustedMovePower(40);
                    break;
                case int n when (n == 4):
                    alteredMove.AdjustedMovePower(50);
                    break;
                case int n when (n == 3):
                    alteredMove.AdjustedMovePower(60);
                    break;
                case int n when (n == 2):
                    alteredMove.AdjustedMovePower(75);
                    break;
                default:// 1
                    alteredMove.AdjustedMovePower(200);
                    break;
            }
        }
        else if(originalMove.MoveName == "Venoshock")
        {
            if (attackingUnit.pokemon.status?.Id == ConditionID.Poison|| attackingUnit.pokemon.status?.Id == ConditionID.ToxicPoison)
            {
                alteredMove.AdjustedMovePower(1);
            }
        }
        else if(originalMove.MoveName == "Weather Ball")
        {
            if(BattleSystem.GetCurrentWeather == WeatherEffectID.Sunshine)
            {
                alteredMove.AdjustedMoveType(ElementType.Fire);
                alteredMove.AdjustedMovePower(1);
            }
            else if(BattleSystem.GetCurrentWeather == WeatherEffectID.Rain)
            {
                alteredMove.AdjustedMoveType(ElementType.Water);
                alteredMove.AdjustedMovePower(1);
            }
            else if (BattleSystem.GetCurrentWeather == WeatherEffectID.Hail)
            {
                alteredMove.AdjustedMoveType(ElementType.Ice);
                alteredMove.AdjustedMovePower(1);
            }
            else if (BattleSystem.GetCurrentWeather == WeatherEffectID.Sandstorm)
            {
                alteredMove.AdjustedMoveType(ElementType.Rock);
                alteredMove.AdjustedMovePower(1);
            }
        }//Status Moves
        else if(originalMove.MoveName == "Acupressure")
        {
            StatBoost statBoost = null;
            bool statsAllBoosted = true;
            for (int i = (int)StatAttribute.Attack; i < (int)StatAttribute.Accuracy; i++)
            {
                StatAttribute statAttribute = (StatAttribute)i;
                if (attackingUnit.pokemon.statBoosts[statAttribute] == 6)
                {
                    continue;
                }
                statsAllBoosted = false;
                break;
            }

            if(statsAllBoosted == false)
            {
                while(statBoost == null)
                {
                    StatAttribute statAttribute = (StatAttribute)Random.Range((int)StatAttribute.Attack, (int)StatAttribute.Accuracy);
                    if (attackingUnit.pokemon.statBoosts[statAttribute] < 6)
                    {
                        statBoost = new StatBoost(statAttribute, 2);
                    }
                }
                alteredMove.MoveEffects.Boosts.Add(statBoost);
            }
        }
        else if (originalMove.MoveName == "Moonlight" || originalMove.MoveName == "Synthesis"
            || originalMove.MoveName == "Morning Sun" || originalMove.MoveName == "Shore Up")
        {
            float hpModifer = HealthRecoveryModifiers(originalMove, BattleSystem.GetCurrentWeather);
            alteredMove.SetHPRecoveredByMultiplier(hpModifer);
        }

            return alteredMove;
    }

    public static SemiInvulnerableType ReturnSemiInvulnerableType(MoveBase move)
    {
        MoveBase originalMove = move.originalMove;
        if (originalMove.MoveName == "Bounce" || originalMove.MoveName == "Fly")
        {
            return SemiInvulnerableType.Air;
        }
        else if (originalMove.MoveName == "Dig")
        {
            return SemiInvulnerableType.Underground;
        }
        else if (originalMove.MoveName == "Dive")
        {
            return SemiInvulnerableType.Underwater;
        }
        else if(originalMove.MoveName == "Phantom Force" || originalMove.MoveName == "Shadow Force")
        {
            return SemiInvulnerableType.Vanished;
        }
        return SemiInvulnerableType.NA;
    }

    public static MoveBase RemoveCharging(MoveBase alteredMove)
    {
        if (alteredMove.SecondaryEffects.Exists(x => x.Volatiletatus == ConditionID.ChargingTurn) == true)
        {
            int chargingIndex = alteredMove.SecondaryEffects.FindIndex(x => x.Volatiletatus == ConditionID.ChargingTurn);
            for (int i = alteredMove.SecondaryEffects.Count - 1; i >= 0; i--)
            {
                if (i == chargingIndex)
                {
                    alteredMove.SecondaryEffects.RemoveAt(i);
                }
            }
        }
        return alteredMove;
    }

    public static List<MoveEffects> EffectsBeforeCharge(MoveBase alteredMove)
    {
        List<MoveEffects> effects = new List<MoveEffects>();
        if (alteredMove.SecondaryEffects.Exists(x => x.Volatiletatus == ConditionID.ChargingTurn) == true)
        {
            int chargingIndex = alteredMove.SecondaryEffects.FindIndex(x => x.Volatiletatus == ConditionID.ChargingTurn);
            for (int i = 0; i < alteredMove.SecondaryEffects.Count; i++)
            {
                if (i < chargingIndex)
                {
                    effects.Add(alteredMove.SecondaryEffects[i]);
                }
            }
        }
        return effects;
    }

    public static int DealsPresetAmountOfDamage(BattleUnit attackingUnit,BattleUnit defendingUnit,MoveBase originalMove)
    {
        if(originalMove.MoveName == "Counter")
        {
            return (attackingUnit.damagedReceived * 2);
        }
        else if(originalMove.MoveName == "Endeavor")
        {
            return (defendingUnit.pokemon.currentHitPoints - attackingUnit.pokemon.currentHitPoints);
        }
        else if(originalMove.MoveName == "Seismic Toss")
        {
            return attackingUnit.pokemon.currentLevel;
        }
        else if(originalMove.MoveName == "Super Fang")
        {
            return Mathf.FloorToInt(defendingUnit.pokemon.currentHitPoints/2);
        }

        return 0;
    }

    public static bool BreaksOpponentsShield(MoveBase originalMove)
    {
        if (originalMove.MoveName == "Brick Break" || originalMove.MoveName == "Psychic Fangs")
        {
            return true;
        }
        return false;
    }
}
