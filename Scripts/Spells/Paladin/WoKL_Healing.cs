using UnityEngine;
using System.Collections;

public class WoKL_Healing : SpellEffect
{
    public WoKL_Healing() : base()
    {

    }

    public override void OnCast(Caster who, Soldier target)
    {
        target.CastFinished(this, who);
    }

    public override void OnCastFinished(Caster who, Soldier target, int minval = 0, int maxval = 0)
    {
        if (target.effectSystem.FindBuff((int)Buff.DB.WORD_OF_KINGS_LOYALTY) != null)
            Spell.Cast(this, target, who, minval, maxval);
        // healing z beaconow (spelltype 4) dziala tylko na cele z buffkiem (buff 2)
    }

    public override void Execute(Caster who, Soldier target, int minval=0, int maxval=0)
    {
        SpellInfo spellInfo = GameCore.Core.spellRepository.Get(SPELL.WORD_OF_KINGS_LOYALTY_HEALING);
        target.Heal(minval, maxval, 0f, who, spellInfo, HEALSOURCE.WOK_LOYALTY, HEALTYPE.OTHER);
    }

}