using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
    Image _healthBar;

    public static float meduimColourThreshold = 0.5f;
    public static float lowColourThreshold = 0.2f;

    void Awake()
    {
        _healthBar = GetComponent<Image>();
    }

    public void SetHP(float hpNormalized)
    {
        _healthBar.fillAmount = hpNormalized;
        _healthBar.color = StatusConditionArt.instance.ReturnHitPointsColor(hpNormalized);
    }

    public IEnumerator SetHPAnimation(int healthAfterDamage,int healthBeforeDamage,int maxHP,Text currentHpText = null)
    {
        float curHP = healthBeforeDamage;
        float changeAmount = curHP - healthAfterDamage;
        while(curHP - healthAfterDamage > Mathf.Epsilon)
        {
            curHP -= changeAmount * Time.deltaTime;
            SetHP((float)curHP/maxHP);
            if(currentHpText != null)
            {
                currentHpText.text = $"{curHP.ToString("0")} / {maxHP.ToString()}";
            }
            yield return null;
        }

        SetHP((float)healthAfterDamage / maxHP);

        if (currentHpText != null)
        {
            currentHpText.text = $"{healthAfterDamage.ToString("0")} / {maxHP.ToString()}";
        }
    }

    public Image healthBarImage
    {
        get { return _healthBar; }
    }
}
