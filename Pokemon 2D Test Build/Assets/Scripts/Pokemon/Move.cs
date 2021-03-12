using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move{

    public MoveBase moveBase { get; set; }

    public int pP { get; set; }

    public Move(MoveBase mBase)
    {
        moveBase = mBase;
        pP = mBase.PowerPoints;
    }

}
