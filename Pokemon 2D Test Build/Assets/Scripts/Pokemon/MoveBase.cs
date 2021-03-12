﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MoveType { Physical, Special, Status}

public enum MoveTarget { Foe,Self}

[CreateAssetMenu(menuName = "Pokedex/Create New Attack Entry")]
public class MoveBase : ScriptableObject {

    [SerializeField] string moveName;
    [TextArea]
    [SerializeField] string moveDescription;

    [SerializeField] ElementType elementType;
    [SerializeField] MoveType moveType;
    [SerializeField] MoveEffects moveEffects;
    [SerializeField] MoveTarget target;

    [SerializeField] int power;
    [SerializeField] int accuracy;
    [SerializeField] int powerPoints;

    #region Return Methods

    public string MoveName
    {
        get { return moveName; }
    }

    public string MoveDescription
    {
        get { return moveDescription; }
    }

    public ElementType Type
    {
        get { return elementType; }
    }

    public MoveType MoveType
    {
        get { return moveType; }
    }

    public int MovePower
    {
        get { return power; }
    }
    public int MoveAccuracy
    {
        get { return accuracy; }
    }
    public int PowerPoints
    {
        get { return powerPoints; }
    }

    public MoveEffects MoveEffects
    {
        get { return moveEffects; }
    }
    public MoveTarget Target
    {
        get { return target; }
    }

    #endregion
}
