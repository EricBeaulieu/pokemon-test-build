using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleHUD : MonoBehaviour
{
    [SerializeField]
    Text _nameText;
    [SerializeField]
    Text _levelText;
    [SerializeField]
    HPBar _hPBar;

    //This is only here due to the background sprite not being cut right, naturally it would be cleaned up
    [SerializeField]
    Text _currentHP;
    [SerializeField]
    Text _maxHP;

    [SerializeField]
    HPBar _experienceBar;

    Pokemon _pokemon;

    public void SetData(Pokemon currentPokemon,bool isEnemy)
    {
        _pokemon = currentPokemon;
        _nameText.text = currentPokemon.currentName;
        _levelText.text = currentPokemon.currentLevel.ToString();
        _hPBar.SetHP((float)currentPokemon.currentHitPoints / currentPokemon.maxHitPoints);
        if(isEnemy == false)
        {
            _currentHP.text = currentPokemon.currentHitPoints.ToString();
            _maxHP.text = currentPokemon.maxHitPoints.ToString();
        }
    }

    public IEnumerator UpdateHP(int hpBeforeDamage)
    {
        yield return _hPBar.SetHPAnimation(_pokemon.currentHitPoints,hpBeforeDamage,_pokemon.maxHitPoints,_currentHP);
    }

    
}
