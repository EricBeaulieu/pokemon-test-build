using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BattleUnit : MonoBehaviour
{
    [SerializeField] Image trainerImage;
    Vector3 trainerImageOriginalPosition;
    public bool startingAnimationsActive { get; private set; }
    [SerializeField] Image battleFloor;
    Vector3 battleFloorOriginalPosition;
    [SerializeField] Image pokemonSprite;
    Sprite[] _pokemonSpriteAnimations;
    Vector3 pokemonSpriteOriginalPosition;
    float _imageSize;

    [SerializeField] bool isPlayersPokemon;
    public bool isPlayerPokemon { get { return isPlayersPokemon; } }

    [SerializeField] BattleHUD hud;
    public BattleHUD HUD { get { return hud; } }

    public Pokemon pokemon {get;set;}
    bool _sendOutPokemonOnTurnEnd = false;
    [SerializeField] BattleAbilityUI abilityUI;

    [SerializeField] Image statusEffectA;
    [SerializeField] Image statusEffectB;
    [SerializeField] Image overtopImage;
    const float STATUS_EFFECT_ANIMATION_SPEED = 1f;
    const float ENTRY_SPRITE_ANIMATION_SPEED = 0.8f;
    const float START_ANIMATION_SPEED = 2.25f;
    const float HUD_ANIMATION_SPEED = 0.75f;

    //This is mainly for the enemy pokemon, so they gain XP
    List<Pokemon> pokemonBattledAgainst;

    void Awake()
    {
        if (battleFloor == null)
        {
            Debug.LogWarning($"battleFloor has not been set", gameObject);
        }
        if (pokemonSprite == null)
        {
            Debug.LogWarning($"pokemonSprite has not been set", gameObject);
        }
        if (hud == null)
        {
            Debug.LogWarning($"hud has not been set", gameObject);
        }
        if (statusEffectA == null)
        {
            Debug.LogWarning($"statusEffectA has not been set", gameObject);
        }
        if (statusEffectB == null)
        {
            Debug.LogWarning($"statusEffectB has not been set", gameObject);
        }

        hud.PrecautionsCheck(isPlayersPokemon);

        statusEffectA.transform.localPosition = Vector3.zero;
        statusEffectB.transform.localPosition = Vector3.zero;
        statusEffectA.color = statusEffectA.color.SetAlpha(0);
        statusEffectB.color = statusEffectA.color.SetAlpha(0);

        trainerImageOriginalPosition = trainerImage.rectTransform.localPosition;
        battleFloorOriginalPosition = battleFloor.rectTransform.localPosition;
        pokemonSpriteOriginalPosition = pokemonSprite.rectTransform.localPosition;
        abilityUI.OnStart(isPlayersPokemon);
        _imageSize = pokemonSprite.rectTransform.sizeDelta.x;
        overtopImage.sprite = StatusConditionArt.instance.Nothing;
    }

    public void SetupAndSendOut(Pokemon pokemon)
    {
        StandardSetupProcedure(pokemon);

        StartCoroutine(PlayEnterAnimation());
    }

    public void SendOut()
    {
        EnablePokemon(true);
        EnableTrainer(false);
        StartCoroutine(PlayEnterAnimation());
    }

    public void SetDataBattleStart(Pokemon pokemon,Sprite trainerSprite)
    {
        startingAnimationsActive = true;
        pokemonSprite.rectTransform.sizeDelta = new Vector2(_imageSize, _imageSize);

        StandardSetupProcedure(pokemon);

        if (trainerSprite != null)
        {
            trainerImage.sprite = trainerSprite;
            EnablePokemon(false);
            EnableTrainer(true);
        }
        else
        {
            EnablePokemon(true);
            EnableTrainer(false);
        }

        StartCoroutine(PlayBattleOpeningAnimation(trainerSprite != null));
    }

    void StandardSetupProcedure(Pokemon pokemon)
    {
        if (isPlayersPokemon)
        {
            _pokemonSpriteAnimations = pokemon.pokemonBase.GetBackSprite(pokemon.isShiny, pokemon.gender);
        }
        else
        {
            _pokemonSpriteAnimations = pokemon.pokemonBase.GetFrontSprite(pokemon.isShiny, pokemon.gender);
        }

        pokemonSprite.sprite = _pokemonSpriteAnimations[0];
        this.pokemon = pokemon;
        hud.SetData(pokemon, isPlayersPokemon);
        pokemon.Reset();
        _sendOutPokemonOnTurnEnd = false;

        pokemonBattledAgainst = new List<Pokemon>();
    }

    IEnumerator PlayBattleOpeningAnimation(bool hasTrainer)
    {
        GameObject temp;
        Vector3 tempEndPos;

        if(isPlayersPokemon)
        {
            trainerImage.transform.localPosition = new Vector3(trainerImageOriginalPosition.x + 800f, trainerImageOriginalPosition.y);
            battleFloor.transform.localPosition = new Vector3(battleFloorOriginalPosition.x + 800f, battleFloorOriginalPosition.y);
            temp = trainerImage.gameObject;
            tempEndPos = trainerImageOriginalPosition;
        }
        else
        {
            if (hasTrainer == true)
            {
                trainerImage.transform.localPosition = new Vector3(trainerImageOriginalPosition.x - 800f, trainerImageOriginalPosition.y);
                battleFloor.transform.localPosition = new Vector3(battleFloorOriginalPosition.x - 800f, battleFloorOriginalPosition.y);
                temp = trainerImage.gameObject;
                tempEndPos = trainerImageOriginalPosition;
            }
            else
            {
                EnablePokemon(true);
                pokemonSprite.color = pokemonSprite.color.SetAlpha(1);
                pokemonSprite.transform.localPosition = new Vector3(pokemonSpriteOriginalPosition.x - 800f, pokemonSpriteOriginalPosition.y);
                battleFloor.transform.localPosition = new Vector3(battleFloorOriginalPosition.x - 800f, battleFloorOriginalPosition.y);
                temp = pokemonSprite.gameObject;
                tempEndPos = pokemonSpriteOriginalPosition;
            }
        }

        StartCoroutine(SmoothTransitionToPosition(temp, tempEndPos, 2f));
        yield return SmoothTransitionToPosition(battleFloor.gameObject, battleFloorOriginalPosition, 2f);
        startingAnimationsActive = false;
    }

    public IEnumerator PlayTrainerExitAnimation(bool hasTrainer)
    {
        if (hasTrainer == true)
        {
            GameObject temp = trainerImage.gameObject;
            Vector3 tempEndPos;

            if (isPlayersPokemon)
            {
                tempEndPos = new Vector3(trainerImageOriginalPosition.x - 350f, trainerImageOriginalPosition.y);
                yield return SmoothTransitionToPosition(temp, tempEndPos, 1f);
            }
            else
            {
                tempEndPos = new Vector3(trainerImageOriginalPosition.x + 350f, trainerImageOriginalPosition.y);
                yield return SmoothTransitionToPosition(temp, tempEndPos, 1f);
            }
        }
        else
        {
            yield return AnimateSpriteUponEntry();
            yield return hud.PlayEnterAnimation(HUD_ANIMATION_SPEED);
        }
    }

    IEnumerator PlayEnterAnimation()
    {
        pokemonSprite.color = pokemonSprite.color.SetAlpha(1);

        if (isPlayersPokemon)
        {
            pokemonSprite.transform.localPosition = new Vector3(pokemonSpriteOriginalPosition.x - 400f, pokemonSpriteOriginalPosition.y);
            yield return SmoothTransitionToPosition(pokemonSprite.gameObject, pokemonSpriteOriginalPosition, 0.75f);
        }
        else
        {
            pokemonSprite.transform.localPosition = new Vector3(pokemonSpriteOriginalPosition.x + 400f, pokemonSpriteOriginalPosition.y);
            yield return SmoothTransitionToPosition(pokemonSprite.gameObject, pokemonSpriteOriginalPosition, 0.75f);
        }

        yield return AnimateSpriteUponEntry();
        yield return hud.PlayEnterAnimation(HUD_ANIMATION_SPEED);
    }

    IEnumerator SmoothTransitionToPosition(GameObject gameobject,Vector3 endPos, float duration)
    {
        Transform tempTrans = gameobject.transform;

        Vector3 startingPos = gameobject.transform.localPosition;
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            tempTrans.localPosition = Vector3.Lerp(startingPos, endPos, (elapsedTime / duration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        tempTrans.localPosition = endPos;
    }

    public IEnumerator PlayAttackAnimation()
    {
        Vector3 targetLocation = pokemonSprite.transform.localPosition;

        if(isPlayersPokemon == true)
        {
            targetLocation.x += 50f;
        }
        else
        {
            targetLocation.x -= 50f;
        }

        yield return SmoothTransitionToPosition(pokemonSprite.gameObject,targetLocation, 0.15f);
        yield return SmoothTransitionToPosition(pokemonSprite.gameObject, pokemonSpriteOriginalPosition, 0.3f);
    }

    public void PlayHitAnimation()
    {
        StartCoroutine(HitanimationFlash());
    }

    IEnumerator HitanimationFlash()
    {
        bool isImageOff = false;

        int totalAmountOfFlashes = 0;
        int amountOfFlashes = 3;

        float durationBetweenFlashes = 0.1f;
        float time = 0;

        while(totalAmountOfFlashes < amountOfFlashes)
        {

            if (time >= durationBetweenFlashes)
            {
                time = 0;
                isImageOff = !isImageOff;
                if (isImageOff == true)
                {
                    pokemonSprite.color = new Color(1, 1, 1, 0);
                    totalAmountOfFlashes++;
                }
                else
                {
                    pokemonSprite.color = new Color(1, 1, 1, 1);
                }
            }
            time += Time.deltaTime;
            yield return null;
        }

        pokemonSprite.color = new Color(1,1,1,1);
    }

    public void PlayFaintAnimation()
    {
        StartCoroutine(FaintAnimation());
        StartCoroutine(hud.FaintedPokemonHUDAnimation(isPlayersPokemon));
    }

    IEnumerator FaintAnimation()
    {
        Transform tempTrans = pokemonSprite.transform;

        Vector3 startingPos = pokemonSprite.transform.localPosition;
        Vector3 endPos = new Vector3(startingPos.x, startingPos.y - 50);
        float elapsedTime = 0;
        float duration = 0.4f;

        while (elapsedTime < duration)
        {
            tempTrans.localPosition = Vector3.Lerp(startingPos, endPos, (elapsedTime / duration));
            pokemonSprite.color = new Color(1, 1, 1, -(elapsedTime / duration)+ 1);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        tempTrans.localPosition = pokemonSpriteOriginalPosition;
    }

    public bool SendOutPokemonOnTurnEnd
    {
        get { return _sendOutPokemonOnTurnEnd; }
        set
        {
            _sendOutPokemonOnTurnEnd = value;
        }
    }

    public IEnumerator CaptureAnimation(Vector3 ballposition)
    {
        float tempAlpha = 1;
        float animationTime = 1.5f;

        Vector3 difference = ballposition - pokemonSprite.transform.localPosition;
        float minimumSizeDuringCapture = 50f;

        while (tempAlpha > 0)
        {
            tempAlpha -= (0.01f * animationTime);
            pokemonSprite.color = pokemonSprite.color.SetAlpha(tempAlpha);

            pokemonSprite.rectTransform.localPosition = pokemonSpriteOriginalPosition + (difference * (-tempAlpha +1));
            float currentSize = (_imageSize * tempAlpha) + (minimumSizeDuringCapture * (-tempAlpha + 1));
            pokemonSprite.rectTransform.sizeDelta = new Vector2(currentSize,currentSize);

            yield return new WaitForSeconds(0.01f);
        }
    }

    public IEnumerator EscapeCaptureAnimation(Vector3 ballposition)
    {
        float tempAlpha = 0;
        float animationTime = 1.5f;

        Vector3 difference = pokemonSpriteOriginalPosition - ballposition;
        float minimumSizeDuringCapture = 50f;

        while (tempAlpha < 1)
        {
            tempAlpha += (0.01f * animationTime);
            pokemonSprite.color = pokemonSprite.color.SetAlpha(tempAlpha);

            pokemonSprite.rectTransform.localPosition = ballposition + (difference * (tempAlpha));
            float currentSize = (minimumSizeDuringCapture * (-tempAlpha + 1)) + (_imageSize * tempAlpha);
            pokemonSprite.rectTransform.sizeDelta = new Vector2(currentSize, currentSize);

            yield return new WaitForSeconds(0.01f);
        }
        pokemonSprite.rectTransform.localPosition = pokemonSpriteOriginalPosition;
        pokemonSprite.rectTransform.sizeDelta = new Vector2(_imageSize, _imageSize);
    }

    public void SetBattlePositionArt(Sprite positionArt)
    {
        battleFloor.sprite = positionArt;
    }

    public IEnumerator ShowStatChanges(List<StatBoost> statChanges,bool increased)
    {
        if(statChanges.Count == 0)
        {
            yield break;
        }

        bool isMixed = false;

        if (statChanges.Count > 1)
        {
            StatAttribute currentStat;

            foreach (StatBoost stat in statChanges)
            {
                currentStat = stat.stat;
                isMixed = statChanges.First(x => x.stat != currentStat) != null;

                if(isMixed == true)
                {
                    break;
                }  
            }
        }

        if(isMixed == true)
        {
            statusEffectA.sprite = StatusConditionArt.instance.ReturnStatusChangesArt(StatAttribute.NA);
            statusEffectB.sprite = StatusConditionArt.instance.ReturnStatusChangesArt(StatAttribute.NA);
        }
        else
        {
            statusEffectA.sprite = StatusConditionArt.instance.ReturnStatusChangesArt(statChanges.First().stat);
            statusEffectB.sprite = StatusConditionArt.instance.ReturnStatusChangesArt(statChanges.First().stat);
        }

        int direction = (increased == true) ? 1 : -1;

        float size = statusEffectA.rectTransform.sizeDelta.y;
        statusEffectB.rectTransform.localPosition += new Vector3(0, (direction * size), 0);

        statusEffectA.color = StatusConditionArt.instance.PlainWhite.SetAlpha(0);
        statusEffectB.color = StatusConditionArt.instance.PlainWhite.SetAlpha(0);

        Vector3 startingPos = pokemonSprite.transform.localPosition;
        float elapsedTime = 0;
        float duration = STATUS_EFFECT_ANIMATION_SPEED;

        while (elapsedTime < duration)
        {
            statusEffectA.rectTransform.localPosition += new Vector3(0, ((direction * size) * Time.deltaTime), 0);
            statusEffectB.rectTransform.localPosition += new Vector3(0, ((direction * size)* Time.deltaTime), 0);

            statusEffectA.color = statusEffectA.color.SetAlpha(elapsedTime / duration);
            statusEffectB.color = statusEffectA.color.SetAlpha(elapsedTime / duration);

            if(increased == true)
            {
                if(statusEffectA.rectTransform.localPosition.y > direction * size)
                {
                    statusEffectA.rectTransform.localPosition -= new Vector3(0, size * 2, 0);
                }
                if (statusEffectB.rectTransform.localPosition.y > direction * size)
                {
                    statusEffectB.rectTransform.localPosition -= new Vector3(0, size * 2, 0);
                }
            }
            else
            {
                if (statusEffectA.rectTransform.localPosition.y < direction * size)
                {
                    statusEffectA.rectTransform.localPosition += new Vector3(0, size * 2, 0);
                }
                if (statusEffectB.rectTransform.localPosition.y < direction * size)
                {
                    statusEffectB.rectTransform.localPosition += new Vector3(0, size * 2, 0);
                }
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        elapsedTime = 0;

        while (elapsedTime < duration)
        {
            statusEffectA.rectTransform.localPosition += new Vector3(0, ((direction * size) * Time.deltaTime), 0);
            statusEffectB.rectTransform.localPosition += new Vector3(0, ((direction * size) * Time.deltaTime), 0);

            statusEffectA.color = statusEffectA.color.SetAlpha(1 - (elapsedTime / duration));
            statusEffectB.color = statusEffectA.color.SetAlpha(1 - (elapsedTime / duration));

            if (increased == true)
            {
                if (statusEffectA.rectTransform.localPosition.y > direction * size)
                {
                    statusEffectA.rectTransform.localPosition -= new Vector3(0, size * 2, 0);
                }
                if (statusEffectB.rectTransform.localPosition.y > direction * size)
                {
                    statusEffectB.rectTransform.localPosition -= new Vector3(0, size * 2, 0);
                }
            }
            else
            {
                if (statusEffectA.rectTransform.localPosition.y < direction * size)
                {
                    statusEffectA.rectTransform.localPosition += new Vector3(0, size * 2, 0);
                }
                if (statusEffectB.rectTransform.localPosition.y < direction * size)
                {
                    statusEffectB.rectTransform.localPosition += new Vector3(0, size * 2, 0);
                }
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        statusEffectA.color = statusEffectA.color.SetAlpha(0);
        statusEffectB.color = statusEffectA.color.SetAlpha(0);

        statusEffectA.transform.localPosition = Vector3.zero;
        statusEffectB.transform.localPosition = Vector3.zero;
    }

    public List<Pokemon> GetListOfPokemonBattledAgainst
    {
        get { return pokemonBattledAgainst; }
    }

    public void AddPokemonToBattleList(Pokemon enemyPokemon)
    {
        if(pokemonBattledAgainst.Contains(enemyPokemon) == true)
        {
            return;
        }
        else
        {
            pokemonBattledAgainst.Add(enemyPokemon);
        }
    }

    IEnumerator AnimateSpriteUponEntry()
    {
        pokemonSprite.sprite = _pokemonSpriteAnimations[1];
        yield return new WaitForSeconds(ENTRY_SPRITE_ANIMATION_SPEED);
        pokemonSprite.sprite = _pokemonSpriteAnimations[0];
    }

    void EnablePokemon(bool enabled)
    {
        pokemonSprite.gameObject.SetActive(enabled);
    }

    void EnableTrainer(bool enabled)
    {
        trainerImage.gameObject.SetActive(enabled);
    }

    public IEnumerator TrainerToField()
    {
        EnableTrainer(true);
        yield return SmoothTransitionToPosition(trainerImage.gameObject, trainerImageOriginalPosition, ENTRY_SPRITE_ANIMATION_SPEED);
    }

    public void OnAbilityActivation()
    {
        abilityUI.OnAbilityActivation(pokemon, pokemon.ability.Name,isPlayersPokemon);
    }

    public IEnumerator StatusConditionAnimation(ConditionID condition)
    {
        switch (condition)
        {
            case ConditionID.poison:
                yield return PlayPoisonAnimation();
                break;
            case ConditionID.burn:
                break;
            case ConditionID.sleep:
                break;
            case ConditionID.paralyzed:
                yield return PlayParalyzedAnimation();
                break;
            case ConditionID.frozen:
                break;
            case ConditionID.toxicPoison:
                yield return PlayPoisonAnimation();
                break;
            case ConditionID.confused:
                PlayHitAnimation();
                break;
            case ConditionID.cursed:
                break;
            default:
                break;
        }
        yield return null;
    }

    IEnumerator ShowStatusAnimationConditionColor(Color currentColour)
    {
        statusEffectA.sprite = StatusConditionArt.instance.BlankWhite;

        float elapsedTime = 0;
        float duration = STATUS_EFFECT_ANIMATION_SPEED;

        while (elapsedTime < duration)
        {
            statusEffectA.color = currentColour.SetAlpha(elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        elapsedTime = 0;

        while (elapsedTime < duration)
        {
            statusEffectA.color = currentColour.SetAlpha(1 - (elapsedTime / duration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        statusEffectA.color = StatusConditionArt.instance.PlainWhite.SetAlpha(0);
    }

    IEnumerator PlayPoisonAnimation()
    {
        yield return ShowStatusAnimationConditionColor(StatusConditionArt.instance.GetStatusConditionAnimationColour(ConditionID.poison).SetAlpha(0));
        yield return null;
    }

    IEnumerator PlayParalyzedAnimation()
    {
        float duration = 0.2f;

        for (int i = 0; i < 5; i++)
        {
            yield return SmoothTransitionToPosition(pokemonSprite.gameObject, GenerateOffestPosition(pokemonSpriteOriginalPosition), duration);
            overtopImage.sprite = StatusConditionArt.instance.GetRandomParalzedConditionAnimationArt();
        }

        yield return new WaitForSeconds(duration);
        yield return SmoothTransitionToPosition(pokemonSprite.gameObject, pokemonSpriteOriginalPosition, duration);
        overtopImage.sprite = StatusConditionArt.instance.Nothing;
    }

    Vector3 GenerateOffestPosition(Vector3 offsetSpot)
    {
        return new Vector3(offsetSpot.x + Random.Range(-5,6), offsetSpot.y + Random.Range(-5, 6));
    }
}
