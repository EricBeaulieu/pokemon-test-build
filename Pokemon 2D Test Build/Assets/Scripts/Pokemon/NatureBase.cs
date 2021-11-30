using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Flavor { NA,Spicy,Sour,Dry,Bitter,Sweet}

[CreateAssetMenu(menuName = "Mechanics/Nature")]
public class NatureBase : ScriptableObject {

    [SerializeField]
    StatAttribute _raisedAttribute;
    [SerializeField]
    StatAttribute _loweredAttribute;

    const float DIFFERENCE_IN_STAT_MODIFIER = 0.1f;

    public string natureName
    {
        get { return this.name; }
    }

    public StatAttribute raisedAttribute
    {
        get { return _raisedAttribute; }
        set
        {
            _raisedAttribute = value;
        }
    }

    public StatAttribute loweredAttribute
    {
        get { return _loweredAttribute; }
        set
        {
            _loweredAttribute = value;
        }
    }

    public float NatureModifier(NatureBase currentNatureBase,StatAttribute currentStatAttribute)
    {
        float defaultValue = 1;

        if(currentNatureBase.raisedAttribute == currentStatAttribute)
        {
            defaultValue += DIFFERENCE_IN_STAT_MODIFIER;
        }
        else if(currentNatureBase.loweredAttribute == currentStatAttribute)
        {
            defaultValue -= DIFFERENCE_IN_STAT_MODIFIER;
        }

        return defaultValue;
    }

    Flavor FavoriteFlavor
    {
        get
        {
            switch (raisedAttribute)
            {
                case StatAttribute.Attack:
                    return Flavor.Spicy;
                case StatAttribute.Defense:
                    return Flavor.Sour;
                case StatAttribute.SpecialAttack:
                    return Flavor.Dry;
                case StatAttribute.SpecialDefense:
                    return Flavor.Bitter;
                default://speed
                    return Flavor.Sweet;
            }
        }
    }

    Flavor HatedFlavor
    {
        get
        {
            switch (raisedAttribute)
            {
                case StatAttribute.Attack:
                    return Flavor.Spicy;
                case StatAttribute.Defense:
                    return Flavor.Sour;
                case StatAttribute.SpecialAttack:
                    return Flavor.Dry;
                case StatAttribute.SpecialDefense:
                    return Flavor.Bitter;
                default://speed
                    return Flavor.Sweet;
            }
        }
    }

    public int GetFlavourRating(Flavor flavor)
    {
        if(raisedAttribute == loweredAttribute)
        {
            return 0;
        }
        else if(flavor == FavoriteFlavor)
        {
            return 1;
        }
        else
        {
            return -1;
        }
    }
}
