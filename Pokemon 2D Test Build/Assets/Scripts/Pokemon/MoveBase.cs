using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MoveType { Physical, Special, Status}

[CreateAssetMenu(menuName = "Pokedex/Create New Attack Entry")]
public class MoveBase : ScriptableObject {

    [SerializeField]
    string _name;
    [TextArea]
    [SerializeField]
    string _description;

    [SerializeField]
    ElementType _elementType;
    [SerializeField]
    MoveType _moveType;
    [SerializeField]
    int _power;
    [SerializeField]
    int _accuracy;
    [SerializeField]
    int _powerPoints;

    #region Return Methods

    public string moveName
    {
        get { return _name; }
    }

    public string description
    {
        get { return _description; }
    }

    public ElementType type
    {
        get { return _elementType; }
    }

    public MoveType moveType
    {
        get { return _moveType; }
    }

    public int power
    {
        get { return _power; }
    }
    public int accuracy
    {
        get { return _accuracy; }
    }
    public int powerPoints
    {
        get { return _powerPoints; }
    }

    #endregion
}
