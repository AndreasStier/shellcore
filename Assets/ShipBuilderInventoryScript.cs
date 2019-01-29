﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; // Required when using Event data.
using UnityEngine.SceneManagement;



public class ShipBuilderInventoryScript : MonoBehaviour, IPointerDownHandler {
    public EntityBlueprint.PartInfo part;
    public GameObject SBPrefab;
    public ShipBuilderCursorScript cursor;
    int count;
    Image image;

    /*
        Some abilities cannot be differentiated just by the AbilityType enum (Like Drone Spawns) 
        and hence the only way to specify which ability perfectly is by using their ID
     */
    void Start() {
        image = GetComponentsInChildren<Image>()[1];
        image.sprite = ResourceManager.GetAsset<Sprite>(part.partID + "_sprite");
        string shooterID = AbilityUtilities.GetShooterByID(part.abilityID);
        if(shooterID != null) {
            GetComponentsInChildren<Image>()[2].sprite = ResourceManager.GetAsset<Sprite>(shooterID);
            GetComponentsInChildren<Image>()[2].color = FactionColors.colors[0];
            GetComponentsInChildren<Image>()[2].rectTransform.sizeDelta = GetComponentsInChildren<Image>()[2].sprite.bounds.size * 100;
        } else GetComponentsInChildren<Image>()[2].enabled = false;
        image.color = FactionColors.colors[0];
        image.GetComponent<RectTransform>().sizeDelta = image.sprite.bounds.size * 100;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if(count > 0) {
            var x = Instantiate(SBPrefab, cursor.transform.parent);
            x.GetComponent<ShipBuilderPart>().info = part;
            x.GetComponent<ShipBuilderPart>().cursorScript = cursor;
            cursor.parts.Add(x.GetComponent<ShipBuilderPart>());
            cursor.GrabPart(x.GetComponent<ShipBuilderPart>());
            count--;
        }
    }
    public void IncrementCount() {
        count++;
    }

    public void DecrementCount() {
        count--;
    }
    public int GetCount() {
        return count;
    }
    void Update() {
        image.color = count > 0 ? FactionColors.colors[0] : Color.gray; // gray gray gray gray gray USA USA USA USA USA
        if(GetComponentsInChildren<Image>().Length > 1) GetComponentsInChildren<Image>()[2].color = count > 0 ? FactionColors.colors[0] : Color.gray;
    }
}