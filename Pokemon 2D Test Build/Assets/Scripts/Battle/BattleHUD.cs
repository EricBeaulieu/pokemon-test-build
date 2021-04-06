using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleHUD : MonoBehaviour
{
    Vector3 _originalPos;

    Image hudBackground;
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

    Pokemon _pokemon;

    void Awake()
    {
        hudBackground = GetComponent<Image>();
        _originalPos = hudBackground.rectTransform.localPosition;
    }

    public void SetData(Pokemon currentPokemon,bool isPlayersPokemon)
    {
        _pokemon = currentPokemon;
        nameText.text = currentPokemon.currentName;
        SetLevel();
        hPBar.SetHPWithoutAnimation(currentPokemon.currentHitPoints,currentPokemon.maxHitPoints);

        if(isPlayersPokemon == true)
        {
            currentHP.text = $"{currentPokemon.currentHitPoints.ToString()} / {currentPokemon.maxHitPoints.ToString()}";
            expBar.SetExpereince(currentPokemon);
        }

        SetStatusSprite();
        _pokemon.OnStatusChanged += SetStatusSprite;

        gender.sprite = StatusConditionArt.instance.ReturnGenderArt(currentPokemon.gender);

        SetHudOffScreen(isPlayersPokemon);
        ResetAlphaHUD(isPlayersPokemon);
    }

    void SetStatusSprite()
    {
        if(_pokemon.status == null)
        {
            statusCondition.sprite = StatusConditionArt.instance.ReturnStatusConditionArt(ConditionID.NA);
        }
        else
        {
            statusCondition.sprite = StatusConditionArt.instance.ReturnStatusConditionArt(_pokemon.status.Id);
        }
    }

    public void SetLevel()
    {
        levelText.text = _pokemon.currentLevel.ToString();
    }

    public IEnumerator UpdateHP(int hpBeforeDamage)
    {
        yield return hPBar.SetHPAnimation(_pokemon.currentHitPoints,hpBeforeDamage,_pokemon.maxHitPoints,currentHP);
    }

    public void UpdateHPWithoutAnimation()
    {
        hPBar.SetHPWithoutAnimation(_pokemon.currentHitPoints,_pokemon.maxHitPoints,currentHP);
    }

    public IEnumerator GainExpAnimation(int expGained, int expBeforeAnim)
    {
        yield return expBar.SetExpAnimation(expGained,expBeforeAnim, _pokemon.pokemonBase.GetExpForLevel(_pokemon.currentLevel), _pokemon.pokemonBase.GetExpForLevel(_pokemon.currentLevel+1));
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
        hudBackground.color = hudBackground.color.SetAlpha(tempAlpha);
        nameText.color = nameText.color.SetAlpha(tempAlpha);
        levelText.color = levelText.color.SetAlpha(tempAlpha);
        levelImageText.color = levelImageText.color.SetAlpha(tempAlpha);
        statusCondition.color = statusCondition.color.SetAlpha(tempAlpha);
        gender.color = gender.color.SetAlpha(tempAlpha);
        healthBarBackground.color = healthBarBackground.color.SetAlpha(tempAlpha);
        healthBarFrame.color = healthBarFrame.color.SetAlpha(tempAlpha);
        healthBarImageText.color = healthBarImageText.color.SetAlpha(tempAlpha);
        healthBarInsideBackground.color = healthBarInsideBackground.color.SetAlpha(tempAlpha);
        hPBar.healthBarImage.color = hPBar.healthBarImage.color.SetAlpha(tempAlpha);

        if (isPlayers == true)
        {
            currentHP.color = currentHP.color.SetAlpha(tempAlpha);
            experienceBarBackground.color = experienceBarBackground.color.SetAlpha(tempAlpha);
            expBar.expBarImage.color = expBar.expBarImage.color.SetAlpha(tempAlpha);
        }

        pointerBody.color = pointerBody.color.SetAlpha(tempAlpha);
        pointerHead.color = pointerHead.color.SetAlpha(tempAlpha);
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
}
