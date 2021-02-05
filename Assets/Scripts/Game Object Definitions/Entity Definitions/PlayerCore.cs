﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// A player ShellCore.
/// </summary>
public class PlayerCore : ShellCore {
    public HUDScript hud;
    public InfoText alerter;
    public PlayerSave cursave;
    public bool loaded;
    private bool isInteracting;
    public int credits;
    public int shards;
    public int[] abilityCaps;
    public int reputation;
    public static PlayerCore Instance;

    public AbilityHandler GetAbilityHandler() {
        return GameObject.Find("AbilityUI").GetComponent<AbilityHandler>();
    }
    public bool GetIsInteracting() {
        return isInteracting;
    }

    public void SetIsInteracting(bool val) {
        isInteracting = val;
    }
    /// <summary>
    /// Respawns the player core, deinitializes the HUD
    /// </summary>
    public override void Respawn() {
        List<bool> weaponActivationStates = new List<bool>();
        for (int i = 0; i < abilities.Count; i++)
        {
            if (abilities[i] is WeaponAbility)
                weaponActivationStates.Add((abilities[i] as WeaponAbility).GetActiveTimeRemaining() == -1);
        }
        if(hud) hud.DeinitializeHUD(); // deinitialize HUD
        transform.position = spawnPoint; // reset position to spawn point
        base.Respawn(); // this will reinitialize the HUD
        int weaponIndex = 0;
        for (int i = 0; i < abilities.Count; i++)
        {
            if (abilities[i] is WeaponAbility)
                (abilities[i] as WeaponAbility).SetActive(weaponActivationStates[weaponIndex++]);
        }
    }
    /// <summary>
    /// The directional driver for the player core, returns a vector based on current inputs
    /// </summary>
    /// <returns>a directional vector based on current inputs</returns>
    public static Vector2 getDirectionalInput()
    {
        //Sum up all inputs
        Vector2 direction = Vector2.zero;
        if (InputManager.GetKey(KeyName.Up))
            direction += new Vector2(0, 1);
        if (InputManager.GetKey(KeyName.Left))
            direction += new Vector2(-1, 0);
        if (InputManager.GetKey(KeyName.Down))
            direction += new Vector2(0, -1);
        if (InputManager.GetKey(KeyName.Right))
            direction += new Vector2(1, 0);

        //Send unit vector
        direction.Normalize();

        return direction; // it's not exactly like it was in the original game, but I like it more like this actually
    }

    protected override void Awake()
    {
        Instance = this;
        name = entityName = "player";
        if (!initialized)
            base.Awake();
        ID = "player";
    }
    // Use this for initialization (overrides the other start methods so is always called even by parent method calls)
    protected override void Start () {
        // initialize instance fields
        base.Start();

        if (hud) hud.InitializeHUD(this);
        else
        {
            Camera.main.GetComponent<CameraScript>().Initialize(this);
            GameObject.Find("AbilityUI").GetComponent<AbilityHandler>().Initialize(this);
        } // initialize the HUD
        if(!loaded) {
            LoadSave(cursave);
            loaded = true;
        }

        // force sectors to load once positioning has been determined
        SectorManager.instance.AttemptSectorLoad();

        // the player needs a predictable name for task interactions, so its object will always be called this
        name = entityName = "player";
	}

    public override void Rebuild() {
        if (!initialized)
            Awake();
        initialized = true;
        hud.DeinitializeHUD();
        for(int i = 0; i < parts.Count; i++) {
            if(parts[i].gameObject.name != "Shell Sprite")
                Destroy(parts[i].gameObject);
        }
        // UnityEditor.AssetDatabase.CreateAsset(blueprint, "Assets/Core Upgrades.asset");
        BuildEntity();
        
        // the player needs a predictable name for task interactions, so its object will always be called this
        name = entityName = "player";
        hud.InitializeHUD(this);
    }

    public void LoadSave(PlayerSave save)
    {
        if(save.timePlayed != 0) 
        {
            if(save.characters != null && save.characters.Length != 0)
            {
                // use the save's characters combined with any new characters in the Sector Manager character set
                var sectoManagerChars = new List<WorldData.CharacterData>(SectorManager.instance.characters);
                var newChars = new List<WorldData.CharacterData>(save.characters);
                foreach(var ch in sectoManagerChars)
                {
                    if(newChars.TrueForAll(c => c.ID != ch.ID))
                    {
                        newChars.Add(ch);
                    }
                }
                SectorManager.instance.characters = newChars.ToArray();
            }
            transform.position = save.position;
        }

        name = entityName = "player";
        positionBeforeOscillation = transform.position.y;
        if(save.currentHealths.Length < 3)
        {
            maxHealth.CopyTo(currentHealth, 0);
        }
        else
        {
            currentHealth = save.currentHealths;
        }
        for(int i = 0; i < currentHealth.Length; i++) {
            if(currentHealth[i] > maxHealth[i]) currentHealth[i] = maxHealth[i];
        }
    }
    public List<EntityBlueprint.PartInfo> GetInventory() {
        if(cursave != null) return cursave.partInventory;
        else return null; 
    }
    
	// Update is called once per frame
	protected override void Update () {
        isImmobile = isDead || isAbsorbing;
        // call methods
        if(group.sortingOrder < maxAirLayer) // player must always be above other entities
        {
            group.sortingOrder = ++maxAirLayer;
        }
        base.Update(); // base update
        if(!isInteracting && !DialogueSystem.isInCutscene && !DevConsoleScript.componentEnabled) MoveCraft(getDirectionalInput()); // move the craft based on the directional input
	}

    public override void Warp(Vector3 point)
    {
        base.Warp(point);
        CameraScript.instance.Focus(transform.position);
        foreach(var instance in RectangleEffectScript.instances)
        {
            instance.Start();
        }
        SectorManager.instance.AttemptSectorLoad();
    }

    protected override void CraftMover(Vector2 directionVector)
    {
        base.CraftMover(directionVector);
        if(directionVector != Vector2.zero) CameraScript.instance.Focus(transform.position);
    }
}
