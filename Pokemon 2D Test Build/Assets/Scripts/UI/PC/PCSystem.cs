using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PCSystem : CoreSystem
{
    DialogManager dialogSystem;

    public static PCPointer pointer { get; private set; }
    public static PCPokemonDataDisplay pCPokemonDataDisplay { get; private set; }
    SelectableBoxUI selectableBox;
    [SerializeField] PCCurrentBoxInfo currentBox;
    [SerializeField] PCCurrentBoxInfo nextBox;
    [SerializeField] int numberOfBoxes = 30;
    PCBoxData[] boxData;

    [SerializeField] PCParty partyPokemon;
    [SerializeField] PCTopButton partyButton;
    [SerializeField] PCTopButton closeButton;
    const string CANT_CLOSE_PC_MESSAGE = "You're holding a Pokemon!";
    const string CANT_TAKE_LAST_POKEMON_PC_MESSAGE = "This is you last Pokemon!";
    const float BOX_SPEED_ANIMATION = .5f;

    int _boxIndex = 0;
    int boxIndex
    {
        get { return _boxIndex; }
        set
        {
            _boxIndex = value;
            if (_boxIndex < 0)
            {
                _boxIndex = numberOfBoxes-1;
            }
            else if (_boxIndex >= numberOfBoxes)
            {
                _boxIndex = 0;
            }
        }
    }

    public override void OpenSystem(bool specifiedBool = false)
    {
        gameObject.SetActive(true);
        dialogSystem.SetCurrentDialogBox(dialogBox);
        dialogSystem.ActivateDialog(false);
        GameManager.SetGameState(GameState.PC);
        currentBox.SetupData(boxData[boxIndex]);
        selectableBox.SelectBox();
        pointer.UpdateData();
        partyPokemon.SetupData();
    }

    protected override void CloseSystem()
    {
        if(pointer.currentPokemon != null)
        {
            StartCoroutine(CannotClosePCWhileHoldingPokemon());
            return;
        }
        GameManager.SetGameState(GameState.Overworld);
        dialogSystem.SetCurrentDialogBox();
        boxData[boxIndex].SavePokemonInsideBox(currentBox);
        GameManager.instance.GetPlayerController.pokemonParty.LoadPlayerParty(partyPokemon.PartyPokemon());
        gameObject.SetActive(false);
    }

    public override void HandleUpdate()
    {
        if(Input.GetKeyDown("Fire2"))
        {
            CloseSystem();
        }
    }

    public override void Initialization()
    {
        gameObject.SetActive(false);
        dialogSystem = GameManager.instance.GetDialogSystem;
        boxData = new PCBoxData[numberOfBoxes];
        for (int i = 0; i < boxData.Length; i++)
        {
            boxData[i] = new PCBoxData();
            boxData[i].boxName = $"Box {i + 1}";
        }
        pointer = GetComponentInChildren<PCPointer>();
        pCPokemonDataDisplay = GetComponentInChildren<PCPokemonDataDisplay>();
        selectableBox = new SelectableBoxUI(currentBox.GetPCPokemonAtIndex(0).gameObject);
        pointer.Initialization(selectableBox);
        partyButton.GetButton().onClick.AddListener(() => { StartCoroutine(SelectPlayerParty()); });
        partyButton.ClosePartyScreen += () => { StartCoroutine(SelectPlayerParty()); };
        closeButton.GetButton().onClick.AddListener(() => { CloseSystem(); });
    }

    IEnumerator CannotClosePCWhileHoldingPokemon()
    {
        selectableBox.SetLastSelected(EventSystem.current.currentSelectedGameObject);
        selectableBox.Deselect();

        dialogSystem.SetCurrentDialogBox(dialogBox);
        dialogSystem.ActivateDialog(true);
        yield return dialogSystem.TypeDialog(CANT_CLOSE_PC_MESSAGE, true);
        dialogSystem.ActivateDialog(false);
        GameManager.SetGameState(GameState.PC);

        selectableBox.SelectBox();
    }

    public IEnumerator CannotTakeLastPartyPokemonPokemon()
    {
        selectableBox.SetLastSelected(EventSystem.current.currentSelectedGameObject);
        selectableBox.Deselect();

        dialogSystem.SetCurrentDialogBox(dialogBox);
        dialogSystem.ActivateDialog(true);
        yield return dialogSystem.TypeDialog(CANT_TAKE_LAST_POKEMON_PC_MESSAGE, true);
        dialogSystem.ActivateDialog(false);
        GameManager.SetGameState(GameState.PC);

        selectableBox.SelectBox();
    }

    public IEnumerator SelectNewBox(bool right)
    {
        selectableBox.Deselect();
        boxData[boxIndex].SavePokemonInsideBox(currentBox);
        nextBox.AlternateSetPosition(right);
        Vector3 endPosition = currentBox.startingPosition;
        if (right)
        {
            boxIndex++;
            endPosition.x -= PCCurrentBoxInfo.OTHER_BOX_OFFSET;
        }
        else
        {
            boxIndex--;
            endPosition.x += PCCurrentBoxInfo.OTHER_BOX_OFFSET;
        }

        nextBox.gameObject.SetActive(true);
        nextBox.SetupData(boxData[boxIndex]);
        StartCoroutine(GlobalTools.SmoothTransitionToPositionUsingLocalPosition(currentBox.transform, endPosition, BOX_SPEED_ANIMATION));
        yield return GlobalTools.SmoothTransitionToPositionUsingLocalPosition(nextBox.transform, currentBox.startingPosition, BOX_SPEED_ANIMATION);

        currentBox.SetupData(boxData[boxIndex]);
        currentBox.transform.localPosition = currentBox.startingPosition;
        nextBox.gameObject.SetActive(false);
        
        selectableBox.SelectBox(currentBox.GetBannerGameObject());
    }

    IEnumerator SelectPlayerParty()
    {
        yield return new WaitForSeconds(0.01f);//prevents error of deselcting the newly selected item
        selectableBox.Deselect();
        yield return partyPokemon.EnableParty();
        if(PCParty.isOn == true)
        {
            selectableBox.SelectBox(partyPokemon.ReturnFirstSelection());
        }
        else
        {
            selectableBox.SelectBox(partyButton.gameObject);
        }
    }

    public PCBoxData[] SaveBoxData()//Saving
    {
        return boxData;
    }

    public void LoadBoxData(PCBoxData[] pCBoxData)//Loading 
    {
        boxData = pCBoxData;
    }

    public void PresetBoxesWithPresetPokemon()//Testing purposes only
    {
        for (int i = 0; i < boxData.Length; i++)
        {
            boxData[i].SetBoxesWithPresetPokemon();
        }
    }


    /// <summary>
    /// Checks to see if the PC is full
    /// </summary>
    public bool IsPCFull()
    {
        for (int i = 0; i < boxData.Length; i++)
        {
            PCBoxData currentBox = boxData[i];
            for (int k = 0; k < currentBox.PokemonInsideBox.Length; k++)
            {
                if (currentBox.PokemonInsideBox[k] == null)
                {
                    return false;
                }
            }
        }
        return true;
    }

    /// <summary>
    /// finds the first available spot and deposits the caught pokemon into position
    /// </summary>
    /// <param name="newCapturedPokemon">newly captured pokemon</param>
    public void DepositPokemonInFirstAvailablePosition(Pokemon newCapturedPokemon)
    {
        for (int i = 0; i < boxData.Length; i++)
        {
            PCBoxData currentBox = boxData[i];
            for (int k = 0; k < currentBox.PokemonInsideBox.Length; k++)
            {
                if (currentBox.PokemonInsideBox[k] == null)
                {
                    currentBox.DepositPokemonAtIndex(newCapturedPokemon, k);
                    return;
                }
            }
        }
    }
}
