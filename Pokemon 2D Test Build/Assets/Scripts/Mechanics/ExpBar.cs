using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExpBar : MonoBehaviour
{
    Image _expBar;

    void Awake()
    {
        _expBar = GetComponent<Image>();
    }

    public void SetExpereince(Pokemon pokemon)
    {
        if(pokemon.currentLevel >= 100)
        {
            _expBar.fillAmount = 1;
        }
        _expBar.fillAmount = ExpNormalized(pokemon);
    }

    void SetExpereince(float expNoramlized)
    {
        _expBar.fillAmount = expNoramlized;
    }

    float ExpNormalized(Pokemon currentPokemon)
    {
        int currentLevelExp = currentPokemon.pokemonBase.GetExpForLevel(currentPokemon.currentLevel);
        int nextLevelExp = currentPokemon.pokemonBase.GetExpForLevel(currentPokemon.currentLevel+1);

        float expNormalized = (float)(currentPokemon.currentExp - currentLevelExp) / (nextLevelExp - currentLevelExp);
        return Mathf.Clamp01(expNormalized);
    }

    public IEnumerator SetExpAnimation(int expGained, int previousEXP,int exptoCurrentLevel, int expToNextLevel)
    {
        float curExp = previousEXP-exptoCurrentLevel;
        float finalExp = curExp + expGained;

        float animationTime = 1.5f;
        float timer = 0;

        float expAnim;
        while (timer < animationTime)
        {
            timer += Time.deltaTime;
            expAnim = curExp + (expGained * (timer / animationTime));
            SetExpereince(expAnim / (expToNextLevel - exptoCurrentLevel));

            //If it grows a level
            if (expAnim / (expToNextLevel - exptoCurrentLevel) > 1)
            {
                SetExpereince(1);
                yield break;
            }

            yield return null;
        }

        SetExpereince((float)(finalExp / (expToNextLevel - exptoCurrentLevel)));
    }

    public Image expBarImage
    {
        get { return _expBar; }
    }
}
