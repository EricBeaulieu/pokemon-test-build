using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AbilityID
{
    NA = -1,
    //A
    Adaptability, Aerilate, Aftermath, AirLock, Analytic, AngerPoint, Anticipation, ArenaTrap, AromaVeil,
    //B
    BadDreams, BallFetch, BattleArmor, BeastBoost, Berserk, BigPecks, Blaze, Bulletproof,
    //C
    Chlorophyll, ClearBody, CloudNine, ColorChange, Competitive, CompoundEyes, Contrary, CottonDown, CursedBody,
    CuteCharm,
    //D
    Damp, DauntlessShield, Dazzling, Defeatist, Defiant, Download, DragonsMaw, Drizzle,
    Drought, DrySkin,
    //E
    EarlyBird,EffectSpore,
    //F
    Filter, FlameBody, FlareBoost, FlashFire, FlowerGift, Fluffy, Forewarn, Frisk, FullMetalBody,
    FurCoat,
    //G
    GaleWings,Galvanize, Gooey, Guts,
    //H
    Heatproof, HugePower, Hustle, Hydration, HyperCutter, 
    //I
    IceBody, IceScales, Immunity, Infiltrator, InnerFocus, Insomnia,
    Intimidate, IntrepidSword, IronBarbs, IronFist,
    //J
    Justified,
    //K
    KeenEye, Klutz,
    //L
    LeafGuard,Levitate, Libero,LightningRod, Limber, LiquidOoze, LiquidVoice, LongReach,
    //M
    MagmaArmor, MarvelScale, MegaLauncher, MotorDrive, Multiscale,
    //N
    Neuroforce, NoGuard, Normalize,
    //O
    Oblivious, Overgrown, OwnTempo,
    //P
    Pixilate, PoisonPoint, Pressure, PrismArmor, Protean, PunkRock, PurePower,
    //Q
    QuickFeet,
    //R
    RainDish, Rattled, Reckless, Refrigerate, RoughSkin, RockHead,
    //S
    SandForce, SandRush, SandStream, SandVeil, SapSipper,
    Scrappy, ShellArmor, SkillLink, SlushRush, Sniper,
    SnowCloak, SnowWarning, SolarPower, SolidRock, Soundproof,
    SpeedBoost, Static, SteamEngine, Steelworker, StormDrain,
    StrongJaw, Sturdy, Swarm, SwiftSwim,
    //T
    TanglingHair, Technician, ThickFat, TintedLens,Torrent,
    ToughClaws, ToxicBoost, Transistor,
    //U
    //V
    VitalSpirit, VoltAbsorb,
    //W
    WaterAbsorb, WaterBubble, WaterCompaction, WaterVeil, WhiteSmoke,
    WonderGuard,
    //X
    //Y
    //Z

}

public class AbilityDB
{

    public static void Initialization(List<AbilityBase> abilityBases)
    {
        foreach (AbilityBase ability in abilityBases)
        {
            AbilityDex.Add(ability.Id, ability);
        }
    }

    static Dictionary<AbilityID, AbilityBase> AbilityDex = new Dictionary<AbilityID, AbilityBase>();

    public static AbilityBase GetAbilityBase(AbilityID iD)
    {
        return AbilityDex[iD].ReturnDerivedClassAsNew();
    }
}
