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
    [SerializeField] Image statusCondition;
    [SerializeField] HPBar hPBar;
    [SerializeField] Image healthBarBackground;
    [SerializeField] Image gender;

    //This is only here due to the background sprite not being cut right, naturally it would be cleaned up
    [SerializeField] Text currentHP;
    [SerializeField] Text maxHP;

    //[SerializeField] HPBar experienceBar;
    //[SerializeField] Image experienceBackground;

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
        levelText.text = currentPokemon.currentLevel.ToString();
        hPBar.SetHP((float)currentPokemon.currentHitPoints / currentPokemon.maxHitPoints);
        if(isPlayersPokemon == true)
        {
            currentHP.text = currentPokemon.currentHitPoints.ToString();
            maxHP.text = currentPokemon.maxHitPoints.ToString();
        }

        SetStatusSprite();
        _pokemon.OnStatusChanged += SetStatusSprite;

        gender.sprite = StatusConditionArt.instance.ReturnGenderArt(currentPokemon.gender);

        SetHudOffScreen(isPlayersPokemon);
        ResetHUD(isPlayersPokemon);
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

    public IEnumerator UpdateHP(int hpBeforeDamage)
    {
        yield return hPBar.SetHPAnimation(_pokemon.currentHitPoints,hpBeforeDamage,_pokemon.maxHitPoints,currentHP);
    }

    public IEnumerator FaintedPokemonHUDAnimation(bool isPlayers)
    {
        float tempAlpha = 1;
        float animationTime = 1.5f;

        while (tempAlpha > 0f)
        {
            tempAlpha -= (0.01f * animationTime);
            hudBackground.color = hudBackground.color.SetAlpha(tempAlpha);
            nameText.color = nameText.color.SetAlpha(tempAlpha);
            levelText.color = levelText.color.SetAlpha(tempAlpha);
            gender.color = gender.color.SetAlpha(tempAlpha);
            healthBarBackground.color = healthBarBackground.color.SetAlpha(tempAlpha);
            hPBar.healthBarImage.color = hPBar.healthBarImage.color.SetAlpha(tempAlpha);
            if(isPlayers == true)
            {
                currentHP.color = currentHP.color.SetAlpha(tempAlpha);
                maxHP.color = maxHP.color.SetAlpha(tempAlpha);
            }
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

    void ResetHUD(bool isPlayers)
    {
        hudBackground.color = hudBackground.color.ResetAlpha();
        nameText.color = nameText.color.ResetAlpha();
        levelText.color = levelText.color.ResetAlpha();
        gender.color = gender.color.ResetAlpha();
        healthBarBackground.color = healthBarBackground.color.ResetAlpha();
        hPBar.healthBarImage.color = hPBar.healthBarImage.color.ResetAlpha();
        if (isPlayers == true)
        {
            currentHP.color = currentHP.color.ResetAlpha();
            maxHP.color = maxHP.color.ResetAlpha();
        }
    }

    public void PlayEnterAnimation(float duration)
    {
        StartCoroutine(SmoothTransitionToPosition(_originalPos, duration));
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
}
