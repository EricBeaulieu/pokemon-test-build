using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PartyDialogBox : MonoBehaviour
{
    [SerializeField] Text _dialogBox;
    [SerializeField] int lettersPerSecond = 30;
    [SerializeField] GameObject choiceBox;

    [SerializeField] GameObject choiceYesButton;
    [SerializeField] GameObject choiceNoButton;

    public void SetDialogText(string dialog)
    {
        _dialogBox.text = dialog;
    }

    public IEnumerator TypeDialog(string dialog, bool makeUserWait = false)
    {
        _dialogBox.text = "";

        foreach (char letter in dialog.ToCharArray())
        {
            _dialogBox.text += letter;
            yield return new WaitForSeconds(1f / lettersPerSecond);
        }
        yield return new WaitForSeconds(1f);
        if (makeUserWait)
        {
            _dialogBox.text += DialogManager.indicatorWhenWaitingOnInput;
            bool waitingForInput = false;

            while (waitingForInput == false)
            {
                if (Input.anyKeyDown)
                {
                    waitingForInput = true;
                }
                yield return null;
            }
        }
    }

    void EnableChoiceBox(bool enabled)
    {
        choiceBox.SetActive(enabled);
    }

    void ChoiceSelectBox()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(choiceYesButton);
    }

    public IEnumerator SetChoiceBox(Action onYesSelected, Action onNoSelected = null)
    {
        EnableChoiceBox(true);
        ChoiceSelectBox();

        choiceYesButton.GetComponent<Button>().onClick.RemoveAllListeners();
        choiceYesButton.GetComponent<Button>().onClick.AddListener(() =>
        {
            onYesSelected?.Invoke();
        });
        bool waitingForInput = false;

        choiceNoButton.GetComponent<Button>().onClick.RemoveAllListeners();
        choiceNoButton.GetComponent<Button>().onClick.AddListener(() =>
        {
            waitingForInput = true;
            onNoSelected?.Invoke();
        });
        
        while (waitingForInput == false)
        {
            if (Input.anyKeyDown)
            {
                waitingForInput = true;
            }
            yield return null;
        }
        EnableChoiceBox(false);
    }
}
