using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum MoveType { Physical, Special, Status}

public enum MoveTarget { Foe,Self,All}

public enum Recoil { NA, DamageDealt, UsersMaximumHP, UsersCurrentHP }

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
    [SerializeField] int baseCriticalHitRate = 0;

    [SerializeField] bool multiStrikeMove;
    [SerializeField] int fixedNumberOfStrikes;

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

    [SerializeField] Recoil recoilType;
    [SerializeField] float recoilPercentage;
    
    [SerializeField] float hpRecovered;
    [SerializeField] bool leavesTargetWith1HP;
    [SerializeField] bool stealsTargetsItem;
    [SerializeField] bool bypassesTargetsStatBoosts;
    [SerializeField] bool oneHitKO;

    MoveBase _originalMove = null;
    public MoveBase originalMove
    {
        get
        {
            if (_originalMove != null)
            {
                return _originalMove;
            }
            return this;
        }
        private set
        {
            _originalMove = value;
        }
    }

    public void AdjustedMovePower(float powerIncrease,bool decrease = false)
    {
        if(decrease == false)
        {
            power += Mathf.RoundToInt(power * powerIncrease);
        }
        else
        {
            power -= Mathf.RoundToInt(power * powerIncrease);
        }
    }

    public void AdjustedMoveType(ElementType newType)
    {
        elementType = newType;
    }

    public void AdjustedMoveAccuracyPercentage(float accuracyIncrease)
    {
        accuracy += Mathf.RoundToInt(accuracy * accuracyIncrease);
    }

    public void AdjustedMoveAccuracy(int accuracySet)
    {
        accuracy = accuracySet;
    }

    public void AdjustedHPRecovered(float hpRecoveredIncrease)
    {
        hpRecovered += hpRecoveredIncrease;
    }

    public void SetHPRecoveredByMultiplier(float hpRecoveredmultiplier)
    {
        hpRecovered *= hpRecoveredmultiplier;
    }

    public void RemoveMoveSecondaryEffects()
    {
        secondaryEffects.Clear();
    }

    public void RemoveContact()
    {
        PhysicalContact = false;
    }

    public void AddSecondaryEffects(MoveSecondaryEffects newEffect)
    {
        if(secondaryEffects.Exists(x => x.Status == newEffect.Status || x.Status == newEffect.Volatiletatus))
        {
            secondaryEffects.Find(x => x.Status == newEffect.Status || x.Status == newEffect.Volatiletatus).PercentChance += newEffect.PercentChance;
        }
        else
        {
            secondaryEffects.Add(newEffect);
        }
    }

    public MoveBase Clone()
    {
        MoveBase clone = Instantiate(this);
        if(clone._originalMove == null)
        {
            clone.originalMove = this;
        }
        return clone;
    }

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
        set { alwaysHits = value; }
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
        set { moveEffects = value; }
    }
    public List<MoveSecondaryEffects> SecondaryEffects
    {
        get { return secondaryEffects; }
        set { secondaryEffects = value; }
    }
    public MoveTarget Target
    {
        get { return target; }
    }

    public int BaseCriticalHitRate
    {
        get { return baseCriticalHitRate; }
    }

    public bool PhysicalContact
    {
        get { return physicalContact; }
        private set { physicalContact = value; }
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
    
    public Recoil RecoilType
    {
        get { return recoilType; }
    }

    public float RecoilPercentage
    {
        get { return recoilPercentage; }
    }

    public bool MultiStrikeMove
    {
        get { return multiStrikeMove; }
    }

    public int FixedNumberOfStrikes
    {
        get { return fixedNumberOfStrikes; }
    }


    public bool DrainsHP
    {
        get { return hpRecovered > 0; }
    }

    public float HpRecovered
    {
        get { return hpRecovered; }
    }

    public bool LeavesTargetWith1HP
    {
        get { return leavesTargetWith1HP; }
    }

    public bool StealsTargetItem
    {
        get { return stealsTargetsItem; }
    }

    public bool BypassesTargetsStatBoosts
    {
        get { return bypassesTargetsStatBoosts; }
    }

    public bool OneHitKO
    {
        get { return oneHitKO; }
    }

    public string GetSpecializedMoveMessage(ConditionID iD)
    {
        if(secondaryEffects.FirstOrDefault(x => x.Volatiletatus == iD) != null)
        {
            return secondaryEffects.FirstOrDefault(x => x.Volatiletatus == iD).SpecialStartMessage;
        }
        return "";
    }

    #endregion
}
