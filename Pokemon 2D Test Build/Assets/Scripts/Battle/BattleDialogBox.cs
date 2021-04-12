using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BattleDialogBox : MonoBehaviour
{
    [SerializeField] Text _dialogBox;
    [SerializeField] int lettersPerSecond = 30;

    [SerializeField] GameObject actionSelector;
    [SerializeField] GameObject moveSelector;
    [SerializeField] GameObject moveDetails;
    [SerializeField] GameObject choiceBox;

    [SerializeField] List<Text> actionsText;
    [SerializeField] List<Text> moveText;

    [SerializeField] GameObject choiceYesButton;
    [SerializeField] GameObject choiceNoButton;

    bool _waitingOnUserInput = false;
    bool _waitingOnUserChoice = false;

    public void SetDialogText(string dialog)
    {
        _dialogBox.text = dialog;
    }

    public IEnumerator TypeDialog(string dialog,bool makeUserWait = false)
    {
        _dialogBox.text = "";

        foreach(char letter in dialog.ToCharArray())
        {
            _dialogBox.text += letter;
            yield return new WaitForSeconds(1f / lettersPerSecond);
        }
        yield return new WaitForSeconds(1f);
        if (makeUserWait)
        {
            _dialogBox.text += DialogManager.indicatorWhenWaitingOnInput;
            _waitingOnUserInput = true;

            while (_waitingOnUserInput == true)
            {
                yield return null;
            }
        }
    }

    public void BattleStartSetup()
    {
        EnableActionSelector(false);
        EnableMoveSelector(false);
        EnableChoiceBox(false);
    }

    public void EnableActionSelector(bool enabled)
    {
        actionSelector.SetActive(enabled);
    }

    public void EnableMoveSelector(bool enabled)
    {
        moveSelector.SetActive(enabled);
        moveDetails.SetActive(enabled);
    }

    void EnableChoiceBox(bool enabled)
    {
        choiceBox.SetActive(enabled);
    }

    public bool WaitingOnUserInput
    {
        get { return _waitingOnUserInput; }
        set
        {
            _waitingOnUserInput = value;
        }
    }

    public bool WaitingOnUserChoice
    {
        get { return _waitingOnUserChoice; }
        set
        {
            _waitingOnUserChoice = value;
            EnableChoiceBox(value);
        }
    }

    void ChoiceSelectBox()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(choiceYesButton);
    }

    public IEnumerator SetChoiceBox(Action onYesSelected,Action onNoSelected = null)
    {
        WaitingOnUserChoice = true;
        ChoiceSelectBox();

        choiceYesButton.GetComponent<Button>().onClick.RemoveAllListeners();
        choiceYesButton.GetComponent<Button>().onClick.AddListener(() =>
        {
            onYesSelected?.Invoke();
            //Hides the box and waits until everything else has been then it continues
            choiceBox.SetActive(false);
        });

        choiceNoButton.GetComponent<Button>().onClick.RemoveAllListeners();
        choiceNoButton.GetComponent<Button>().onClick.AddListener(() =>
        {
            WaitingOnUserChoice = false;
            onNoSelected?.Invoke();
        });

        while (WaitingOnUserChoice == true)
        {
            yield return null;
        }
    }

    public IEnumerator AfterDialogWait()
    {
        _dialogBox.text += DialogManager.indicatorWhenWaitingOnInput;
        _waitingOnUserInput = true;

        while (_waitingOnUserInput == true)
        {
            yield return null;
        }
    }
}
