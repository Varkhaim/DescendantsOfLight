using UnityEngine;
using System.Collections;

public class WordOfKingsCourage : SpellEffect
{
    public WordOfKingsCourage() : base()
    {

    }

    public override void OnCast(Caster who, Soldier target)
    {
        /*
        Troop targets = core.troopsHandler.GetTargets(TARGETTYPE.BY_HEALTH,5);
        for (int i = 0; i < targets.amount; i++)
        {
            if (targets.Soldier[i] != null)
            targets.Soldier[i].CastFinished(this, who);
        }
        */
        target.CastFinished(this, who);
        if (who.myAura[(int)AURA.GENEROUSITY].isActive)
        {
            GameCore.Core.FindSpellByName("Divine Intervention").ChangeCooldown(VALUES.GENEROUSITY_REDUCTION);
        }
        //if (!target.frame.GetComponent<AudioSource>().isPlaying)
        //    target.frame.GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("Sounds/WoKCourageSound"));
    }

    public override void OnCastFinished(Caster who, Soldier target, int minval = 0, int maxval = 0)
    {
        Spell.Cast(this, target, who);
    }

    public override void Execute(Caster who, Soldier target, int minval = 0, int maxval = 0)
    {
        int _value = 0;
        SpellInfo spellInfo = GameCore.Core.spellRepository.Get(SPELL.WORD_OF_KINGS_COURAGE);
        _value = spellInfo.baseValue;
        _value += (int)(GameCore.Core.chosenAccount.statPWR * spellInfo.coeff);

        //Object.Instantiate(Resources.Load("Visuals/WoKCourageParticle"), target.frame.transform.position + new Vector3(1.4f, -0.5f), Quaternion.Euler(270, 0, 0));

        target.Heal(_value, _value + 15, GameCore.Core.criticalStrikeChance, who, spellInfo, HEALSOURCE.WOK_COURAGE, spellInfo.healtype);

        GameObject.Instantiate(Resources.Load("Animations/lightexplosion_0"), target.frame.transform.position + new Vector3(0, 0.35f, 0), Quaternion.Euler(0, 0, 0));
    }

}