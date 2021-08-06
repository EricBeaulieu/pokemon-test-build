using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HoldItemID
{
    NA,
    AbsorbBulb, AdamantOrb ,AirBalloon, AmuletCoin, AssaultVest,
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
    PowerWeight,

    //Berries
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

    static Dictionary<HoldItemID, HoldItemBase> HoldItemDex = new Dictionary<HoldItemID, HoldItemBase>();

    public static HoldItemBase GetHoldItem(HoldItemID iD)
    {
        return HoldItemDex[iD].ReturnDerivedClassAsNew();
    }
}
