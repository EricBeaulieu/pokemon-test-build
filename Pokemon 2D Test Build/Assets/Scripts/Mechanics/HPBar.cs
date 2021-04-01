using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
    Image _healthBar;

    void Awake()
    {
        _healthBar = GetComponent<Image>();
    }

    public void SetHP(float hpNormalized)
    {
        _healthBar.fillAmount = hpNormalized;
    }

    public IEnumerator SetHPAnimation(int healthAfterDamage,int healthBeforeDamage,int maxHP,Text currentHpText = null)
    {
        float curHP = healthBeforeDamage;
        float changeAmount = curHP - healthAfterDamage;
        while(curHP - healthAfterDamage > Mathf.Epsilon)
        {
            curHP -= changeAmount * Time.deltaTime;
            _healthBar.fillAmount = (float)curHP/maxHP;
            if(currentHpText != null)
            {
                currentHpText.text = curHP.ToString("0");
            }
            yield return null;
        }

        SetHP((float)healthAfterDamage / maxHP);
    }

    public Image healthBarImage
    {
        get { return _healthBar; }
    }
}
