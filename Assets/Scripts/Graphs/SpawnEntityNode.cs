﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NodeEditorFramework.Standard
{
    [Node(false, "Actions/Spawn Entity")]
    public class SpawnEntityNode : Node
    {
        public override string GetName { get { return "SpawnEntityNode"; } }
        public override string Title { get { return "Spawn Entity"; } }

        public override Vector2 DefaultSize { get { return new Vector2(200, 240); } }

        [ConnectionKnob("Output", Direction.Out, "TaskFlow", NodeSide.Right)]
        public ConnectionKnob output;

        [ConnectionKnob("Name Out", Direction.Out, "EntityID", NodeSide.Right)]
        public ConnectionKnob IDOut;

        [ConnectionKnob("Input", Direction.In, "TaskFlow", NodeSide.Left)]
        public ConnectionKnob input;

        public bool action; //TODO: copy & paste action mode
        public string blueprintID;
        public string entityName;
        public int faction;
        public string flagName;
        public Vector2 coordinates;
        public bool useCoordinates;

        public override void NodeGUI()
        {
            GUILayout.BeginHorizontal();
            input.DisplayLayout();
            output.DisplayLayout();
            GUILayout.EndHorizontal();
            GUILayout.Label("Blueprint ID:");
            blueprintID = GUILayout.TextField(blueprintID);
            GUILayout.Label("Entity Name:");
            entityName = GUILayout.TextField(entityName);
            GUILayout.Label("Faction number:");
            faction = Utilities.RTEditorGUI.IntField(faction);

            if (useCoordinates = Utilities.RTEditorGUI.Toggle(useCoordinates, "Use coordinates"))
            {
                GUILayout.Label("Coordinates:");
                float x = coordinates.x, y = coordinates.y;
                GUILayout.BeginHorizontal();
                x = Utilities.RTEditorGUI.FloatField("X", x);
                y = Utilities.RTEditorGUI.FloatField("Y", y);
                coordinates = new Vector2(x, y);
                GUILayout.EndHorizontal();
            }
            else
            {
                GUILayout.Label("Flag Name:");
                flagName = GUILayout.TextField(flagName);
            }
        }

        public override int Traverse()
        {
            Vector2 coords = coordinates;
            if(!useCoordinates)
            {
                for (int i = 0; i < AIData.flags.Count; i++)
                {
                    if (AIData.flags[i].name == flagName)
                    {
                        coords = AIData.flags[i].transform.position;
                        break;
                    }
                }
            }
            var blueprint = ResourceManager.GetAsset<EntityBlueprint>(blueprintID);
            if (blueprint)
            {
                Sector.LevelEntity entityData = new Sector.LevelEntity
                {
                    faction = faction,
                    name = entityName,
                    position = coords,
                    ID = "",
                };
                var entity = SectorManager.instance.SpawnEntity(blueprint, entityData);
                entity.name = entityName;
            }
            else
            {
                Debug.LogWarning("Blueprint not found!");
            }
            return 0;
        }
    }
}