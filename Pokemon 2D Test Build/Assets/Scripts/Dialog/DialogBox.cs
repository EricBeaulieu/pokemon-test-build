using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DialogBox : MonoBehaviour
{
    [SerializeField] Text dialogBox;
    [SerializeField] GameObject choiceBox;

    [SerializeField] GameObject choiceYesButton;
    [SerializeField] GameObject choiceNoButton;

    public void ShowDialogBox(bool on)
    {
        gameObject.SetActive(on);
    }

    public void SetDialogText(string dialog)
    {
        dialogBox.text = dialog;
    }

    public IEnumerator SetChoiceBox(Action onYesSelected, Action onNoSelected = null)
    {
        EnableChoiceBox(true);
        ChoiceSelectBox();

        bool waitingForInput = false;

        choiceYesButton.GetComponent<Button>().onClick.RemoveAllListeners();
        choiceYesButton.GetComponent<Button>().onClick.AddListener(() =>
        {
            onYesSelected?.Invoke();
            waitingForInput = true;
        });


        choiceNoButton.GetComponent<Button>().onClick.RemoveAllListeners();
        choiceNoButton.GetComponent<Button>().onClick.AddListener(() =>
        {
            waitingForInput = true;
            onNoSelected?.Invoke();
        });

        while (waitingForInput == false)
        {
            yield return null;
        }
        EnableChoiceBox(false);
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
}
