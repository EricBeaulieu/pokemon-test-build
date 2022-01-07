using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SpecializedMoves
{
    public static void Initialization(SpecializedMovesHelper helper)
    {
        //Specialized Moves
        struggle = helper.struggle;

        //Physical
        acrobatics = helper.acrobatics;
        assurance = helper.assurance;
        avalanche = helper.avalanche;
        bounce = helper.bounce;
        brickBreak = helper.brickBreak;
        counter = helper.counter;
        crushGrip = helper.crushGrip;
        cut = helper.cut;
        dig = helper.dig;
        dive = helper.dive;
        earthquake = helper.earthquake;
        endeavor = helper.endeavor;
        facade = helper.facade;
        fakeOut = helper.fakeOut;
        fellStinger = helper.fellStinger;
        firstImpression = helper.firstImpression;
        fissure = helper.fissure;
        flail = helper.flail;
        fly = helper.fly;
        focusPunch = helper.focusPunch;
        foulPlay = helper.foulPlay;
        freezeShock = helper.freezeShock;
        furyCutter = helper.furyCutter;
        gyroBall = helper.gyroBall;
        highJumpKick = helper.highJumpKick;
        jumpKick = helper.jumpKick;
        knockOff = helper.knockOff;
        lastResort = helper.lastResort;
        magnitude = helper.magnitude;
        payback = helper.payback;
        payDay = helper.payDay;
        phantomForce = helper.phantomForce;
        powerTrip = helper.powerTrip;
        psychicFangs = helper.psychicFangs;
        punishment = helper.punishment;
        rage = helper.rage;
        revenge = helper.revenge;
        reversal = helper.reversal;
        seismicToss = helper.seismicToss;
        shadowForce = helper.shadowForce;
        smackDown = helper.smackDown;
        smellingSalts = helper.smellingSalts;
        solarBlade = helper.solarBlade;
        stompingTantrum = helper.stompingTantrum;
        suckerPunch = helper.suckerPunch;
        superFang = helper.superFang;
        thousandArrows = helper.thousandArrows;
        throatChop = helper.throatChop;
        tripleKick = helper.tripleKick;
        wakeUpSlap = helper.wakeUpSlap;

        //Special
        blizzard = helper.blizzard;
        brine = helper.brine;
        clearSmog = helper.clearSmog;
        dreamEater = helper.dreamEater;
        echoedVoice = helper.echoedVoice;
        electroBall = helper.electroBall;
        eruption = helper.eruption;
        gust = helper.gust;
        hex = helper.hex;
        hurricane = helper.hurricane;
        psywave = helper.psywave;
        round = helper.round;
        shellTrap = helper.shellTrap;
        snore = helper.snore;
        solarBeam = helper.solarBeam;
        storedPower = helper.storedPower;
        surf = helper.surf;
        synchronoise = helper.synchronoise;
        thunder = helper.thunder;
        trumpCard = helper.trumpCard;
        twister = helper.twister;
        venoshock = helper.venoshock;
        waterSpout = helper.waterSpout;
        weatherBall = helper.weatherBall;
        whirlpool = helper.whirlpool;
        wringOut = helper.wringOut;

        //Status
        acupressure = helper.acupressure;
        auroraVeil = helper.auroraVeil;
        bellyDrum = helper.bellyDrum;
        captivate = helper.captivate;
        disable = helper.disable;
        encore = helper.encore;
        lightScreen = helper.lightScreen;
        mist = helper.mist;
        moonlight = helper.moonlight;
        morningSun = helper.morningSun;
        noRetreat = helper.noRetreat;
        purify = helper.purify;
        reflect = helper.reflect;
        rest = helper.rest;
        shoreUp = helper.shoreUp;
        synthesis = helper.synthesis;
}
    //Specialized Moves
    public static MoveBase struggle { get; private set; }

    //Physical
    public static MoveBase acrobatics { get; private set; }
    public static MoveBase assurance { get; private set; }
    public static MoveBase avalanche { get; private set; }
    public static MoveBase bounce { get; private set; }
    public static MoveBase brickBreak { get; private set; }
    public static MoveBase counter { get; private set; }
    public static MoveBase crushGrip { get; private set; }
    public static MoveBase cut { get; private set; }
    public static MoveBase dig { get; private set; }
    public static MoveBase dive { get; private set; }
    public static MoveBase earthquake { get; private set; }
    public static MoveBase endeavor { get; private set; }
    public static MoveBase facade { get; private set; }
    public static MoveBase fakeOut { get; private set; }
    public static MoveBase fellStinger { get; private set; }
    public static MoveBase firstImpression { get; private set; }
    public static MoveBase fissure { get; private set; }
    public static MoveBase flail { get; private set; }
    public static MoveBase fly { get; private set; }
    public static MoveBase focusPunch { get; private set; }
    public static MoveBase foulPlay { get; private set; }
    public static MoveBase freezeShock { get; private set; }
    public static MoveBase furyCutter { get; private set; }
    public static MoveBase gyroBall { get; private set; }
    public static MoveBase highJumpKick { get; private set; }
    public static MoveBase jumpKick { get; private set; }
    public static MoveBase knockOff { get; private set; }
    public static MoveBase lastResort { get; private set; }
    public static MoveBase magnitude { get; private set; }
    public static int magnitudeNumber { get; private set; }
    public static MoveBase payback { get; private set; }
    public static MoveBase payDay { get; private set; }
    public static MoveBase phantomForce { get; private set; }
    public static MoveBase powerTrip { get; private set; }
    public static MoveBase psychicFangs { get; private set; }
    public static MoveBase punishment { get; private set; }
    public static MoveBase rage { get; private set; }
    public static MoveBase revenge { get; private set; }
    public static MoveBase reversal { get; private set; }
    public static MoveBase seismicToss { get; private set; }
    public static MoveBase shadowForce { get; private set; }
    public static MoveBase smackDown { get; private set; }
    public static MoveBase smellingSalts { get; private set; }
    public static MoveBase solarBlade { get; private set; }
    public static MoveBase stompingTantrum { get; private set; }
    public static MoveBase suckerPunch { get; private set; }
    public static MoveBase superFang { get; private set; }
    public static MoveBase thousandArrows { get; private set; }
    public static MoveBase throatChop { get; private set; }
    public static MoveBase tripleKick { get; private set; }
    public static MoveBase wakeUpSlap { get; private set; }

    //Special
    public static MoveBase blizzard { get; private set; }
    public static MoveBase brine { get; private set; }
    public static MoveBase clearSmog { get; private set; }
    public static MoveBase dreamEater { get; private set; }
    public static MoveBase echoedVoice { get; private set; }
    public static MoveBase electroBall { get; private set; }
    public static MoveBase eruption { get; private set; }
    public static MoveBase gust { get; private set; }
    public static MoveBase hex { get; private set; }
    public static MoveBase hurricane { get; private set; }
    public static MoveBase psywave { get; private set; }
    public static MoveBase round { get; private set; }
    public static MoveBase shellTrap { get; private set; }
    public static MoveBase snore { get; private set; }
    public static MoveBase solarBeam { get; private set; }
    public static MoveBase storedPower { get; private set; }
    public static MoveBase surf { get; private set; }
    public static MoveBase synchronoise { get; private set; }
    public static MoveBase thunder { get; private set; }
    public static MoveBase trumpCard { get; private set; }
    public static MoveBase twister { get; private set; }
    public static MoveBase venoshock { get; private set; }
    public static MoveBase waterSpout { get; private set; }
    public static MoveBase weatherBall { get; private set; }
    public static MoveBase whirlpool { get; private set; }
    public static MoveBase wringOut { get; private set; }

    //Status
    public static MoveBase acupressure { get; private set; }
    public static MoveBase auroraVeil { get; private set; }
    public static MoveBase bellyDrum { get; private set; }
    public static MoveBase captivate { get; private set; }
    public static MoveBase encore { get; private set; }
    public static MoveBase noRetreat { get; private set; }
    public static MoveBase synthesis { get; private set; }
    public static MoveBase moonlight { get; private set; }
    public static MoveBase morningSun { get; private set; }
    public static MoveBase shoreUp { get; private set; }
    public static MoveBase purify { get; private set; }
    public static MoveBase rest { get; private set; }
    public static MoveBase disable { get; private set; }
    public static MoveBase reflect { get; private set; }
    public static MoveBase lightScreen { get; private set; }
    public static MoveBase mist { get; private set; }

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
        else if (originalMove == counter)
        {
            return (sourceUnit.damagedThisTurn == true && sourceUnit.damagedReceived > 0 && targetUnit.lastMoveUsed?.moveBase.MoveType == MoveType.Physical);
        }
        else if (originalMove == dreamEater)
        {
            return (DreamEaterSuccessful(targetUnit));
        }
        else if (originalMove == endeavor)
        {
            return (sourceUnit.pokemon.currentHitPoints > targetUnit.pokemon.currentHitPoints);
        }
        else if (originalMove == fakeOut || originalMove == firstImpression)
        {
            return (FirstTurnOnlyMoveSuccessful(sourceUnit));
        }
        else if (originalMove == focusPunch)
        {
            return (FailsIfHurtSuccessful(sourceUnit));
        }
        else if (originalMove == lastResort)
        {
            return (LastResortSuccessful(sourceUnit));
        }
        else if (originalMove == suckerPunch)
        {
            return (SuckerPunchSuccessful(targetUnit));
        }//Special
        else if (originalMove == shellTrap)
        {
            return (ShellTrapSuccessful(sourceUnit, targetUnit));
        }
        else if (originalMove == snore)
        {
            return (sourceUnit.pokemon.status?.Id == ConditionID.Sleep);
        }
        else if (originalMove == synchronoise)
        {
            return (SynchronoiseSuccessfull(sourceUnit, targetUnit));
        }//Status
        else if (originalMove == auroraVeil || originalMove == lightScreen || originalMove == mist || originalMove == reflect)
        {
            return (ShieldSuccessful(sourceUnit, targetUnit, originalMove));
        }
        else if (originalMove == bellyDrum)
        {
            return BellyDrumSuccessfull(sourceUnit);
        }
        else if (originalMove == captivate)
        {
            return WorksOnlyWithOppositeGender(sourceUnit, targetUnit);
        }
        else if (originalMove == disable)
        {
            return (DisableSuccessful(targetUnit));
        }
        else if (originalMove == encore)
        {
            return EncoreSuccessful(targetUnit);
        }
        else if (originalMove == noRetreat)
        {
            return (NoRetreatSuccessful(sourceUnit));
        }
        else if (originalMove == purify)
        {
            return (PurifySuccessful(sourceUnit));
        }
        else if (originalMove == rest)
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
        else if (targetUnit.lastMoveUsed.moveBase == encore)
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
        if (moveBase == moonlight || moveBase == synthesis || moveBase == morningSun)
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

        if (moveBase == shoreUp)
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

        switch (shieldType)
        {
            case MoveBase n when (n == lightScreen):
                shield = ShieldType.LightScreen;
                break;
            case MoveBase n when (n == reflect):
                shield = ShieldType.Reflect;
                break;
            case MoveBase n when (n == auroraVeil):
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
            if (move.moveBase == lastResort)
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
        if(sourceUnit.pokemon.gender == Gender.NA || targetUnit.pokemon.gender == Gender.NA)
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

        if (originalMove == acrobatics)
        {
            if (attackingUnit.pokemon.GetHoldItemEffects == null)
            {
                alteredMove.AdjustedMovePower(1);
            }
        }
        else if (originalMove == assurance || originalMove == avalanche || originalMove == revenge)
        {
            if (attackingUnit.damagedThisTurn == true)
            {
                alteredMove.AdjustedMovePower(1);
            }
        }
        else if (originalMove == crushGrip|| originalMove == wringOut)
        {
            float adjustment = 110 * (1 - ((float)defendingUnit.pokemon.currentHitPoints / (float)defendingUnit.pokemon.maxHitPoints));
            alteredMove.AdjustedMovePower(adjustment);
        }
        else if (originalMove == facade)
        {
            if (attackingUnit.pokemon.status != null)
            {
                alteredMove.AdjustedMovePower(1);
            }
        }
        else if (originalMove == flail || originalMove == reversal)
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
        else if (originalMove == furyCutter || originalMove == round)
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
        else if (originalMove == gyroBall)
        {
            float adjustment = 25 * (defendingUnit.pokemon.speed / attackingUnit.pokemon.speed);
            adjustment = Mathf.Clamp(adjustment, 1, 150);
            alteredMove.AdjustedMovePower(adjustment);
        }
        else if (originalMove == knockOff)
        {
            if (defendingUnit.pokemon.GetCurrentItem != null)
            {
                alteredMove.AdjustedMovePower(.5f);
            }
        }
        else if (originalMove == magnitude)
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
        else if (originalMove == payback)
        {
            if (defendingUnit.turnsOnField > 0 && BattleSystem.currentTurnDetails.Count <= 1)
            {
                alteredMove.AdjustedMovePower(1);
            }
        }
        else if (originalMove == powerTrip)
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
        else if (originalMove == punishment)
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
        else if (originalMove == rage)
        {
            attackingUnit.enraged = true;
        }
        else if (originalMove == smellingSalts)
        {
            if (attackingUnit.pokemon.status?.Id == ConditionID.Paralyzed)
            {
                alteredMove.AdjustedMovePower(1);
            }
        }
        else if (originalMove == stompingTantrum)
        {
            if (attackingUnit.previousMoveFailed == true)
            {
                alteredMove.AdjustedMovePower(1);
            }
        }
        else if (originalMove == wakeUpSlap)
        {
            if (attackingUnit.pokemon.status?.Id == ConditionID.Sleep)
            {
                alteredMove.AdjustedMovePower(1);
            }
        }//Special Moves
        else if(originalMove == blizzard)
        {
            if(BattleSystem.GetCurrentWeather == WeatherEffectID.Hail)
            {
                alteredMove.AlwaysHits = true;
            }
        }
        else if (originalMove == brine)
        {
            if (defendingUnit.pokemon.currentHitPoints < (defendingUnit.pokemon.maxHitPoints / 2))
            {
                alteredMove.AdjustedMovePower(1);
            }
        }
        else if (originalMove == clearSmog)
        {
            defendingUnit.pokemon.ResetStatBoosts();
        }
        else if (originalMove == echoedVoice)
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
        else if (originalMove == electroBall)
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
        else if (originalMove == eruption|| originalMove == waterSpout)
        {
            float adjustment = (1 - ((float)defendingUnit.pokemon.currentHitPoints / (float)defendingUnit.pokemon.maxHitPoints));
            alteredMove.AdjustedMovePower(adjustment, true);
        }
        else if(originalMove == hex)
        {
            if(defendingUnit.pokemon.status != null)
            {
                alteredMove.AdjustedMovePower(1);
            }
        }
        else if(originalMove == hurricane || originalMove == thunder)
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
        else if (originalMove == psywave)
        {//Inflicts damage equal to 0.5 to 1.5 x user's level.
            float adjustment = Random.Range(0.5f,1.5f) * attackingUnit.pokemon.currentLevel;
            alteredMove.AdjustedMovePower(adjustment);
        }
        else if (originalMove == storedPower)
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
        else if(originalMove == trumpCard)
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
        else if(originalMove == venoshock)
        {
            if (attackingUnit.pokemon.status?.Id == ConditionID.Poison|| attackingUnit.pokemon.status?.Id == ConditionID.ToxicPoison)
            {
                alteredMove.AdjustedMovePower(1);
            }
        }
        else if(originalMove == weatherBall)
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
        else if(originalMove == acupressure)
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
        else if (originalMove == moonlight || originalMove == synthesis || originalMove == morningSun || originalMove == shoreUp)
        {
            float hpModifer = HealthRecoveryModifiers(originalMove, BattleSystem.GetCurrentWeather);
            alteredMove.SetHPRecoveredByMultiplier(hpModifer);
        }
        
        return alteredMove;
    }

    public static SemiInvulnerableType ReturnSemiInvulnerableType(MoveBase move)
    {
        MoveBase originalMove = move.originalMove;
        if (originalMove == bounce ||originalMove == fly)
        {
            return SemiInvulnerableType.Air;
        }
        else if (originalMove == dig)
        {
            return SemiInvulnerableType.Underground;
        }
        else if (originalMove == dive)
        {
            return SemiInvulnerableType.Underwater;
        }
        else if(originalMove == phantomForce || originalMove == shadowForce)
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
        if(originalMove == counter)
        {
            return (attackingUnit.damagedReceived * 2);
        }
        else if(originalMove == endeavor)
        {
            return (defendingUnit.pokemon.currentHitPoints - attackingUnit.pokemon.currentHitPoints);
        }
        else if(originalMove == seismicToss)
        {
            return attackingUnit.pokemon.currentLevel;
        }
        else if(originalMove == superFang)
        {
            return Mathf.FloorToInt(defendingUnit.pokemon.currentHitPoints/2);
        }

        return 0;
    }

    public static bool BreaksOpponentsShield(MoveBase originalMove)
    {
        if (originalMove == SpecializedMoves.brickBreak || originalMove == SpecializedMoves.psychicFangs)
        {
            return true;
        }
        return false;
    }
}
