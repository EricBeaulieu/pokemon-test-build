using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleHUD : MonoBehaviour
{
    Vector3 _originalPos;

    [SerializeField] Image hudBackground;
    [SerializeField] Text nameText;
    [SerializeField] Text levelText;
    [SerializeField] Image levelImageText;
    [SerializeField] Image statusCondition;
    [SerializeField] HPBar hPBar;
    [SerializeField] Image healthBarBackground;
    [SerializeField] Image healthBarFrame;
    [SerializeField] Image healthBarInsideBackground;
    [SerializeField] Image healthBarImageText;
    [SerializeField] Image gender;

    [Header("PlayerSpecific")]
    [SerializeField] Text currentHP;
    [SerializeField] ExpBar expBar;
    [SerializeField] Image experienceBarBackground;

    [SerializeField] Image pointerBody;
    [SerializeField] Image pointerHead;

    Pokemon pokemon;

    void Awake()
    {
        _originalPos = hudBackground.rectTransform.localPosition;
    }

    public void SetData(Pokemon currentPokemon,bool isPlayersPokemon)
    {
        pokemon = currentPokemon;
        nameText.text = currentPokemon.currentName;
        SetLevel();
        hPBar.SetHPWithoutAnimation(currentPokemon.currentHitPoints,currentPokemon.maxHitPoints);

        if(isPlayersPokemon == true)
        {
            currentHP.text = $"{currentPokemon.currentHitPoints.ToString()} / {currentPokemon.maxHitPoints.ToString()}";
            expBar.SetExpereince(currentPokemon);
        }

        SetStatusSprite();
        pokemon.OnStatusChanged += SetStatusSprite;

        gender.sprite = StatusConditionArt.instance.ReturnGenderArt(currentPokemon.gender);

        SetHudOffScreen(isPlayersPokemon);
        ResetAlphaHUD(isPlayersPokemon);
    }

    void SetStatusSprite()
    {
        if(pokemon.status == null)
        {
            statusCondition.sprite = StatusConditionArt.instance.ReturnStatusConditionArt(ConditionID.NA);
        }
        else
        {
            statusCondition.sprite = StatusConditionArt.instance.ReturnStatusConditionArt(pokemon.status.Id);
        }
    }

    public void SetLevel()
    {
        levelText.text = pokemon.currentLevel.ToString();
    }

    public IEnumerator UpdateHP(int hpBeforeChange)
    {
        if (hpBeforeChange == pokemon.currentHitPoints)
        {
            yield break;
        }
        else if (hpBeforeChange > pokemon.currentHitPoints)
        {
            yield return hPBar.SetHPDamageAnimation(pokemon.currentHitPoints, hpBeforeChange, pokemon.maxHitPoints, currentHP);
        }
        else if (hpBeforeChange < pokemon.currentHitPoints)
        {
            yield return hPBar.SetHPRecoveredAnimation(pokemon.currentHitPoints,pokemon.currentHitPoints - hpBeforeChange, pokemon.maxHitPoints, currentHP);
        }
    }

    public void UpdateHPWithoutAnimation()
    {
        hPBar.SetHPWithoutAnimation(pokemon.currentHitPoints,pokemon.maxHitPoints,currentHP);
    }

    public IEnumerator GainExpAnimation(int expGained, int expBeforeAnim)
    {
        yield return expBar.SetExpAnimation(expGained,expBeforeAnim, pokemon.pokemonBase.GetExpForLevel(pokemon.currentLevel), pokemon.pokemonBase.GetExpForLevel(pokemon.currentLevel+1));
    }

    public IEnumerator FaintedPokemonHUDAnimation(bool isPlayers)
    {
        float tempAlpha = 1;
        float animationTime = 1.5f;

        while (tempAlpha > 0)
        {
            tempAlpha -= (0.01f * animationTime);

            SetAllHudAlphas(tempAlpha,isPlayers);
            yield return new WaitForSeconds(0.01f);
        }
        pokemon.Reset();
    }

    void SetHudOffScreen(bool isPlayers)
    {
        if (isPlayers == true)
        {
            transform.localPosition = new Vector3(_originalPos.x + 400f, _originalPos.y);
        }
        else
        {
            transform.localPosition = new Vector3(_originalPos.x - 400f, _originalPos.y);
        }
    }

    void ResetAlphaHUD(bool isPlayers)
    {
        SetAllHudAlphas(1,isPlayers);
    }

    public IEnumerator PlayEnterAnimation(float duration)
    {
        yield return SmoothTransitionToPosition(_originalPos, duration);
    }

    IEnumerator SmoothTransitionToPosition(Vector3 endPos, float duration)
    {
        Transform tempTrans = transform;

        Vector3 startingPos = transform.localPosition;
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            tempTrans.localPosition = Vector3.Lerp(startingPos, endPos, (elapsedTime / duration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        tempTrans.localPosition = _originalPos;
    }

    void SetAllHudAlphas(float tempAlpha,bool isPlayers)
    {
        hudBackground.SetAlpha(tempAlpha);
        nameText.SetAlpha(tempAlpha);
        levelText.SetAlpha(tempAlpha);
        levelImageText.SetAlpha(tempAlpha);
        statusCondition.SetAlpha(tempAlpha);
        gender.SetAlpha(tempAlpha);
        healthBarBackground.SetAlpha(tempAlpha);
        healthBarFrame.SetAlpha(tempAlpha);
        healthBarImageText.SetAlpha(tempAlpha);
        healthBarInsideBackground.SetAlpha(tempAlpha);
        hPBar.healthBarImage.SetAlpha(tempAlpha);

        if (isPlayers == true)
        {
            currentHP.SetAlpha(tempAlpha);
            experienceBarBackground.SetAlpha(tempAlpha);
            expBar.expBarImage.SetAlpha(tempAlpha);
        }

        pointerBody.SetAlpha(tempAlpha);
        pointerHead.SetAlpha(tempAlpha);
    }

    /// <summary>
    /// called from battle unit, it passes if its the players pokemon as well ontop of it,
    /// this is due to some of the enemy stats being null
    /// </summary>
    public void PrecautionsCheck(bool isPlayersHUD)
    {
        if (nameText == null)
        {
            Debug.LogWarning($"nameText has not been set",gameObject);
        }
        if (levelText == null)
        {
            Debug.LogWarning($"levelText has not been set", gameObject);
        }
        if (levelImageText == null)
        {
            Debug.LogWarning($"levelImageText has not been set", gameObject);
        }
        if (statusCondition == null)
        {
            Debug.LogWarning($"statusCondition has not been set", gameObject);
        }
        if (hPBar == null)
        {
            Debug.LogWarning($"hPBar has not been set", gameObject);
        }
        if (healthBarBackground == null)
        {
            Debug.LogWarning($"healthBarBackground has not been set", gameObject);
        }
        if (healthBarFrame == null)
        {
            Debug.LogWarning($"healthBarFrame has not been set", gameObject);
        }
        if (healthBarImageText == null)
        {
            Debug.LogWarning($"healthBarImageText has not been set", gameObject);
        }
        if (gender == null)
        {
            Debug.LogWarning($"gender has not been set", gameObject);
        }

        if(isPlayersHUD == true)
        {
            if (currentHP == null)
            {
                Debug.LogWarning($"currentHP has not been set", gameObject);
            }
            if (expBar == null)
            {
                Debug.LogWarning($"expBar has not been set", gameObject);
            }
            if (experienceBarBackground == null)
            {
                Debug.LogWarning($"experienceBarBackground has not been set", gameObject);
            }
        }

        if (pointerBody == null)
        {
            Debug.LogWarning($"pointerBody has not been set", gameObject);
        }
        if (pointerHead == null)
        {
            Debug.LogWarning($"pointerHead has not been set", gameObject);
        }
    }

    public void UpdateHud()
    {
        hPBar.SetHPWithoutAnimation(pokemon.currentHitPoints, pokemon.maxHitPoints, currentHP);
        SetStatusSprite();
    }
}
