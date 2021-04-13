using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SummarySkills : MonoBehaviour
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

    public void SetupData(Pokemon pokemon)
    {

        //pokeballSprite.sprite =
    }
}
