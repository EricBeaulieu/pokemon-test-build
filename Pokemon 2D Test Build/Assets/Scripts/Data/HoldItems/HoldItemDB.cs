using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HoldItemID
{
    NA,
    AbsorbBulb, AdamantOrb, AirBalloon, AmuletCoin, AssaultVest,
    BigRoot, BindingBand, BlackBelt, BlackGlasses, BlackSludge,
    BlunderPolicy, BrightPowder, BugGem, CellBattery, Charcoal,
    ChoiceBand, ChoiceScarf, ChoiceSpecs, CleanseTag, DampRock,
    DarkGem, DeepSeaScale, DeepSeaTooth, DestinyKnot, DragonFang,
    DragonGem, EjectButton, EjectPack, ElectricGem, Everstone,
    Eviolite, ExpertBelt, ExpShare, FairyGem, FightingGem,
    FireGem, FlameOrb, FlyingGem, FocusBand, FocusSash,
    GhostGem, GrassGem, GripClaw, GriseousOrb, GroundGem,
    HardStone, HeatRock, HeavyDutyBoots, IceGem, IcyRock,
    KingsRock, LaggingTail, Leek, Leftovers, LifeOrb,
    LightBall, LightClay, LuckyEgg, LuckyPunch, LuminousMoss,
    LustrousOrb, MachoBrace, Magnet, MentalHerb, MetalCoat,
    MetalPowder, Metronome, MiracleSeed, MuscleBand, MysticWater,
    NeverMeltIce, NormalGem, PoisonBarb, PoisonGem, PowerAnklet,
    PowerBand, PowerBelt, PowerBracer, PowerHerb, PowerLens,
    PowerWeight, ProtectivePads, PsychicGem, QuickClaw, QuickPowder,
    RazorClaw, RazorFang, RedCard, RingTarget, RockGem,
    RockyHelmet, RoomService, SafetyGoggles, ScopeLens, SharpBeak,
    ShedShell, ShellBell, SilkScarf, SilverPowder, SmokeBall,
    SmoothRock, Snowball, SoftSand, SootheBell, SoulDew,
    SpellTag, SteelGem, StickyBarb, ThickClub, ThroatSpray,
    ToxicOrb, TwistedSpoon, UtilityUmbrella, WaterGem, WeaknessPolicy,
    WhiteHerb, WideLens, WiseGlasses, ZoomLens,    
}

public enum BerryID
{
    NA,
    Aguav,Cheri
}

public static class HoldItemDB
{
    public static void Initialization(List<HoldItemBase> holdItems)
    {
        foreach (HoldItemBase item in holdItems)
        {
            HoldItemDex.Add(item.Id, item);
        }
    }

    static Dictionary<string, HoldItemBase> HoldItemDex = new Dictionary<string, HoldItemBase>();

    public static HoldItemBase GetHoldItem()
    {
        return HoldItemDex[HoldItemID.NA.ToString()].ReturnDerivedClassAsNew();
    }

    public static HoldItemBase GetHoldItem(HoldItemID iD)
    {
        return HoldItemDex[iD.ToString()].ReturnDerivedClassAsNew();
    }

    public static HoldItemBase GetHoldItem(BerryID iD)
    {
        return HoldItemDex[iD.ToString()].ReturnDerivedClassAsNew();
    }
}
