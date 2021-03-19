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
}