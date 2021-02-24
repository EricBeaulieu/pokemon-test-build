using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ElementType { NA, Bug, Dark, Dragon, Electric, Fairy, Fighting, Fire, Flying, Ghost,
                          Grass, Ground, Ice, Normal, Poison, Psychic, Rock, Steel, Water}

[CreateAssetMenu(menuName = "Pokedex/Create New Pokemon Entry")]
public class PokemonBase : ScriptableObject {

    [Header("PokeDex Information")]
    [SerializeField]
    int _pokedexNumber;
    [SerializeField]
    string _pokedexName;
    [TextArea]
    [SerializeField]
    string _pokedexDescription;
    [SerializeField]
    string _classification;
    [SerializeField]
    float _heightInFeet;
    [SerializeField]
    float _heightInMeters;
    [SerializeField]
    float _weightInPounds;
    [SerializeField]
    float _weightInKilograms;
    [SerializeField]
    int _captureRate;
    [SerializeField]
    List<EarnableEV> _rewardedEffortValue;

    [Header("Sprites")]
    [SerializeField]
    Sprite _frontRegularSprite;
    [SerializeField]
    Sprite _frontIntroSprite;
    [SerializeField]
    Sprite _backRegularSprite;
    [SerializeField]
    Sprite _backIntroSprite;
    [SerializeField]
    Sprite _shinyFrontRegularSprite;
    [SerializeField]
    Sprite _shinyFrontIntroSprite;
    [SerializeField]
    Sprite _shinyBackSprite;
    [SerializeField]
    Sprite _shinyBackIntroSprite;

    [Header("Experience Group")]
    [SerializeField]
    ExperienceGroup _baseGroup;

    [Header("PokemonTypes")]
    [SerializeField]
    ElementType _type1;
    [SerializeField]
    ElementType _type2;

    [Header("Egg Information")]
    [SerializeField]
    EggGroup _eggGroup1;
    [SerializeField]
    EggGroup _eggGroup2;

    [Header("Base Stats")]
    [SerializeField]
    int _maxHitPoints;
    [SerializeField]
    int _attack;
    [SerializeField]
    int _defense;
    [SerializeField]
    int _specialAttack;
    [SerializeField]
    int _specialDefense;
    [SerializeField]
    int _speed;

    [Header("Moveset")]
    [SerializeField] List<LearnableMove> _learnableMoves;

    PokemonBase()
    {
        if(_eggGroup1 == EggGroup.BreedingSelection)
        {
            Debug.LogWarning("First Egg group not set", this);
        }
        
        if(_type1 == ElementType.NA)
        {
            Debug.LogWarning("First type not set", this);
        }
    }

    #region Getters/Setters

    public string GetPokedexName()
    {
        return _pokedexName;
    }

    public Sprite[] GetFrontSprite(bool isShiny)
    {
        if(isShiny == false)
        {
            return new[] { _frontRegularSprite, _frontIntroSprite };
        }
        else
        {
            return new[] { _shinyFrontRegularSprite, _shinyFrontIntroSprite };
        }
    }

    public Sprite[] GetBackSprite(bool isShiny)
    {
        if (isShiny == false)
        {
            return new[] { _backRegularSprite, _backIntroSprite };
        }
        else
        {
            return new[] { _shinyBackIntroSprite, _shinyBackIntroSprite };
        }
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
        get { return _rewardedEffortValue; }
    }

    public List<LearnableMove> LearnableMoves
    {
        get { return _learnableMoves; }
    }

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
