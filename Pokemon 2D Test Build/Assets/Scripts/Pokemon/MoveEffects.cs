using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StatBoost
{
    public StatAttribute stat;
    public int boost;
}

[System.Serializable]
public class MoveEffects
{
    [SerializeField] List<StatBoost> boosts;
    [SerializeField] ConditionID status;
    [SerializeField] ConditionID volatileStatus;
    [SerializeField] WeatherEffectID weatherEffect;
    [SerializeField] EntryHazardID entryHazard;
    [SerializeField] string specialStartMessage;

    public List<StatBoost> Boosts
    {
        get { return boosts; }
    }

    public ConditionID Status
    {
        get { return status; }
    }

    public ConditionID Volatiletatus
    {
        get { return volatileStatus; }
    }

    public WeatherEffectID WeatherEffect
    {
        get { return weatherEffect; }
    }

    public EntryHazardID EntryHazard
    {
        get { return entryHazard; }
    }

    public string SpecialStartMessage
    {
        get { return specialStartMessage; }
    }
}
