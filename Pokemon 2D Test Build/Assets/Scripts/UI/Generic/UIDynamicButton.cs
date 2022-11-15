using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIDynamicButton : MonoBehaviour
{
    [SerializeField] Text buttonText;
    [SerializeField] Button button;
    public Button GetButton { get { return button; } }
    [SerializeField] RectTransform rectTrans;
    public RectTransform RectTransform { get { return rectTrans; } set { rectTrans = value; } }

    public void UpdateText(string newText)
    {
        buttonText.text = newText;
    }
}
