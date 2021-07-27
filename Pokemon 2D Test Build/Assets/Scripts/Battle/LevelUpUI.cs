using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public struct StandardStats
{
    public List<int> stats { get; private set; }

    public StandardStats(int hp,int att,int def,int spAtt,int spDef,int spd)
    {
        stats = new List<int>();
        stats.Add(hp);
        stats.Add(att);
        stats.Add(def);
        stats.Add(spAtt);
        stats.Add(spDef);
        stats.Add(spd);
    }
}

public class LevelUpUI : MonoBehaviour
{
    [SerializeField] GameObject levelUpUI;
    [SerializeField] Text[] statPlus;
    [SerializeField] Text[] statNumber;

    [SerializeField] GameObject alternativePokemon;
    [SerializeField] Image pokemonSprite;
    [SerializeField] Text pokemonName;
    [SerializeField] Text pokemonLevel;
    [SerializeField] Image pokemonGender;

    bool _waitingOnUserInput = false;

    public void HandleStart()
    {
        levelUpUI.SetActive(false);
        alternativePokemon.SetActive(false);
    }

    public IEnumerator DisplayLevelUp(StandardStats StatsBeforeLevelUp,StandardStats StatsAfterLevelUp,Pokemon pokemon = null)
    {
        _waitingOnUserInput = true;

        List<int> diffInStats = new List<int>();

        for (int i = 0; i < StatsAfterLevelUp.stats.Count; i++)
        {
            SetStat(i, StatsAfterLevelUp.stats[i] - StatsBeforeLevelUp.stats[i],true);
        }

        if(pokemon != null)
        {
            SetDataAlternativePokemon(pokemon);
            alternativePokemon.SetActive(true);
        }

        levelUpUI.SetActive(true);

        while(_waitingOnUserInput == true)
        {
            if (Input.anyKeyDown)
            {
                _waitingOnUserInput = false;
            }
            yield return null;
        }

        _waitingOnUserInput = true;

        for (int i = 0; i < StatsAfterLevelUp.stats.Count; i++)
        {
            SetStat(i, StatsAfterLevelUp.stats[i]);
        }

        while (_waitingOnUserInput == true)
        {
            if (Input.anyKeyDown)
            {
                _waitingOnUserInput = false;
            }
            yield return null;
        }

        levelUpUI.SetActive(false);
        alternativePokemon.SetActive(false);
    }

    void SetStat(int Pos,int dif,bool showingDif = false)
    {
        if (dif > 0 && showingDif == true)
        {
            statPlus[Pos].text = "+";
        }
        else
        {
            statPlus[Pos].text = "";
        }
        statNumber[Pos].text = $"{dif}";
    }

    void SetDataAlternativePokemon(Pokemon pokemon)
    {
        pokemonSprite.sprite = pokemon.pokemonBase.GetAnimatedSprites()[0];
        pokemonName.text = pokemon.currentName;
        pokemonLevel.text = pokemon.currentLevel.ToString();
        pokemonGender.sprite = StatusConditionArt.instance.ReturnGenderArt(pokemon.gender);
    }
}
