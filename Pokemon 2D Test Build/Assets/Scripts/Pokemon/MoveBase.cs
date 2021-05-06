using System.Collections;
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
    [SerializeField] List<MoveSecondaryEffects> secondaryEffects;
    [SerializeField] MoveTarget target;

    [SerializeField] int power;
    [SerializeField] int accuracy;
    [SerializeField] bool alwaysHits;
    [SerializeField] int priority;
    [SerializeField] int powerPoints;
    [SerializeField] float baseCriticalHitRate;

    [SerializeField] bool physicalContact;
    [SerializeField] bool soundType;
    [SerializeField] bool punchMove;
    [SerializeField] bool bitingMove;
    [SerializeField] bool snatchable;
    [SerializeField] bool affectedByGravity;
    [SerializeField] bool defrostsWhenUsed;
    [SerializeField] bool reflectedByMagicCoatMagicBounce;
    [SerializeField] bool blockedByProtectDetect;
    [SerializeField] bool copyableByMirrorMove;

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
    public bool AlwaysHits
    {
        get { return alwaysHits; }
    }
    public int Priority
    {
        get { return priority; }
    }
    public int PowerPoints
    {
        get { return powerPoints; }
    }

    public MoveEffects MoveEffects
    {
        get { return moveEffects; }
    }
    public List<MoveSecondaryEffects> SecondaryEffects
    {
        get { return secondaryEffects; }
    }
    public MoveTarget Target
    {
        get { return target; }
    }

    public float BaseCriticalHitRate
    {
        get { return baseCriticalHitRate; }
    }

    public bool PhysicalContact
    {
        get { return physicalContact; }
    }

    public bool SoundType
    {
        get { return soundType; }
    }

    public bool PunchMove
    {
        get { return punchMove; }
    }

    public bool BitingMove
    {
        get { return bitingMove; }
    }
    public bool Snatchable
    {
        get { return snatchable; }
    }
    public bool AffectedByGravity
    {
        get { return affectedByGravity; }
    }

    public bool DefrostsWhenUsed
    {
        get { return defrostsWhenUsed; }
    }

    public bool ReflectedByMagicCoatMagicBounce
    {
        get { return reflectedByMagicCoatMagicBounce; }
    }

    public bool BlockedByProtectDetect
    {
        get { return blockedByProtectDetect; }
    }

    public bool CopyableByMirrorMove
    {
        get { return copyableByMirrorMove; }
    }

    #endregion
}
