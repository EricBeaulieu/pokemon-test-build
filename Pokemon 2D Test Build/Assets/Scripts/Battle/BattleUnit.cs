using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleUnit : MonoBehaviour
{
    Image _pokemonSprite;
    Vector3 originalPosition;
    [SerializeField] bool _isPlayersPokemon;

    public Pokemon pokemon {get;set;}

    void Awake()
    {
        _pokemonSprite = GetComponentInChildren<Image>();
        originalPosition = _pokemonSprite.rectTransform.localPosition;
    }

    public void Setup(Pokemon pokemon)
    {
        if(_isPlayersPokemon)
        {
            _pokemonSprite.sprite = pokemon.pokemonBase.GetBackSprite(false)[0];
        }
        else
        {
            _pokemonSprite.sprite = pokemon.pokemonBase.GetFrontSprite(false)[0];
        }

        this.pokemon = pokemon;

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

        _pokemonSprite.color = new Color(1, 1, 1, 1);

        StartCoroutine(SmoothTransitionToPosition(originalPosition, 1f));
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
}
