﻿using NodeEditorFramework.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NodeEditorFramework.Standard
{
    [Node(false, "Dialogue/Start Dialogue")]
    public class StartDialogueNode : Node
    {
        public static StartDialogueNode dialogueStartNode;

        public override string GetName { get { return "StartDialogueNode"; } }
        public override string Title { get { return "Start Dialogue"; } }

        public override bool AutoLayout { get { return true; } }
        public override bool AllowRecursion { get { return true; } }

        [ConnectionKnob("Input Left", Direction.In, "TaskFlow", NodeSide.Left)]
        public ConnectionKnob input;

        [ConnectionKnob("Output", Direction.Out, "Dialogue", ConnectionCount.Single, NodeSide.Right)]
        public ConnectionKnob output;

        public bool SpeakToEntity;
        public string EntityID;
        public bool forceStart;
        public string originSector;
        public ConnectionKnob flowOutput;
        ConnectionKnobAttribute flowInStyle = new ConnectionKnobAttribute("ContinueAsync", Direction.Out, "TaskFlow", ConnectionCount.Single, NodeSide.Right, 20);
        public bool allowAfterSpeaking;

        public override void NodeGUI()
        {
            SpeakToEntity = RTEditorGUI.Toggle(SpeakToEntity, "Speak to entity");
            if(SpeakToEntity)
            {
                GUILayout.Label("Entity ID");
                EntityID = GUILayout.TextField(EntityID);
                if (WorldCreatorCursor.instance != null)
                {
                    if (GUILayout.Button("Select", GUILayout.ExpandWidth(false)))
                    {
                        WorldCreatorCursor.selectEntity += SetEntityID;
                        WorldCreatorCursor.instance.EntitySelection();
                    }
                }

                forceStart = RTEditorGUI.Toggle(forceStart, "Force dialogue start");
                allowAfterSpeaking = RTEditorGUI.Toggle(allowAfterSpeaking, "Allow passing async");

                if (GUI.changed)
                {
                if (allowAfterSpeaking)
                    flowOutput = CreateConnectionKnob(flowInStyle);
                else
                    DeleteConnectionPort(flowOutput);
                }
            }
        }

        public override int Traverse()
        {
            dialogueStartNode = this;
            if (SpeakToEntity)
            {
                TaskManager.speakerID = EntityID;
                TryAddObjective();
                if (TaskManager.interactionOverrides.ContainsKey(EntityID))
                {
                    TaskManager.interactionOverrides[EntityID] = () => {
                        TaskManager.Instance.setNode(output);
                    };

                }
                else
                {
                    TaskManager.interactionOverrides.Add(EntityID, () => {
                        TaskManager.Instance.setNode(output);
                    });
                }

                if(!allowAfterSpeaking)
                    return forceStart ? 0 : -1;
                else
                {
                    if(flowOutput == null)
                        flowOutput = outputKnobs[1];
                    TaskManager.Instance.setNode(flowOutput);
                    Debug.Log(flowOutput.name);
                    return 0;
                }
            }
            else
            {
                TaskManager.speakerID = null;
                return 0;
            }
        }

        void SetEntityID(string newID)
        {
            Debug.Log("selected " + newID + "!");

            EntityID = newID;
            WorldCreatorCursor.selectEntity -= SetEntityID;
        }

        void TryAddObjective()
        {
            foreach(var ent in AIData.entities)
            {
                if(!ent) continue;
                if(ent.ID == EntityID)
                {
                    TaskManager.objectiveLocations.Clear();
                    TaskManager.objectiveLocations.Add(new TaskManager.ObjectiveLocation(
                        ent.transform.position,
                        true,
                        ent
                    ));
                    TaskManager.DrawObjectiveLocations();
                    break;
                }
            }
        }
    }
}
