using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummarySystem : MonoBehaviour
{
    [SerializeField] SummaryDisplay display;
    [SerializeField] SummaryInfo info;
    [SerializeField] SummarySkills skills;
    [SerializeField] SummaryMoves moves;

    PartySystem partySystem;

    int _currentPage;
    int _pokemonIndex;
    List<Pokemon> _availablePokemon;
    bool _animating;
    Vector2 _currentInput;

    public void Initialization()
    {
        partySystem = GameManager.instance.GetPartySystem;
    }

    void Update()
    {
        _currentInput.x = Input.GetAxisRaw("Horizontal");
        _currentInput.y = Input.GetAxisRaw("Vertical");

        if(_animating == false)
        {
            if (_currentInput.x != 0)
            {
                _currentInput.x = _currentInput.x > 0 ? 1 : -1;
            }

            if (_currentInput.y != 0)
            {
                _currentInput.y = _currentInput.y > 0 ? 1 : -1;
            }

            if (_currentInput.x != 0) _currentInput.y = 0;

            if (_currentInput != Vector2.zero)
            {
                if (_currentInput.x != 0)
                {
                    StartCoroutine(SwitchPage(Mathf.RoundToInt(_currentInput.x)));
                }
                else//_currentInput.y != 0
                {
                    StartCoroutine(SwitchPokemon(Mathf.RoundToInt(-_currentInput.y)));
                }
                    
            }
        }

        if (Input.GetButtonDown("Fire2"))
        {
            CloseSummarySystem();
        }

    }

    public void OnSummaryMenuOpened(List<Pokemon> allAvailablePokemon,int index)
    {
        gameObject.SetActive(true);
        _animating = false;
        _currentPage = 0;
        _pokemonIndex = index;
        _availablePokemon = allAvailablePokemon;

        SetupCurrentPokemonData(allAvailablePokemon[index]);
        display.SetPosition(true);
        info.SetPosition(true);
        skills.SetPosition(false);
        moves.SetPosition(false);
    }

    public void CloseSummarySystem()
    {
        partySystem.ReturnFromSummarySystem();
        gameObject.SetActive(false);
    }

    void SetupCurrentPokemonData(Pokemon pokemon)
    {
        display.SetupData(pokemon);
        info.SetupData(pokemon);
        skills.SetupData(pokemon);
        moves.SetupData(pokemon);
    }

    IEnumerator SwitchPage(int direction)
    {
        _animating = true;
        //Go Right
        if(direction > 0)
        {
            if(_currentPage == 0)
            {
                yield return info.Animate(false);
                yield return skills.Animate(true);
                _currentPage++;
            }
            else if (_currentPage == 1)
            {
                yield return skills.Animate(false);
                yield return moves.Animate(true);
                _currentPage++;
            }
        }
        else//Left
        {
            if (_currentPage == 1)
            {
                yield return skills.Animate(false);
                yield return info.Animate(true);
                _currentPage--;
            }
            else if (_currentPage == 2)
            {
                yield return moves.Animate(false);
                yield return skills.Animate(true);
                _currentPage--;
            }
        }
        _animating = false;
    }

    IEnumerator SwitchPokemon(int direction)
    {
        _animating = true;
        if(_pokemonIndex + direction >= 0 && _pokemonIndex + direction < _availablePokemon.Count)
        {
            _pokemonIndex += direction;
            SetupCurrentPokemonData(_availablePokemon[_pokemonIndex]);
        }
        yield return new WaitForSeconds(0.25f);
        _animating = false;
    }
}
