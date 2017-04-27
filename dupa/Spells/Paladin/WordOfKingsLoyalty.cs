using UnityEngine;
using System.Collections;

public class WordOfKingsLoyalty : SpellEffect
{
    public WordOfKingsLoyalty() : base()
    {
    }

    public override void OnCast(Caster who, Soldier target)
    {
        target.CastFinished(this, who);
    }

    public override void OnCastFinished(Caster who, Soldier target, int minval = 0, int maxval = 0)
    {
        Spell.Cast(this, target, who);
    }

    public override void Execute(Caster who, Soldier target, int minval = 0, int maxval = 0)
    {
        SpellInfo spellInfo = GameCore.Core.spellRepository.Get(SPELL.WORD_OF_KINGS_LOYALTY);
        target.BuffMe((int)Buff.DB.WORD_OF_KINGS_LOYALTY, spellInfo.HoTgap * spellInfo.ticksCount + who.myAura[(int)AURA.CONSECRATION].stacks * VALUES.CONSECRATION_DUR_INCREASE, who, spellInfo, 0);
        int _val = who.myTalentTree.GetTalentPointsByName("Royalty");
        if (_val > 0)
            target.BuffMe((int)Buff.DB.ROYALTY, _val * 90, who, spellInfo, 0);
        if (!target.frame.GetComponent<AudioSource>().isPlaying)
            target.frame.GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("Sounds/WoKLoyaltySound"));
    }

}