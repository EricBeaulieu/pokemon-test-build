using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleHUD : MonoBehaviour
{
    [SerializeField] Text nameText;
    [SerializeField] Text levelText;
    [SerializeField] HPBar hPBar;

    //This is only here due to the background sprite not being cut right, naturally it would be cleaned up
    [SerializeField] Text currentHP;
    [SerializeField] Text maxHP;

    [SerializeField] HPBar experienceBar;

    Pokemon _pokemon;

    public void SetData(Pokemon currentPokemon,bool isEnemy)
    {
        _pokemon = currentPokemon;
        nameText.text = currentPokemon.currentName;
        levelText.text = currentPokemon.currentLevel.ToString();
        hPBar.SetHP((float)currentPokemon.currentHitPoints / currentPokemon.maxHitPoints);
        if(isEnemy == false)
        {
            currentHP.text = currentPokemon.currentHitPoints.ToString();
            maxHP.text = currentPokemon.maxHitPoints.ToString();
        }
    }

    public IEnumerator UpdateHP(int hpBeforeDamage)
    {
        yield return hPBar.SetHPAnimation(_pokemon.currentHitPoints,hpBeforeDamage,_pokemon.maxHitPoints,currentHP);
    }

    
}
