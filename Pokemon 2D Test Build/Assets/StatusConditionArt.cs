using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusConditionArt : MonoBehaviour
{
    static StatusConditionArt _instance = null;

    [SerializeField] Sprite nothing;

    [Header("Status")]
    [SerializeField] Sprite conditionPoison;
    [SerializeField] Sprite conditionBurn;
    [SerializeField] Sprite conditionSleep;
    [SerializeField] Sprite conditionParalyzed;
    [SerializeField] Sprite conditionFrozen;

    [Header("Gender")]
    [SerializeField] Sprite male;
    [SerializeField] Sprite female;

    [Header("Element")]
    [SerializeField] Sprite nA;
    [SerializeField] Sprite bug;
    [SerializeField] Sprite dark;
    [SerializeField] Sprite dragon;
    [SerializeField] Sprite electric;
    [SerializeField] Sprite fairy;
    [SerializeField] Sprite fighting;
    [SerializeField] Sprite fire;
    [SerializeField] Sprite flying;
    [SerializeField] Sprite ghost;
    [SerializeField] Sprite grass;
    [SerializeField] Sprite ground;
    [SerializeField] Sprite ice;
    [SerializeField] Sprite normal;
    [SerializeField] Sprite poison;
    [SerializeField] Sprite psychic;
    [SerializeField] Sprite rock;
    [SerializeField] Sprite steel;
    [SerializeField] Sprite water;

    public static StatusConditionArt instance
    {
        get { return _instance; }
        set { _instance = value; }
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public Sprite Nothing
    {
        get { return nothing; }
    }

    public Sprite ReturnStatusConditionArt(ConditionID currentCondition)
    {
        switch (currentCondition)
        {
            case ConditionID.poison:
                return conditionPoison;
            case ConditionID.burn:
                return conditionBurn;
            case ConditionID.sleep:
                return conditionSleep;
            case ConditionID.paralyzed:
                return conditionParalyzed;
            case ConditionID.frozen:
                return conditionFrozen;
            case ConditionID.toxicPoison:
                return conditionPoison;
        }
        return nothing;
    }

    public Sprite ReturnGenderArt(Gender gender)
    {
        switch (gender)
        {
            case Gender.Male:
                return male;
            case Gender.Female:
                return female;
        }
        return nothing;
    }

    public Sprite ReturnElementArt(ElementType type)
    {
        switch (type)
        {
            case ElementType.Bug:
                return bug;
            case ElementType.Dark:
                return dark;
            case ElementType.Dragon:
                return dragon;
            case ElementType.Electric:
                return electric;
            case ElementType.Fairy:
                return fairy;
            case ElementType.Fighting:
                return fighting;
            case ElementType.Fire:
                return fire;
            case ElementType.Flying:
                return flying;
            case ElementType.Ghost:
                return ghost;
            case ElementType.Grass:
                return grass;
            case ElementType.Ground:
                return ground;
            case ElementType.Ice:
                return ice;
            case ElementType.Normal:
                return normal;
            case ElementType.Poison:
                return poison;
            case ElementType.Psychic:
                return psychic;
            case ElementType.Rock:
                return rock;
            case ElementType.Steel:
                return steel;
            case ElementType.Water:
                return water;
            default:
                return nA;
        }
    }
}
