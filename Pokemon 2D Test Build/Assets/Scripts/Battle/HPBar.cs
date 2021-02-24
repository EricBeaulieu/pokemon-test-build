using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
    Image _healthbar;

    void Awake()
    {
        _healthbar = GetComponent<Image>();
    }

    public void SetHP(float hpNormalized)
    {
        _healthbar.fillAmount = hpNormalized;
    }
}
