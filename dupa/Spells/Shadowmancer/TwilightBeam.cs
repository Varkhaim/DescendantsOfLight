using UnityEngine;
using System.Collections;

public class TwilightBeam : SpellEffect
{
    public TwilightBeam() : base()
    {

    }

    public override void OnCast(Caster who, Soldier target)
    {
        Talent _myTalent = who.myTalentTree.GetTalentByName("Awakening");
        if (_myTalent.Points > 0)
        {
            GameCore.Core.FindSpellByName("Shadowsong").ChangeCooldown(-_myTalent.Points*30f);
        }
        target.CastFinished(this, who);
    }

    public override void OnCastFinished(Caster who, Soldier target, int minval = 0, int maxval = 0)
    {
        Spell.Cast(this, target, who);
    }

    public override void Execute(Caster who, Soldier target, int minval = 0, int maxval = 0)
    {
        SpellInfo spellInfo = GameCore.Core.spellRepository.Get(SPELL.TWILIGHT_BEAM);
        int _tempgap = spellInfo.HoTgap - who.myTalentTree.GetTalentPointsByName("Dawn") * 6;
        int _dur = spellInfo.ticksCount * _tempgap;
        target.BuffMe((int)Buff.DB.TWILIGHT_BEAM, _dur, who, spellInfo, _tempgap);
        int _points = who.myTalentTree.GetTalentPointsByName("Dream");
        if (_points > 0)
        {
            Buff myb = target.effectSystem.FindBuff((int)Buff.DB.SOOTHING_VOID);
            if (myb != null)
            {
                myb.refreshCount = 1;
                myb.duration = 0;

                target.Shield((int)(spellInfo.baseValue2 * _points * 0.75f), HEALSOURCE.DREAM);
            }
        }
    }

}