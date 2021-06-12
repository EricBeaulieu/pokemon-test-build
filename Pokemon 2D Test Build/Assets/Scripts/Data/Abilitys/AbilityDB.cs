using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AbilityID
{
    NA,
    //A
    Adaptability, Aerilate, AirLock,
    //B
    BattleArmor, BigPecks, Blaze,
    //C
    Chlorophyll, ClearBody, CloudNine, Competitive, CuteCharm,
    //D
    Defiant, DragonsMaw, Drizzle, Drought,DrySkin,
    //E
    EffectSpore,
    //F
    Filter, FlameBody, FlareBoost, FullMetalBody, FurCoat,
    //G
    Galvanize, Guts,
    //H
    HugePower, HyperCutter,
    //I
    IceBody, IceScales, Immunity, InnerFocus, Insomnia,
    Intimidate, IronFist,
    //J
    //K
    KeenEye,
    //L
    Limber,
    //M
    MagmaArmor, MarvelScale, MegaLauncher,
    //N
    Neuroforce, Normalize,
    //O
    Oblivious, Overgrown, OwnTempo,
    //P
    Pixilate, PoisonPoint, PrismArmor, PunkRock, PurePower,
    //Q
    QuickFeet,
    //R
    RainDish, Reckless, Refrigerate,
    //S
    SandRush, SandStream, SandVeil,ShellArmor, SkillLink,
    SlushRush, SnowCloak, SnowWarning, SolarPower, SolidRock,
    Static, Steelworker, StrongJaw, Swarm, SwiftSwim,
    //T
    Technician, TintedLens,Torrent, ToughClaws, ToxicBoost,
    Transistor,
    //U
    //V
    VitalSpirit,
    //W
    WaterVeil, WhiteSmoke,
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
