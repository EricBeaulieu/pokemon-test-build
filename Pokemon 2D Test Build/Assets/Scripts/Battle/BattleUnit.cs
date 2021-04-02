using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleUnit : MonoBehaviour
{
    [SerializeField] Image battleFloor;
    Vector3 battleFloorOriginalPosition;
    [SerializeField] Image pokemonSprite;
    Vector3 pokemonSpriteOriginalPosition;
    float _imageSize;

    [SerializeField] bool _isPlayersPokemon;
    public bool isPlayerPokemon { get { return _isPlayersPokemon; } }

    [SerializeField] BattleHUD hud;
    public BattleHUD HUD { get { return hud; } }

    public Pokemon pokemon {get;set;}
    bool _sendOutPokemonOnTurnEnd = false;

    void Awake()
    {
        
        pokemonSpriteOriginalPosition = pokemonSprite.rectTransform.localPosition;
        _imageSize = pokemonSprite.rectTransform.sizeDelta.x;
    }

    public void Setup(Pokemon pokemon)
    {
        pokemonSprite.rectTransform.sizeDelta = new Vector2(_imageSize, _imageSize);
        if (_isPlayersPokemon)
        {
            pokemonSprite.sprite = pokemon.pokemonBase.GetBackSprite(pokemon.isShiny,pokemon.gender)[0];
        }
        else
        {
            pokemonSprite.sprite = pokemon.pokemonBase.GetFrontSprite(pokemon.isShiny, pokemon.gender)[0];
        }

        this.pokemon = pokemon;
        hud.SetData(pokemon, _isPlayersPokemon);
        pokemon.Reset();
        _sendOutPokemonOnTurnEnd = false;

        PlayEnterAnimation();
    }

    void PlayEnterAnimation()
    {
        if (_isPlayersPokemon)
        {
            pokemonSprite.transform.localPosition = new Vector3(pokemonSpriteOriginalPosition.x -400f, pokemonSpriteOriginalPosition.y);
        }
        else
        {
            pokemonSprite.transform.localPosition = new Vector3(pokemonSpriteOriginalPosition.x + 300f, pokemonSpriteOriginalPosition.y);
        }

        pokemonSprite.color = pokemonSprite.color.ResetAlpha();

        StartCoroutine(SmoothTransitionToPosition(pokemonSpriteOriginalPosition, 1f, hud.PlayEnterAnimation(0.75f)));        
    }

    IEnumerator SmoothTransitionToPosition(Vector3 endPos, float duration,IEnumerator calledWhenFinished = null)
    {
        Transform tempTrans = pokemonSprite.transform;

        Vector3 startingPos = pokemonSprite.transform.localPosition;
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            tempTrans.localPosition = Vector3.Lerp(startingPos, endPos, (elapsedTime / duration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        tempTrans.localPosition = pokemonSpriteOriginalPosition;

        if(calledWhenFinished != null)
        {
            StartCoroutine(calledWhenFinished);
        }
    }

    public void PlayAttackAnimation()
    {
        Vector3 targetLocation = pokemonSprite.transform.localPosition;

        if(_isPlayersPokemon == true)
        {
            targetLocation.x += 50f;
        }
        else
        {
            targetLocation.x -= 50f;
        }

        StartCoroutine(SmoothTransitionToPosition(targetLocation, 0.25f, SmoothTransitionToPosition(pokemonSpriteOriginalPosition, 0.5f)));
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
        StartCoroutine(hud.FaintedPokemonHUDAnimation(_isPlayersPokemon));
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
}
