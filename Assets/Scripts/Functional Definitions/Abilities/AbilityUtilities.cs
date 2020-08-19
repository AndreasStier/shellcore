﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum AbilityID
{
	Unused,
	SpeedThrust,
	ShellBoost,
	MainBullet,
	Beam,
	Bullet,
	Cannon,
	Missile,
	Torpedo,
	Laser,
	SpawnDrone,
	CoreHeal,
	Energy,
	Speed,
	SiegeBullet,
	SpeederBullet,
	Harvester,
	ShellRegen,
	ShellMax,
	EnergyRegen,
	EnergyMax,
	Command,
	CoreRegen,
	CoreMax,
	Stealth,
	DamageBoost,
	AreaRestore,
	PinDown,
	Retreat,
	Absorb,
	ActiveShellRegen,
	ActiveCoreRegen,
	ActiveEnergyRegen,
	Disrupt,
	Control
}
public class AbilityUtilities : MonoBehaviour {

	public static Sprite GetAbilityImageByID(int ID, string secondaryData) {
		if(ID == 0) return null;
		if(ID == 10) {
            DroneSpawnData data = GetDroneSpawnData(secondaryData);
			return DroneUtilities.GetAbilitySpriteBySpawnData(data);
		}
		return ResourceManager.GetAsset<Sprite>("AbilitySprite" + ID);
	}

	public static Sprite GetAbilityImage(Ability ability) {
		var ID = ability.GetID();
		if(ID == 0) return null;
		if(ID == 10) {
			return DroneUtilities.GetAbilitySpriteBySpawnData((ability as SpawnDrone).spawnData);
		}
		return ResourceManager.GetAsset<Sprite>("AbilitySprite" + ID);
	}

	public static AbilityHandler.AbilityTypes GetAbilityTypeByID(int ID) {
		switch(ID) {
			case 10:
				return AbilityHandler.AbilityTypes.Spawns;
			case 3:
			case 4:
			case 5:
			case 6:
			case 7:
			case 8:
			case 9:
				return AbilityHandler.AbilityTypes.Weapons;
			case 1:
			case 2:
			case 11:
			case 12:
            case 24:
            case 25:
            case 26:
            case 27:
            case 28:
            case 29:
            case 30:
            case 31:
            case 32:
            case 33:
                return AbilityHandler.AbilityTypes.Skills;
			case 13:
			case 17:
			case 18:
			case 19:
			case 20:
			case 21:
			case 34:
                return AbilityHandler.AbilityTypes.Passive;
			case 0:
			default:
				return AbilityHandler.AbilityTypes.None;
		}
		
	}
	public static string GetDescriptionByID(int ID, int tier, string secondaryData) {
		switch(ID) {
			case 0:
				return "Does nothing.";
			case 1:
				return "Temporarily increases speed.";
			case 2:
				return "Instantly heal " + HealthHeal.heals[0] * tier + " shell.";
			case 3:
				return "Projectile that deals " + MainBullet.mbDamage + " damage. \nStays with you no matter what.";
			case 4:
				return "Instant attack that deals " + Beam.beamDamage * tier + " damage.";
			case 5:
				return "Projectile that deals " + Bullet.bulletDamage * tier + " damage.";
			case 6:
				return "Instant attack that deals " + Cannon.cannonDamage * tier + " damage.";
			case 7:
				return "Slow homing projectile that deals " + Missile.missileDamage * tier + " damage.";
			case 8:
				return "Slow projectile that deals " + Torpedo.torpedoDamage * tier + " damage to ground entities.";
			case 9:
				return "Fast projectile that deals " + Laser.laserDamage * tier + " damage. 18% pierces to core.";
			case 10:
				if(secondaryData == null || secondaryData == "") return "Spawns a drone.";
                DroneSpawnData data = GetDroneSpawnData(secondaryData);
				return DroneUtilities.GetDescriptionByType(data.type);
			case 11:
				return "Instantly heal " + HealthHeal.heals[1] * tier + " core.";
			case 12:
				return "Instantly heal " + HealthHeal.heals[2] * tier + " energy.";
			case 13:
				return "Passively increases speed.";
			case 17:
				return "Passively increases shell regen by "  + ShellRegen.regen * tier + " points.";
			case 18:
				return "Passively increases maximum shell by " + ShellMax.max * tier + " points.";
			case 19:
				return "Passively increases energy regen by " +  ShellRegen.regen * tier + " points.";
			case 20:
				return "Passively increases maximum energy by " + ShellMax.max * tier + " points.";
			case 21:
				return "Passively increases the maximum allowed number of controlled units by " + Command.commandUnitIncrease + ".";
            case 24:
                return "Become invisible to enemies.";
            case 25:
                return "All weapon damage increased by 150.";
            case 26:
                return "Instantly heals self and nearby allies by +500 shell and +500 core.";
            case 27:
                return "Immobilizes the target.";
            case 28:
                return "Respawn at base.";
            case 29:
                return "Absorb damage and turn it into energy.";
            case 30:
                return "Temporarily increase shell regen.";
            case 31:
                return "Temporarily increase core... wait, this isn't supposed to exist!";
            case 32:
                return "Temporarily increase energy regen.";
            case 33:
                return "Disrupt enemy ability cooldowns.";
            case 34:
                return "Makes allies stronger.";
            default:
				return "Description unset";
		}
	}

	public static string GetDescription(Ability ability) {
		switch(ability.GetID()) {
			case 10:
				return DroneUtilities.GetDescriptionByType((ability as SpawnDrone).spawnData.type);
			default:
				return GetDescriptionByID(ability.GetID(), ability.GetTier(), "");
		}
	}
	public static string GetShooterByID(int ID, string data = null) {
		switch(ID) {
			case 0:
			case 13:
			case 17:
			case 18:
			case 19:
			case 20:
			case 21:
			case 34:
                return null;
			case 4:
				return "beamshooter_sprite";
			case 5:
			case 14:
			case 15:
				return "bulletshooter_sprite";
			case 6:
				return "cannonshooter_sprite";
			case 7:
			if(data != "missile_station_shooter")
				return "missileshooter_sprite";
			else return "missile_station_shooter";
			case 8:
				return "torpedoshooter_sprite";
			case 9:
				return "lasershooter_sprite";
			default:
				return "ability_indicator";
		}
	}
	public static string GetAbilityNameByID(int ID, string secondaryData) {
        switch(ID) {
			case 0:
				return "None";
            case 1:
                return "Speed Thrust";
            case 2:
                return "Shell Boost";
            case 3:
                return "Main Bullet";
            case 4:
                return "Beam";
            case 5:
                return "Bullet";
            case 6:
                return "Cannon";
            case 7:
                return "Missile";
            case 8:
                return "Torpedo";
            case 9:
                return "Laser";
            case 10:
				if(secondaryData == null) return "Spawn Drone";
                DroneSpawnData data = GetDroneSpawnData(secondaryData);
				return DroneUtilities.GetAbilityNameByType(data.type);
            case 11:
                return "Core Heal";
            case 12:
                return "Energy";
            case 13:
                return "Speed";
			case 17:
				return "Shell Regen";
			case 18:
				return "Shell Max";
			case 19:
				return "Energy Regen";
			case 20:
				return "Energy Max";
			case 21:
				return "Command";
			case 22:
				return "Core Regen";
			case 23:
				return "Core Max";
            case 24:
                return "Stealth";
            case 25:
                return "Damage Boost";
            case 26:
                return "Area Restore";
            case 27:
                return "Pin Down";
            case 28:
                return "Retreat";
            case 29:
                return "Absorption";
            case 30:
                return "Shell Regen";
            case 31:
                return "Core Regen";
            case 32:
                return "Energy Regen";
            case 33:
                return "Disrupt";
            case 34:
                return "Control";
            default:
                return "Name unset";
        }
    }
	public static string GetAbilityName(Ability ability) {
        switch(ability.GetID()) {
            case 10:
				return DroneUtilities.GetAbilityNameByType((ability as SpawnDrone).spawnData.type);
            default:
                return GetAbilityNameByID(ability.GetID(), "");
        }
    }
	public static Ability AddAbilityToGameObjectByID(GameObject obj, int ID, string data = null, int tier = 0) {
		Ability ability = null;
		switch(ID) {
			case 1:
				ability = obj.AddComponent<SpeedThrust>();
				break;
			case 2:
				ability = obj.AddComponent<HealthHeal>();
				((HealthHeal)ability).type = HealthHeal.HealingType.shell;
				((HealthHeal)ability).Initialize();
				break;
			case 3:
				Debug.Log("Main bullet should be intrinsically added!");
				ability = obj.AddComponent<MainBullet>();
				break;
			case 4:
				ability = obj.AddComponent<Beam>();
				break;
			case 5:
				ability = obj.AddComponent<Bullet>();
				break;
			case 6:
				ability = obj.AddComponent<Cannon>();
				break;
			case 7:
				ability = obj.AddComponent<Missile>();
				if(data == "missile_station_shooter") {
					((Missile)ability).category = Entity.EntityCategory.All;
					((Missile)ability).terrain = Entity.TerrainType.All;
				}
				break;
			case 8:
				ability = obj.AddComponent<Torpedo>();
				break;
			case 9:
				ability = obj.AddComponent<Laser>();
				break;
			case 10:
				ability = obj.AddComponent<SpawnDrone>();
                ((SpawnDrone)ability).spawnData = GetDroneSpawnData(data);
                ((SpawnDrone)ability).Init();
				break;
			case 11:
				ability = obj.AddComponent<HealthHeal>();
				((HealthHeal)ability).type = HealthHeal.HealingType.core;
				((HealthHeal)ability).Initialize();
				break;
			case 12:
				ability = obj.AddComponent<HealthHeal>();
				((HealthHeal)ability).type = HealthHeal.HealingType.energy;
				((HealthHeal)ability).Initialize();
				break;
			case 13:
				ability = obj.AddComponent<Speed>();
				break;
			case 14:
				ability = obj.AddComponent<SiegeBullet>();
				break;
			case 15:
				ability = obj.AddComponent<SpeederBullet>();
				break;
			case 16:
				ability = obj.AddComponent<Harvester>();
				break;
			case 17:
				ability = obj.AddComponent<ShellRegen>();
				(ability as ShellRegen).index = 0;
				(ability as ShellRegen).Initialize();
				break;
			case 18:
				ability = obj.AddComponent<ShellMax>();
				(ability as ShellMax).index = 0;
				(ability as ShellMax).Initialize();
				break;
			case 19:
				ability = obj.AddComponent<ShellRegen>();
				(ability as ShellRegen).index = 2;
				(ability as ShellRegen).Initialize();
				break;
			case 20:
				ability = obj.AddComponent<ShellMax>();
				(ability as ShellMax).index = 2;
				(ability as ShellMax).Initialize();
				break;
			case 21:
				ability = obj.AddComponent<Command>();
				break;
            case 24:
                ability = obj.AddComponent<Stealth>();
                break;
            case 25:
                ability = obj.AddComponent<DamageBoost>();
                break;
            case 26:
                ability = obj.AddComponent<AreaRestore>();
                break;
            case 27:
                ability = obj.AddComponent<PinDown>();
                break;
            case 28:
                ability = obj.AddComponent<Retreat>();
                break;
            case 29:
                ability = obj.AddComponent<AbsorptionField>();
                break;
            case 30:
                ability = obj.AddComponent<ActiveRegen>();
                (ability as ActiveRegen).index = 0;
                (ability as ActiveRegen).Initialize();
                break;
            case 31:
                ability = obj.AddComponent<ActiveRegen>();
                (ability as ActiveRegen).index = 1;
                (ability as ActiveRegen).Initialize();
                break;
            case 32:
                ability = obj.AddComponent<ActiveRegen>();
                (ability as ActiveRegen).index = 2;
                (ability as ActiveRegen).Initialize();
                break;
            case 33:
                ability = obj.AddComponent<Disrupt>();
                break;
            case 34:
                ability = obj.AddComponent<Control>();
                break;
        }
		if(ability) ability.SetTier(tier);
		return ability;
	}

    static DroneSpawnData GetDroneSpawnData(string secondaryData)
    {

        switch (secondaryData)
        {
            case "mini_drone":
                return ResourceManager.GetAsset<DroneSpawnData>("mini_drone_spawn");
            case "counter_drone":
                return ResourceManager.GetAsset<DroneSpawnData>("counter_drone_spawn");
            case "light_drone":
                return ResourceManager.GetAsset<DroneSpawnData>("light_drone_spawn");
            case "strike_drone":
                return ResourceManager.GetAsset<DroneSpawnData>("strike_drone_spawn");
            case "gun_drone":
                return ResourceManager.GetAsset<DroneSpawnData>("gun_drone_spawn");
            case "heavy_drone":
                return ResourceManager.GetAsset<DroneSpawnData>("heavy_drone_spawn");
            case "torpedo_drone":
                return ResourceManager.GetAsset<DroneSpawnData>("torpedo_drone_spawn");
            case "worker_drone":
                return ResourceManager.GetAsset<DroneSpawnData>("worker_drone_spawn");
            default:
                var spawnData = ScriptableObject.CreateInstance<DroneSpawnData>();
                JsonUtility.FromJsonOverwrite(secondaryData, spawnData);
                return spawnData;
        }
    }
}