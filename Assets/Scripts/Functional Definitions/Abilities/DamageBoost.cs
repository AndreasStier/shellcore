﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Temporarily increases the craft's damage multiplier
/// </summary>
public class DamageBoost: ActiveAbility, IBlinkOnUse
{
    bool activated = false;
    protected override void Awake()
    {
        base.Awake(); // base awake
                      // hardcoded values here
        ID = AbilityID.DamageBoost;
        cooldownDuration = 20;
        CDRemaining = cooldownDuration;
        activeDuration = 5;
        activeTimeRemaining = activeDuration;
        energyCost = 200;
    }

    /// <summary>
    /// Returns the engine power to the original value
    /// </summary>
    public override void Deactivate()
    {
        if(Core && activated)
        {
            Core.damageAddition -= 150;
        }
            
        base.Deactivate();
    }



    public override void Tick(int key)
    {
        base.Tick(key);
    }

    /// <summary>
    /// Increases core engine power to speed up the core
    /// </summary>
    protected override void Execute()
    {
        if (Core)
        {
            Core.damageAddition += 150;
            activated = true;
        }

        AudioManager.PlayClipByID("clip_buff", transform.position);
        isOnCD = true; // set to on cooldown
        isActive = true; // set to "active"
        base.Execute();
    }
}
