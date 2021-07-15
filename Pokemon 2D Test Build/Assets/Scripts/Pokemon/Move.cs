using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move
{
    public MoveBase moveBase { get; set; }

    int currentPP;
    public int pP
    {
        get { return currentPP; }
        set
        {
            currentPP = Mathf.Clamp(value, 0, moveBase.PowerPoints);
        }
    }
    public bool disabled { get; set; }

    public Move(MoveBase mBase)
    {
        moveBase = mBase;
        pP = mBase.PowerPoints;
    }
}