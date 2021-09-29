using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SummarySkills : SummaryUIBase
{
    [SerializeField] Text hitPoints;
    [SerializeField] HPBar hPBar;
    [SerializeField] Text attack;
    [SerializeField] Text defense;
    [SerializeField] Text specialAttack;
    [SerializeField] Text specialDefense;
    [SerializeField] Text speed;

    [SerializeField] Text totalExperience;
    [SerializeField] Text experienceRequiredToNextLevel;
    [SerializeField] ExpBar expBar;

    [SerializeField] Text abilityName;
    [SerializeField] Text abilityDescription;

    public override float offsetXPosDifference()
    {
        return GetComponent<RectTransform>().sizeDelta.x;
    }

    public override void SetupData(Pokemon pokemon)
    {
        hPBar.SetHPWithoutAnimation(pokemon.currentHitPoints, pokemon.maxHitPoints, hitPoints);
        attack.text = $"{pokemon.baseStats[StatAttribute.Attack]}";
        defense.text = $"{pokemon.baseStats[StatAttribute.Defense]}";
        specialAttack.text = $"{pokemon.baseStats[StatAttribute.SpecialAttack]}";
        specialDefense.text = $"{pokemon.baseStats[StatAttribute.SpecialDefense]}";
        speed.text = $"{pokemon.baseStats[StatAttribute.Speed]}";

        totalExperience.text = $"{pokemon.currentExp}";
        experienceRequiredToNextLevel.text = $"{pokemon.pokemonBase.GetExpForLevel(pokemon.currentLevel+1) - pokemon.currentExp}";
        expBar.SetExpereince(pokemon);

        if(pokemon.ability != null)
        {
            abilityName.text = pokemon.ability.Name;
            abilityDescription.text = pokemon.ability.Description();
        }
        else
        {
            abilityName.text = "ABILITY NOT SET";
            abilityDescription.text = "ABILITY NOT SET";
        }

    }

}
