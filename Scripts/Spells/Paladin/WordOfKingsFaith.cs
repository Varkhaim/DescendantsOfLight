using UnityEngine;
using System.Collections;

public class WordOfKingsFaith : SpellEffect
{
    public WordOfKingsFaith() : base()
    {
    }

    public override void OnCast(Caster who, Soldier target)
    {
        if (who.AuraActive(AURA.FLASH_OF_FUTURE))
        {
            if (Random.Range(0, 100) < VALUES.FLASH_OF_FUTURE_PROC2+who.AuraStacks(AURA.FLASH_OF_FUTURE))
            {
                GameCore.Core.buffSystem.BuffMe(CASTERBUFF.FLASH_OF_FUTURE_LIGHT, 900f, who);
                GameObject _icon = GameCore.Core.FindSpellByName("Word of Kings: Light").myIcon;
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
        SpellInfo spellInfo = GameCore.Core.spellRepository.Get(SPELL.WORD_OF_KINGS_FAITH);
        int _dur = (int)(spellInfo.ticksCount * spellInfo.HoTgap / (1f+who.myAura[(int)AURA.EMPATHY].stacks*VALUES.EMPATHY_PERCENT2));
        int _gap = (int)(spellInfo.HoTgap / (1f + who.myAura[(int)AURA.EMPATHY].stacks * VALUES.EMPATHY_PERCENT2));
        if (who.myAura[(int)AURA.CONSECRATION].isActive)
            GameCore.Core.buffSystem.BuffMe(CASTERBUFF.CONSECRATION, 600f, who);
        target.BuffMe((int)Buff.DB.WORD_OF_KINGS_FAITH, _dur, who, spellInfo, _gap);
        if (!target.frame.GetComponent<AudioSource>().isPlaying)
            target.frame.GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("Sounds/WoKFaithSound"));
    }

}