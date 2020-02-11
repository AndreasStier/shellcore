﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NodeEditorFramework.Utilities;

namespace NodeEditorFramework.Standard
{
    [Node(false, "AI/Rotate Craft")]
    public class RotateCraftNode : Node
    {
        public override string GetName { get { return "RotateCraftNode"; } }
        public override string Title { get { return "Rotate Craft"; } }
        public override bool AllowRecursion { get { return true; } }
        public override bool AutoLayout { get { return true; } }

        [ConnectionKnob("Output", Direction.Out, "TaskFlow", NodeSide.Right)]
        public ConnectionKnob output;

        [ConnectionKnob("Input", Direction.In, "TaskFlow", NodeSide.Left)]
        public ConnectionKnob input;

        //public bool action; //TODO: action input
        public bool useIDInput;
        public bool useIDInputTarget;
        public bool asynchronous;
        public string entityName = "";
        public string targetEntityName = "";

        public ConnectionKnob IDInput;

        ConnectionKnobAttribute IDInStyle = new ConnectionKnobAttribute("Name Input", Direction.In, "EntityID", ConnectionCount.Single, NodeSide.Left);

        public override void NodeGUI()
        {
            GUILayout.BeginHorizontal();
            input.DisplayLayout();
            output.DisplayLayout();
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            if (useIDInput)
            {
                if (IDInput == null)
                {
                    if (inputKnobs.Count == 1)
                        IDInput = CreateConnectionKnob(IDInStyle);
                    else
                        IDInput = inputKnobs[1];
                }
                IDInput.DisplayLayout();
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Subject to rotation");
            GUILayout.EndHorizontal();
            useIDInput = RTEditorGUI.Toggle(useIDInput, "Use Name Input", GUILayout.MinWidth(400));
            if (GUI.changed)
            {
                if (useIDInput)
                    IDInput = CreateConnectionKnob(IDInStyle);
                else
                    DeleteConnectionPort(IDInput);
            }
            if (!useIDInput)
            {
                GUILayout.Label("Entity Name");
                entityName = GUILayout.TextField(entityName);
                if (WorldCreatorCursor.instance != null)
                {
                    if (GUILayout.Button("Select", GUILayout.ExpandWidth(false)))
                    {
                        WorldCreatorCursor.selectEntity += SetEntityName;
                        WorldCreatorCursor.instance.EntitySelection();
                    }
                }
            }

            GUILayout.BeginHorizontal();
            GUILayout.Label("Target for rotation");
            GUILayout.EndHorizontal();
            useIDInputTarget = RTEditorGUI.Toggle(useIDInputTarget, "Use Name Input (Unfinished, don't use)", GUILayout.MinWidth(400));
            if (GUI.changed)
            {
                if (useIDInputTarget)
                    IDInput = CreateConnectionKnob(IDInStyle);
                else
                    DeleteConnectionPort(IDInput);
            }
            if (!useIDInputTarget)
            {
                GUILayout.Label("Entity Name");
                targetEntityName = GUILayout.TextField(targetEntityName);
                if (WorldCreatorCursor.instance != null)
                {
                    if (GUILayout.Button("Select", GUILayout.ExpandWidth(false)))
                    {
                        WorldCreatorCursor.selectEntity += SetTargetName;
                        WorldCreatorCursor.instance.EntitySelection();
                    }
                }
            }

            asynchronous = RTEditorGUI.Toggle(asynchronous, "Asynchronous Mode (Unfinished, don't select)", GUILayout.MinWidth(400));

            RTEditorGUI.Seperator();
        }

        void SetEntityName(string newName)
        {
            Debug.Log("selected " + newName + "!");

            entityName = newName;
            WorldCreatorCursor.selectEntity -= SetEntityName;
        }

        void SetTargetName(string newName)
        {
            Debug.Log("selected " + newName + "!");

            targetEntityName = newName;
            WorldCreatorCursor.selectEntity -= SetTargetName;
        }

        AirCraft entity = null;
        Entity target = null;
        public override int Traverse()
        {
            if (useIDInput)
            {
                if (useIDInput && IDInput == null)
                    IDInput = inputKnobs[1];

                if (IDInput.connected())
                {
                    entityName = (IDInput.connections[0].body as SpawnEntityNode).entityName;
                }
                else
                {
                    Debug.LogWarning("Name Input not connected!");
                }
            }

            Debug.Log("Entity name: " + entityName);
            Debug.Log("Target name: " +targetEntityName);

            if(!(target && entity)) // room for improvement but probably unecessary
            for (int i = 0; i < AIData.entities.Count; i++)
            {
                if (AIData.entities[i].name == entityName && AIData.entities[i] is AirCraft)
                    entity = AIData.entities[i] as AirCraft;
                if (AIData.entities[i].name == targetEntityName && AIData.entities[i] is AirCraft)
                    target = AIData.entities[i];
            }

            if(!(target && entity))
            {
                Debug.LogWarning("Could not find target/entity! " + target + " " + entity);
                return 0;
            }

            if(!asynchronous)
            {
                Vector2 targetVector = target.transform.position - entity.transform.position; 
                //calculate difference of angles and compare them to find the correct turning direction
                entity.GetAI().RotateTo(targetVector);   
            }

            return 0;
        }
    }
}