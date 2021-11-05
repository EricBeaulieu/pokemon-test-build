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
    [SerializeField] int numberOfBoxes = 30;
    PCBoxData[] boxData;

    [SerializeField] PCTopButton partyButton;
    [SerializeField] PCTopButton closeButton;
    const string CANT_CLOSE_PC_MESSAGE = "You're holding a Pokemon!";

    int _boxIndex = 0;
    int boxIndex
    {
        get { return _boxIndex; }
        set
        {
            _boxIndex += value;
            if (_boxIndex < 0)
            {
                _boxIndex = numberOfBoxes;
            }
            else if (_boxIndex > numberOfBoxes)
            {
                _boxIndex = 0;
            }
        }
    }

    public override void OpenSystem(bool specifiedBool = false)
    {
        GameManager.SetGameState(GameState.PC);
        gameObject.SetActive(true);
        dialogSystem.SetCurrentDialogBox(dialogBox);
        dialogSystem.ActivateDialog(false);
        currentBox.SetupData(boxData[boxIndex]);
        selectableBox.SetLastSelected(null);
        selectableBox.SelectBox();
        pointer.MoveToPosition(currentBox.GetPCPokemonAtIndex(0).gameObject.transform.position);
        pointer.UpdateData();
        pCPokemonDataDisplay.SetupData(currentBox.GetPCPokemonAtIndex(0).currentPokemon);
        //player busy
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
        gameObject.SetActive(false);
        Debug.Log("closed PC");
        //player active
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
        //load all current pokemon inside of the boxes
        //currentBox.Initialization();
        pointer = GetComponentInChildren<PCPointer>();
        pCPokemonDataDisplay = GetComponentInChildren<PCPokemonDataDisplay>();
        selectableBox = new SelectableBoxUI(currentBox.GetPCPokemonAtIndex(0).gameObject);
        pointer.Initialization(selectableBox);
        Debug.Log("pointer", pointer.gameObject);
        //Set party button here
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

        selectableBox.SelectBox();
    }

    //on close box or changing save the current box info
}
