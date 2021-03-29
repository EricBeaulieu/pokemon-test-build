using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleUnit : MonoBehaviour
{
    Image _pokemonSprite;
    Vector3 originalPosition;
    float _imageSize;

    [SerializeField] bool _isPlayersPokemon;
    public bool isPlayerPokemon { get { return _isPlayersPokemon; } }

    [SerializeField] BattleHUD hud;
    public BattleHUD HUD { get { return hud; } }

    public Pokemon pokemon {get;set;}
    bool _sendOutPokemonOnTurnEnd = false;

    void Awake()
    {
        _pokemonSprite = GetComponentInChildren<Image>();
        originalPosition = _pokemonSprite.rectTransform.localPosition;
        _imageSize = _pokemonSprite.rectTransform.sizeDelta.x;
    }

    public void Setup(Pokemon pokemon)
    {
        _pokemonSprite.rectTransform.sizeDelta = new Vector2(_imageSize, _imageSize);
        if (_isPlayersPokemon)
        {
            _pokemonSprite.sprite = pokemon.pokemonBase.GetBackSprite(pokemon.isShiny,pokemon.gender)[0];
        }
        else
        {
            _pokemonSprite.sprite = pokemon.pokemonBase.GetFrontSprite(pokemon.isShiny, pokemon.gender)[0];
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
            _pokemonSprite.transform.localPosition = new Vector3(originalPosition.x -400f, originalPosition.y);
        }
        else
        {
            _pokemonSprite.transform.localPosition = new Vector3(originalPosition.x + 300f, originalPosition.y);
        }

        _pokemonSprite.color = _pokemonSprite.color.ResetAlpha();

        StartCoroutine(SmoothTransitionToPosition(originalPosition, 1f, hud.PlayEnterAnimation(0.75f)));        
    }

    IEnumerator SmoothTransitionToPosition(Vector3 endPos, float duration,IEnumerator calledWhenFinished = null)
    {
        Transform tempTrans = _pokemonSprite.transform;

        Vector3 startingPos = _pokemonSprite.transform.localPosition;
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            tempTrans.localPosition = Vector3.Lerp(startingPos, endPos, (elapsedTime / duration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        tempTrans.localPosition = originalPosition;

        if(calledWhenFinished != null)
        {
            StartCoroutine(calledWhenFinished);
        }
    }

    public void PlayAttackAnimation()
    {
        Vector3 targetLocation = _pokemonSprite.transform.localPosition;

        if(_isPlayersPokemon == true)
        {
            targetLocation.x += 50f;
        }
        else
        {
            targetLocation.x -= 50f;
        }

        StartCoroutine(SmoothTransitionToPosition(targetLocation, 0.25f, SmoothTransitionToPosition(originalPosition, 0.5f)));
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
                    _pokemonSprite.color = new Color(1, 1, 1, 0);
                    totalAmountOfFlashes++;
                }
                else
                {
                    _pokemonSprite.color = new Color(1, 1, 1, 1);
                }
            }
            time += Time.deltaTime;
            yield return null;
        }

        _pokemonSprite.color = new Color(1,1,1,1);
    }

    public void PlayFaintAnimation()
    {
        StartCoroutine(FaintAnimation());
        StartCoroutine(hud.FaintedPokemonHUDAnimation(_isPlayersPokemon));
    }

    IEnumerator FaintAnimation()
    {
        Transform tempTrans = _pokemonSprite.transform;

        Vector3 startingPos = _pokemonSprite.transform.localPosition;
        Vector3 endPos = new Vector3(startingPos.x, startingPos.y - 50);
        float elapsedTime = 0;
        float duration = 0.4f;

        while (elapsedTime < duration)
        {
            tempTrans.localPosition = Vector3.Lerp(startingPos, endPos, (elapsedTime / duration));
            _pokemonSprite.color = new Color(1, 1, 1, -(elapsedTime / duration)+ 1);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        tempTrans.localPosition = originalPosition;
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

        Vector3 difference = ballposition - _pokemonSprite.transform.localPosition;
        float minimumSizeDuringCapture = 50f;

        while (tempAlpha > 0)
        {
            tempAlpha -= (0.01f * animationTime);
            _pokemonSprite.color = _pokemonSprite.color.SetAlpha(tempAlpha);

            _pokemonSprite.rectTransform.localPosition = originalPosition + (difference * (-tempAlpha +1));
            float currentSize = (_imageSize * tempAlpha) + (minimumSizeDuringCapture * (-tempAlpha + 1));
            _pokemonSprite.rectTransform.sizeDelta = new Vector2(currentSize,currentSize);

            yield return new WaitForSeconds(0.01f);
        }
    }

    public IEnumerator EscapeCaptureAnimation(Vector3 ballposition)
    {
        float tempAlpha = 0;
        float animationTime = 1.5f;

        Vector3 difference = originalPosition - ballposition;
        float minimumSizeDuringCapture = 50f;

        while (tempAlpha < 1)
        {
            tempAlpha += (0.01f * animationTime);
            _pokemonSprite.color = _pokemonSprite.color.SetAlpha(tempAlpha);

            _pokemonSprite.rectTransform.localPosition = ballposition + (difference * (tempAlpha));
            float currentSize = (minimumSizeDuringCapture * (-tempAlpha + 1)) + (_imageSize * tempAlpha);
            _pokemonSprite.rectTransform.sizeDelta = new Vector2(currentSize, currentSize);

            yield return new WaitForSeconds(0.01f);
        }
        _pokemonSprite.rectTransform.localPosition = originalPosition;
        _pokemonSprite.rectTransform.sizeDelta = new Vector2(_imageSize, _imageSize);
    }

}
