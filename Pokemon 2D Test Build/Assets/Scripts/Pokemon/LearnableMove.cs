using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LearnableMove
{
    [SerializeField]
    MoveBase _moveBase;
    [SerializeField]
    int _levelLeanred;

    public MoveBase moveBase
    {
        get { return _moveBase; }
    }

    public int levelLearned
    {
        get { return _levelLeanred; }
    }

}
