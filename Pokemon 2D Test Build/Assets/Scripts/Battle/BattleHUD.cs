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

    public void SetData(Pokemon currentPokemon,bool isEnemy)
    {
        _nameText.text = currentPokemon.currentName;
        _levelText.text = currentPokemon.currentLevel.ToString();
        _hPBar.SetHP((float)currentPokemon.currentHitPoints / currentPokemon.maxHitPoints);
        if(isEnemy == false)
        {
            _currentHP.text = currentPokemon.currentHitPoints.ToString();
            _maxHP.text = currentPokemon.maxHitPoints.ToString();
        }
    }

    
}
