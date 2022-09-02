using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
    [SerializeField] Image healthBar;

    public static float meduimColourThreshold = 0.5f;
    public static float lowColourThreshold = 0.2f;

    void SetHP(float hpNormalized)
    {
        healthBar.fillAmount = hpNormalized;
        healthBar.color = GlobalArt.ReturnHitPointsColor(hpNormalized);
    }

    public IEnumerator SetHPDamageAnimation(int healthAfterDamage,int healthBeforeDamage,int maxHP,Text currentHpText = null)
    {
        float curHP = healthBeforeDamage;
        float changeAmount = curHP - healthAfterDamage;
        while(curHP - healthAfterDamage > Mathf.Epsilon)
        {
            curHP -= changeAmount * Time.deltaTime;
            curHP = Mathf.Clamp(curHP, healthAfterDamage, maxHP);
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

    public IEnumerator SetHPRecoveredAnimation(int healthAfterRecovery, int hpHealed, int maxHP, Text currentHpText = null)
    {
        float curHP = healthAfterRecovery - hpHealed;
        while (healthAfterRecovery - curHP > Mathf.Epsilon)
        {
            curHP += hpHealed * Time.deltaTime;
            SetHP((float)curHP / maxHP);
            if (currentHpText != null)
            {
                currentHpText.text = $"{curHP.ToString("0")} / {maxHP.ToString()}";
            }
            yield return null;
        }

        SetHP((float)healthAfterRecovery / maxHP);

        if (currentHpText != null)
        {
            currentHpText.text = $"{healthAfterRecovery.ToString("0")} / {maxHP.ToString()}";
        }
    }

    public void SetHPWithoutAnimation(int currentHp, int maxHP, Text currentHpText = null)
    {
        float hpNormalized = (float)currentHp / (float)maxHP;
        SetHP(hpNormalized);
        if (currentHpText != null)
        {
            currentHpText.text = $"{currentHp.ToString("0")} / {maxHP.ToString()}";
        }
    }

    public Image healthBarImage
    {
        get { return healthBar; }
    }
}
