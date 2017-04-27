using UnityEngine;
using System.Collections;

public class Shadowsurge : SpellEffect
{
    public Shadowsurge() : base()
    {

    }

    public override void OnCast(Caster who, Soldier target)
    {
        foreach (Soldier _Soldier in GameCore.Core.troopsHandler.soldier)
        {
            _Soldier.CastFinished(this, who);
        }
    }

    public override void OnCastFinished(Caster who, Soldier target, int minval = 0, int maxval = 0)
    {
        if (target.effectSystem.FindBuff((int)Buff.DB.SOOTHING_VOID) != null)
            Spell.Cast(this, target, who);
    }

    public override void Execute(Caster who, Soldier target, int minval = 0, int maxval = 0)
    {
        int _value = 0;
        SpellInfo spellInfo = GameCore.Core.spellRepository.Get(SPELL.SHADOWSONG);
        _value = spellInfo.baseValue;
        _value += (int)(GameCore.Core.chosenAccount.statPWR * spellInfo.coeff);

        int _temp = 0;
        if (who.myTalentTree.GetTalentPointsByName("Peace") > 0)
        {
            if (target.health < who.myTalentTree.GetTalentPointsByName("Peace") * 25)
                _temp = 50;
        }
        target.Heal(_value, _value + 20, GameCore.Core.criticalStrikeChance + _temp, who, spellInfo, HEALSOURCE.SHADOWSURGE, spellInfo.healtype);
        if (who.myTalentTree.GetTalentByName("Ascetic") != null)
            GameCore.Core.FindSpellByName("Sacrifice").ChangeCooldown(-60);

        if (Random.Range(0, 100) >= who.myTalentTree.GetTalentByName("Shadowforce").Points * 20)
        {
            Buff myb = target.effectSystem.FindBuff((int)Buff.DB.SOOTHING_VOID);
            if (myb != null)
            {
                myb.refreshCount = 1;
                myb.duration = 0;
            }
        }
    }
}