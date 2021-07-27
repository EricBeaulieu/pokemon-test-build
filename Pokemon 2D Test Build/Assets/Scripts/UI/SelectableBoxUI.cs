using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectableBoxUI
{
    GameObject lastSelected = null;
    GameObject defaultSelectableGameobject;

    public SelectableBoxUI(GameObject defaultGameobject)
    {
        defaultSelectableGameobject = defaultGameobject;
    }

    public void SelectBox(GameObject gameObject = null)
    {
        Deselect();
        if (lastSelected == null)
        {
            EventSystem.current.SetSelectedGameObject(defaultSelectableGameobject);
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(lastSelected);
        }

        if (gameObject != null)
        {
            EventSystem.current.SetSelectedGameObject(gameObject);
        }
    }

    public void SetLastSelected(GameObject gameObject)
    {
        lastSelected = gameObject;
    }

    public void Deselect()
    {
        EventSystem.current.SetSelectedGameObject(null);
    }
}
