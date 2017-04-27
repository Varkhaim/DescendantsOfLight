using UnityEngine;
using System.Collections;

public class WordOfKingsLight : SpellEffect
{
    public WordOfKingsLight() : base()
    {

    }

    public override void OnCast(Caster who, Soldier target)
    {
        if (who.myAura[(int)AURA.HAND_OF_LIGHT].isActive)
        {
            if (Random.Range(0, 100) < 30 + who.myAura[(int)AURA.HAND_OF_LIGHT].stacks * 10)
            {
                GameCore.Core.buffSystem.BuffMe(CASTERBUFF.HAND_OF_LIGHT, 900f, who);
            }
        }

        if (who.AuraActive(AURA.FLASH_OF_FUTURE))
        {
            if (Random.Range(0, 100) < VALUES.FLASH_OF_FUTURE_PROC1+who.AuraStacks(AURA.FLASH_OF_FUTURE)*(5f/3f))
            {
                GameCore.Core.buffSystem.BuffMe(CASTERBUFF.FLASH_OF_FUTURE_FAITH, 900f, who);
                GameObject _icon = GameCore.Core.FindSpellByName("Word of Kings: Faith").myIcon;
                _icon.GetComponent<scrSpellButton>().Animate(2f);
            }
        }

        target.CastFinished(this, who);
    }

    public override void OnCastFinished(Caster who, Soldier target, int minval = 0, int maxval = 0)
    {
        Spell.Cast(this, target, who);
    }

    public override void Execute(Caster who, Soldier target, int minval = 0, int maxval = 0)
    {
        int _value = 0;
        SpellInfo spellInfo = GameCore.Core.spellRepository.Get(SPELL.WORD_OF_KINGS_LIGHT);
        _value = spellInfo.baseValue;
        _value += (int)(GameCore.Core.chosenAccount.statPWR * spellInfo.coeff);

        if (who.myAura[(int)AURA.FLASH_OF_FUTURE].isActive)
        {
            CasterBuff myb = GameCore.Core.buffSystem.FindBuff(CASTERBUFF.FLASH_OF_FUTURE_LIGHT);
            if (myb != null)
            {
                myb.Remove();
                _value *= 2;
            }
        }

        Healing temp;
        temp = target.Heal(_value, _value + 5, GameCore.Core.criticalStrikeChance, who, spellInfo, HEALSOURCE.WOK_LIGHT, spellInfo.healtype);

        if (who.myAura[(int)AURA.AURA_OF_LIGHT].isActive)
        {
            int chance = 10;
            if (temp.isCrit)
                chance += who.myAura[(int)AURA.AURA_OF_LIGHT].stacks * VALUES.AURA_OF_LIGHT_INCREASE;

            if (Random.Range(0, 100) < 30 + who.myAura[(int)AURA.AURA_OF_LIGHT].stacks)
            {
                GameCore.Core.FindSpellByName("Word of Kings: Courage").ChangeCooldown(0f);
            }
        }

        if (who.myAura[(int)AURA.DIVINITY].isActive)
        {
            if (temp.isCrit)
            {
                target.Shield((int)(temp.value * VALUES.DIVINITY_PERCENT), HEALSOURCE.DIVNITY_SHIELD);
            }
        }        

        if (who.myAura[(int)AURA.IRON_FAITH].isActive)
        {
            Buff myb = target.effectSystem.FindBuff((int)Buff.DB.WORD_OF_KINGS_FAITH);
            if (myb != null)
            {
                myb.Refresh(0);
            }
        }

        if (!target.frame.GetComponent<AudioSource>().isPlaying)
        target.frame.GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("Sounds/WoKLightSound"));

        GameObject.Instantiate(Resources.Load("Animations/lightexplosion_0"), target.frame.transform.position + new Vector3(0,0.35f,0), Quaternion.Euler(0,0,0));
    }

}