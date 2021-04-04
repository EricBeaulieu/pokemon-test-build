using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ElementType { NA = -1, Bug, Dark, Dragon, Electric, Fairy, Fighting, Fire, Flying, Ghost,
                          Grass, Ground, Ice, Normal, Poison, Psychic, Rock, Steel, Water}

public enum StatAttribute { NA, HitPoints, Attack, Defense, SpecialAttack, SpecialDefense, Speed, Evasion, Accuracy }

public enum Gender { NA,Male,Female}

[CreateAssetMenu(menuName = "Pokedex/Create New Pokemon Entry")]
public class PokemonBase : ScriptableObject {

    [Header("PokeDex Information")]
    [SerializeField] int _pokedexNumber;
    [SerializeField] string _pokedexName;
    [TextArea]
    [SerializeField] string _pokedexDescription;
    [SerializeField] string _classification;
    [SerializeField] float _heightInFeet;
    [SerializeField] float _heightInMeters;
    [SerializeField] float _weightInPounds;
    [SerializeField] float _weightInKilograms;
    [SerializeField] int _captureRate;
    [SerializeField] List<EarnableEV> rewardedEffortValue;

    [Header("Sprites")]
    [SerializeField] Sprite frontRegularSprite;
    [SerializeField] Sprite frontIntroSprite;
    [SerializeField] Sprite backRegularSprite;
    [SerializeField] Sprite backIntroSprite;
    [SerializeField] Sprite shinyFrontRegularSprite;
    [SerializeField] Sprite shinyFrontIntroSprite;
    [SerializeField] Sprite shinyBackRegularSprite;
    [SerializeField] Sprite shinyBackIntroSprite;
    [SerializeField] Sprite standardSpriteA;
    [SerializeField] Sprite standardSpriteB;
    [SerializeField] bool differentGenderSprites;
    [SerializeField] Sprite femaleFrontRegularSprite;
    [SerializeField] Sprite femaleFrontIntroSprite;
    [SerializeField] Sprite femaleBackRegularSprite;
    [SerializeField] Sprite femaleBackIntroSprite;
    [SerializeField] Sprite femaleShinyFrontRegularSprite;
    [SerializeField] Sprite femaleShinyFrontIntroSprite;
    [SerializeField] Sprite femaleShinyBackRegularSprite;
    [SerializeField] Sprite femaleShinyBackIntroSprite;

    [Header("Gender")]
    [SerializeField] bool _hasGender = true;
    [Range(0, 100)]
    [SerializeField] float _maleFemaleRatio = 50f;

    [Header("Experience Group")]
    [SerializeField] ExperienceGroup _baseGroup;
    [SerializeField] int rewardedBaseExp;

    [Header("PokemonTypes")]
    [SerializeField] ElementType _type1;
    [SerializeField] ElementType _type2;

    [Header("Egg Information")]
    [SerializeField] EggGroup _eggGroup1;
    [SerializeField] EggGroup _eggGroup2;

    [Header("Base Stats")]
    [SerializeField] int _maxHitPoints;
    [SerializeField] int _attack;
    [SerializeField] int _defense;
    [SerializeField] int _specialAttack;
    [SerializeField] int _specialDefense;
    [SerializeField] int _speed;

    [Header("Abilities")]
    [SerializeField] AbilityID firstAbility;
    [SerializeField] AbilityID secondAbility;
    [SerializeField] AbilityID hiddenAbility;

    [Header("Moveset")]
    [SerializeField] List<LearnableMove> _learnableMoves;
    public static int MAX_NUMBER_OF_MOVES { get; } = 4;

    //Editor Additions

    #region Getters/Setters

    public string GetPokedexName()
    {
        return _pokedexName;
    }

    public int GetCatchRate()
    {
        return _captureRate;
    }

    public Sprite[] GetFrontSprite(bool isShiny,Gender gender)
    {
        if(differentGenderSprites == true)
        {
            if (gender == Gender.Female)
            {
                if (isShiny == false)
                {
                    return new[] { femaleFrontRegularSprite, femaleFrontIntroSprite };
                }
                else
                {
                    return new[] { femaleShinyFrontRegularSprite, femaleShinyFrontIntroSprite };
                }
            }
        }

        //If no specialised gender
        if (isShiny == false)
        {
            return new[] { frontRegularSprite, frontIntroSprite };
        }
        else
        {
            return new[] { shinyFrontRegularSprite, shinyFrontIntroSprite };
        }
    }

    public Sprite[] GetBackSprite(bool isShiny, Gender gender)
    {
        if (differentGenderSprites == true)
        {
            if (gender == Gender.Female)
            {
                if (isShiny == false)
                {
                    return new[] { femaleBackRegularSprite, femaleBackIntroSprite };
                }
                else
                {
                    return new[] { femaleShinyBackRegularSprite, femaleShinyBackIntroSprite };
                }
            }
        }

        //If no specialised gender
        if (isShiny == false)
        {
            return new[] { backRegularSprite, backIntroSprite };
        }
        else
        {
            return new[] { shinyBackRegularSprite, shinyBackIntroSprite };
        }
    }

    /// <summary>
    /// gets both animations of the sprite available
    /// </summary>
    /// <returns>returns an array of the animated sprite</returns>
    public Sprite[] GetAnimatedSprites()
    {
        return new[] { standardSpriteA, standardSpriteB };
    }

    #endregion

    #region Gender

    public bool HasGender
    {
        get { return _hasGender; }
    }

    public float MaleFemaleGenderRatio
    {
        get { return _maleFemaleRatio; }
    }

    #endregion

    #region Return Pokemon Type

    public ElementType pokemonType1
    {
        get { return _type1; }
    }

    public ElementType pokemonType2
    {
        get { return _type2; }
    }

    public bool IsType(ElementType elementType)
    {
        if(elementType == _type1 || elementType == _type2)
        {
            return true;
        }
        return false;
    }

    #endregion

    #region Return Base Stats

    public int maxHitPoints
    {
        get { return _maxHitPoints; }
    }

    public int attack
    {
        get { return _attack;  }
    }

    public int defense
    {
        get { return _defense; }
    }

    public int specialAttack
    {
        get { return _specialAttack; }
    }
    public int specialDefense
    {
        get { return _specialDefense; }
    }
    public int speed
    {
        get { return _speed; }
    }

    #endregion

    public List<EarnableEV> rewardedEfforValue
    {
        get { return rewardedEffortValue; }
    }

    public int GetExpForLevel(int level)
    {
        return ExperienceTable.ReturnExperienceRequiredForLevel(level, _baseGroup);
    }

    public int RewardedExperienceYield
    {
        get { return rewardedBaseExp; }
    }

    public List<LearnableMove> LearnableMoves
    {
        get { return _learnableMoves; }
    }

    public AbilityID FirstAbility
    {
        get { return firstAbility; }
    }
    public AbilityID SecondAbility
    {
        get { return secondAbility; }
    }
    public AbilityID HiddenAbility
    {
        get { return hiddenAbility; }
    }

    #region EditorFunctions

    //public bool DifferentGenderSprites
    //{
    //    get { return differentGenderSprites; }
    //    set
    //    {
    //        differentGenderSprites = value;
    //    }
    //}

    //public Sprite FemaleFrontRegularSprite
    //{
    //    get { return _femaleFrontRegularSprite; }
    //    set
    //    {
    //        _femaleFrontRegularSprite = value;
    //    }
    //}

    //public Sprite FemaleFrontIntroSprite
    //{
    //    get { return _femaleFrontIntroSprite; }
    //    set
    //    {
    //        _femaleFrontIntroSprite = value;
    //    }
    //}

    //public Sprite FemaleBackRegularSprite
    //{
    //    get { return _femaleBackRegularSprite; }
    //    set
    //    {
    //        _femaleBackRegularSprite = value;
    //    }
    //}

    //public Sprite FemaleBackIntroSprite
    //{
    //    get { return _femaleBackIntroSprite; }
    //    set
    //    {
    //        _femaleBackIntroSprite = value;
    //    }
    //}

    //public Sprite FemaleShinyFrontRegularSprite
    //{
    //    get { return _femaleShinyFrontRegularSprite; }
    //    set
    //    {
    //        _femaleShinyFrontRegularSprite = value;
    //    }
    //}

    //public Sprite FemaleShinyFrontIntroSprite
    //{
    //    get { return _femaleShinyFrontIntroSprite; }
    //    set
    //    {
    //        _femaleShinyFrontIntroSprite = value;
    //    }
    //}

    //public Sprite FemaleShinyBackRegularSprite
    //{
    //    get { return _femaleShinyBackRegularSprite; }
    //    set
    //    {
    //        _femaleShinyBackRegularSprite = value;
    //    }
    //}

    //public Sprite FemaleShinyBackIntroSprite
    //{
    //    get { return _femaleShinyBackIntroSprite; }
    //    set
    //    {
    //        _femaleShinyBackIntroSprite = value;
    //    }
    //}

    #endregion

    //void CheckPokemonEVs()
    //{
    //    int effortValueTotal = 0;
    //    foreach (EarnableEV ev in _rewardedEffortValue)
    //    {
    //        effortValueTotal += ev.statValue;
    //    }
    //    if (effortValueTotal > 3)
    //    {
    //        Debug.LogError("MaxAmount of Effort Values given Exceeds Given Amount", this);
    //    }
    //}

}
