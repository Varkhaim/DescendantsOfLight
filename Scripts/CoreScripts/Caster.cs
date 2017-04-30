﻿using UnityEngine;
using System.Collections;

public class Caster
{
    public TalentTree myTalentTree;
    public GameCore core = GameCore.Core;
    public Aura[] myAura = new Aura[100];

    private int basestatINT = 10; // INTelligence - redukuje cooldown spelli o x%
    private int basestatKNG = 10; // KNowledGe - zwieksza regen many o x%
    private int basestatFCS = 10; // FoCuS - zwieksza szanse na krytyczne uderzenie o x%
    private int basestatPWR = 10; // PoWeR - zwieksza sile leczenia o x%
    private float critChance = 0f;

    public float HealingMultiplier()
    {
        float _value = 1f;
        BuffHandler buffSystem = core.buffSystem;
        if (buffSystem.FindBuff(CASTERBUFF.VISIONS_OF_ANCIENT_KINGS) != null)
            _value *= (1.3f + myAura[(int)AURA.VISIONS_OF_ANCIENT_KINGS].stacks*VALUES.VOAK_INCREASE);
        if (buffSystem.FindBuff(CASTERBUFF.DIVINE_INTERVENTION) != null)
            _value *= 1.5f;
        if (myTalentTree.GetTalentByName("Trauma") != null)
        {
            _value *= 1 + (core.ManaMax - core.ManaCurrent) / core.ManaMax * 0.5f;
        }
        if (myTalentTree.GetTalentByName("Book of Prime Shadows") != null)
        {
            CasterBuff myb = buffSystem.FindBuff(CASTERBUFF.BOOK_OF_PRIME_SHADOWS);
            if (myb != null)
            {
                _value *= 1 + myb.stacks * 0.01f;
            }
        }
        return _value;
    }

    public bool AuraActive(AURA which)
    {
        return myAura[(int)which].isActive;
    }
    public int AuraStacks(AURA which)
    {
        return myAura[(int)which].stacks;
    }

    public Caster(TalentTree _tree, Account _acc)
    {
        myTalentTree = _tree;
        ApplyAurasFromTree(_tree);
        CopyStats(_acc);
        RefreshStats();
    }

    private void CopyStats(Account _acc)
    {
        basestatFCS = _acc.statFCS;
        basestatINT = _acc.statINT;
        basestatKNG = _acc.statKNG;
        basestatPWR = _acc.statPWR;
    }

    public void RefreshStats()
    {
        critChance = 2 * basestatFCS / (2 * basestatFCS + 1000);
    }

    private void ApplyAurasFromTree(TalentTree _tree)
    {
        switch (_tree.classID)
        {
            case CHAMPION.PALADIN:
                {
                    myAura[(int)AURA.HAND_OF_LIGHT] = new Aura(_tree.GetTalentByName("Hand of Light"));
                    myAura[(int)AURA.IRON_FAITH] = new Aura(_tree.GetTalentByName("Iron Faith"));
                    myAura[(int)AURA.SPIRIT_BOND] = new Aura(_tree.GetTalentByName("Spirit Bond"));
                    myAura[(int)AURA.AURA_OF_LIGHT] = new Aura(_tree.GetTalentByName("Aura of Light"));
                    myAura[(int)AURA.CONSECRATION] = new Aura(_tree.GetTalentByName("Consecration"));
                    myAura[(int)AURA.ROYALTY] = new Aura(_tree.GetTalentByName("Royalty"));
                    myAura[(int)AURA.DIVINITY] = new Aura(_tree.GetTalentByName("Divinity"));
                    myAura[(int)AURA.EMPATHY] = new Aura(_tree.GetTalentByName("Empathy"));
                    myAura[(int)AURA.GENEROUSITY] = new Aura(_tree.GetTalentByName("Generousity"));
                    myAura[(int)AURA.MODESTY] = new Aura(_tree.GetTalentByName("Modesty"));
                    myAura[(int)AURA.VISIONS_OF_ANCIENT_KINGS] = new Aura(_tree.GetTalentByName("Visions of Ancient Kings"));
                    myAura[(int)AURA.GUIDANCE_OF_RAELA] = new Aura(_tree.GetTalentByName("Guidance of Raela"));
                    myAura[(int)AURA.FLASH_OF_FUTURE] = new Aura(_tree.GetTalentByName("Flash of Future"));
                } break;
            case CHAMPION.SHADOWMANCER:
                {

                }
                break;
        }
    }

    public float GetCritChance()
    {
        return critChance;
    }

    public float GetPower()
    {
        return basestatPWR;
    }
}

