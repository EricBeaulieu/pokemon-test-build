using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum ElementType { NA = -1, Bug, Dark, Dragon, Electric, Fairy, Fighting, Fire, Flying, Ghost,
                          Grass, Ground, Ice, Normal, Poison, Psychic, Rock, Steel, Water}

public enum StatAttribute { NA, HitPoints, Attack, Defense, SpecialAttack, SpecialDefense, Speed, Evasion, Accuracy, CriticalHitRatio }

public enum Gender { NA,Male,Female}

[CreateAssetMenu(menuName = "Pokedex/Create New Pokemon Entry")]
public class PokemonBase : ScriptableObject {

    [Header("PokeDex Information")]
    [SerializeField] int _pokedexNumber;
    [TextArea]
    [SerializeField] string _pokedexDescription;
    [SerializeField] string _classification;
    [SerializeField] float _heightInMeters;
    [SerializeField] float _weightInPounds;
    [SerializeField] int _captureRate;
    [SerializeField] List<EarnableEV> rewardedEffortValue;

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
    [SerializeField] List<MoveBase> learnableTMHMMoves;
    public static int MAX_NUMBER_OF_MOVES { get; } = 4;

    [Header("Evolving")]
    [SerializeField] List<EvolveLevelBased> evolveLevelBased;
    [SerializeField] List<EvolutionStone> evolutionsByStone;
    List<EvolutionBase> evolutions;

    [Header("Wild")]
    [SerializeField] List<WildPokemonHoldItems> wildPokemonHoldItems;

    #region Getters/Setters

    public int GetPokedexNumber()
    {
        return _pokedexNumber;
    }

    public string GetPokedexName()
    {
        return PokemonNameList.GetPokeDexName(_pokedexNumber);
    }

    public int GetCatchRate()
    {
        return _captureRate;
    }

    public Sprite[] GetFrontSprite(bool isShiny,Gender gender)
    {
        string spriteName = GetStartingSpriteNameEntry(isShiny, gender);

        return new[] { SpriteAtlas.GetSprite(spriteName + "FrontA"), SpriteAtlas.GetSprite(spriteName + "FrontB") };
    }

    public Sprite[] GetBackSprite(bool isShiny, Gender gender)
    {
        string spriteName = GetStartingSpriteNameEntry(isShiny, gender);

        return new[] { SpriteAtlas.GetSprite(spriteName + "BackA"), SpriteAtlas.GetSprite(spriteName + "BackB") };
    }

    /// <summary>
    /// gets both animations of the sprite available
    /// </summary>
    /// <returns>returns an array of the animated sprite</returns>
    public Sprite[] GetAnimatedSprites()
    {
        string spriteName = GetStartingSpriteNameEntry(false);

        return new[] { SpriteAtlas.GetSprite(spriteName + "SpriteA"), SpriteAtlas.GetSprite(spriteName + "SpriteB") };
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

    public List<MoveBase> LearnableTMHMMoves
    {
        get { return learnableTMHMMoves; }
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

    string GetStartingSpriteNameEntry(bool isShiny,Gender gender = Gender.NA)
    {
        string spriteName = $"{GetPokedexNumber().ToString("000")}_{PokemonNameList.GetPokeDexName(_pokedexNumber)}_";

        if (isShiny == true)
        {
            spriteName += "Shiny_";
        }

        if (PokemonNameList.GenderExclusive(GetPokedexNumber()) == true)
        {
            if (gender == Gender.Male)
            {
                spriteName += "Male_";
            }
            else if(gender == Gender.Female)
            {
                spriteName += "Female_";
            }
        }

        return spriteName;
    }


    public List<EvolveLevelBased> EvolveLevelBased
    {
        get { return evolveLevelBased; }
    }

    public List<EvolutionStone> EvolutionsByStone
    {
        get { return evolutionsByStone; }
    }

    public List<WildPokemonHoldItems> WildPokemonHoldItems
    {
        get { return wildPokemonHoldItems; }
    }

}
