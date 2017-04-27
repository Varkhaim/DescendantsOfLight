using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class Buff
{


    public int ID;
    public float duration;
    public float maxDuration;
    public GameObject icon = null;
    public float multiplier = 1f;
    public GameObject myText;
    public Soldier myParent;
    public Caster myCaster;
    public int refreshCount = 0;
    public SpellEffect mEffect;
    public SpellInfo spellInfo;
    public int minv, maxv;
    public int gap;

    public enum DB
    {
        NOTHING,
        WORD_OF_KINGS_FAITH,
        WORD_OF_KINGS_LOYALTY,
        ROYALTY,
        FLASH_OF_FUTURE,
        SOOTHING_VOID,
        TWILIGHT_BEAM,
        SHADOWMEND
    }

    public Buff(int _ID, int _duration, Soldier _myParent, Caster _caster, SpellInfo _info, int _gap, int _minv =0, int _maxv =0)
    {
        ID = _ID;
        maxDuration = _duration;
        duration = maxDuration;
        myParent = _myParent;
        myCaster = _caster;
        spellInfo = _info;
        mEffect = spellInfo.effect;
        minv = _minv;
        maxv = _maxv;
        gap = _gap;
    }

    public void Refresh(float _value)
    {
        // 0 - odswiez do pelnego czasu
        if (_value == 0)
        {
            duration = maxDuration;
        }
        else
        {
            duration += _value;
        }
    }

    public void SetIcon(Sprite _icon)
    {
        icon.transform.GetChild(0).GetComponent<Image>().sprite = _icon;
    }

    public void Remove()
    {
        if (ID == (int)DB.SOOTHING_VOID)
        {
            if (refreshCount == 0)
            {
                if (!myParent.isDead)
                {
                    int _points = myCaster.myTalentTree.GetTalentPointsByName("Fading Light");
                    if (_points > 0)
                    {
                        SpellInfo _ws = GameCore.Core.spellRepository.Get(SPELL.SOOTHING_VOID);
                        int _value = 0;
                        _value = _ws.baseValue;
                        _value += (int)(GameCore.Core.chosenAccount.statPWR * _ws.coeff);
                        _value *= (int)(_points * 0.2f);

                        myParent.Heal(_value, _value + 5, GameCore.Core.criticalStrikeChance, myCaster, _ws, HEALSOURCE.FADING_LIGHT, HEALTYPE.OTHER);
                    }
                }
            }
        }
        if (ID ==(int)DB.WORD_OF_KINGS_LOYALTY)
        {
            GameCore.Core.RemoveBeaconedTarget(myParent);
        }

        Object.Destroy(myText);
        Object.Destroy(icon);
    }

    public void Update()
    {
        if (myParent.isDead)
        {
            Remove();
        }
        else
        {
            duration = Mathf.Max(0, --duration);
            if (icon != null)
            {
                icon.transform.GetChild(3).GetComponent<Image>().fillAmount = (float)duration / (float)maxDuration;
            }
            //if (icon != null)
            //icon.GetComponent<Image>().color = new Color(1, 1, 1, Mathf.Max(duration / 180f, duration / maxDuration));
        }

        if (myText != null)
            myText.GetComponent<TextMesh>().text = Mathf.Ceil(duration / 60f).ToString();
    }

    public void Execute()
    {
        int _value = 0;
        _value = spellInfo.baseValue2 / spellInfo.ticksCount;
        _value += (int)(GameCore.Core.chosenAccount.statPWR * spellInfo.coeff2 / spellInfo.ticksCount);

        switch (ID)
        {
            case 0:
                {
                    // no buff
                }
                break;
            case (int)Buff.DB.WORD_OF_KINGS_FAITH:
                {
                    Healing temp = myParent.Heal((int)(_value * multiplier), (int)((_value + 6) * multiplier), GameCore.Core.criticalStrikeChance, myCaster, spellInfo, HEALSOURCE.WOK_FAITH, HEALTYPE.PERIODIC_SINGLE);
                    if (temp.isCrit)
                    {
                        if (myCaster.myAura[(int)AURA.DIVINITY].isActive)
                            myParent.Shield((int)(temp.value * VALUES.DIVINITY_PERCENT), HEALSOURCE.DIVNITY_SHIELD);
                    }
                    if (temp.overhealing > 0)
                    {
                        if (myCaster.myAura[(int)AURA.EMPATHY].isActive)
                        myParent.Shield((int)(temp.overhealing * VALUES.EMPATHY_PERCENT), HEALSOURCE.EMPATHY);
                    }
                }
                break;
            case (int)Buff.DB.SHADOWMEND:
                {
                    myParent.Heal(minv, maxv, 0, myCaster, spellInfo, HEALSOURCE.SHADOWMEND, HEALTYPE.PERIODIC_MULTI);
                }
                break;
            case (int)Buff.DB.SOOTHING_VOID:
                {
                    myParent.Heal(_value, _value, 0, myCaster, spellInfo, HEALSOURCE.SOOTHING_VOID, HEALTYPE.PERIODIC_SINGLE);
                }
                break;
            case (int)Buff.DB.TWILIGHT_BEAM:
                {
                    float _pen = 1.5f - (multiplier*0.1f);
                    int _val = (int)(_value * _pen);

                    Healing _heal = myParent.Heal(_val, _val, GameCore.Core.criticalStrikeChance, myCaster, spellInfo, HEALSOURCE.TWILIGHT_BEAM, HEALTYPE.PERIODIC_SINGLE);
                    multiplier += 1;
                }
                break;
        }
    }
}

