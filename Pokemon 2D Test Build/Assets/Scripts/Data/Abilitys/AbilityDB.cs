using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AbilityID
{
    NA,
    //A
    Adaptability, Aerilate, AirLock, AngerPoint,
    //B
    BattleArmor, BigPecks, Blaze, Bulletproof,
    //C
    Chlorophyll, ClearBody, CloudNine, Competitive, CompoundEyes,
    CuteCharm,
    //D
    Damp, Defiant, Download, DragonsMaw, Drizzle,
    Drought, DrySkin,
    //E
    EffectSpore,
    //F
    Filter, FlameBody, FlareBoost, FlashFire, FullMetalBody,
    FurCoat,
    //G
    Galvanize, Gooey, Guts,
    //H
    Heatproof, HugePower, HyperCutter,
    //I
    IceBody, IceScales, Immunity, InnerFocus, Insomnia,
    Intimidate, IronFist,
    //J
    Justified,
    //K
    KeenEye,
    //L
    LeafGuard,Levitate, LightningRod, Limber,
    //M
    MagmaArmor, MarvelScale, MegaLauncher, MotorDrive, Multiscale,
    //N
    Neuroforce, NoGuard, Normalize,
    //O
    Oblivious, Overgrown, OwnTempo,
    //P
    Pixilate, PoisonPoint, PrismArmor, PunkRock, PurePower,
    //Q
    QuickFeet,
    //R
    RainDish, Rattled, Reckless, Refrigerate,
    //S
    SandForce, SandRush, SandStream, SandVeil, SapSipper,
    Scrappy, ShellArmor, SkillLink, SlushRush, Sniper,
    SnowCloak, SnowWarning, SolarPower, SolidRock, Soundproof,
    SpeedBoost, Static, SteamEngine, Steelworker, StormDrain,
    StrongJaw, Swarm, SwiftSwim,
    //T
    TanglingHair, Technician, ThickFat, TintedLens,Torrent,
    ToughClaws, ToxicBoost, Transistor,
    //U
    //V
    VitalSpirit,
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

            if (ability.Description() == "")
            {
                Debug.LogWarning($"{ability.Id} Description is not preset");
            }
        }
    }

    static Dictionary<AbilityID, AbilityBase> AbilityDex = new Dictionary<AbilityID, AbilityBase>();

    public static AbilityBase GetAbilityBase(AbilityID iD)
    {
        return AbilityDex[iD].ReturnDerivedClassAsNew();
    }
}
