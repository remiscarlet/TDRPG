using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerState : MonoBehaviour {
    private int hotbarSize = 4;
    private int selectedHotbarSlot;

    private PlayerAbility[] hotbar;
    private GameObject[] hotbarUISlots;

    private SpawnManager spawnManager;
    private TextMeshProUGUI scoreTextMesh;
    private TextMeshProUGUI waveTextMesh;
    public void Start() {
        spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        scoreTextMesh = GameObject.Find("Canvas/TopPanel/ScoreText").GetComponent<TextMeshProUGUI>();
        waveTextMesh = GameObject.Find("Canvas/TopPanel/WaveText").GetComponent<TextMeshProUGUI>();

        print("Initializing PlayerState");

        // Make hotbar class
        hotbar = new PlayerAbility[hotbarSize];
        
        hotbarUISlots = new GameObject[hotbarSize];
        hotbarUISlots[0] = GameObject.Find("Canvas/HotbarPanel/Slot1");
        hotbarUISlots[1] = GameObject.Find("Canvas/HotbarPanel/Slot2");
        hotbarUISlots[2] = GameObject.Find("Canvas/HotbarPanel/Slot3");
        hotbarUISlots[3] = GameObject.Find("Canvas/HotbarPanel/Slot4");

        selectedHotbarSlot = 0;

        Points = 500;
    }

    
    private int points;
    public int Points {
        get { return points; }
        set { points = value; }
    }

    public void AddPoints(int pointsToAdd) {
        Points += pointsToAdd;
    }

    public void DeductPoints(int pointsToDeduct) {
        Points -= pointsToDeduct;
    }

    public bool CanAffordPurchase(int costToPurchase) {
        return Points > costToPurchase;
    }

    public void Update() {
        if (Input.GetKey(KeyCode.Alpha1)) {
            selectedHotbarSlot = 0;
        } else if (Input.GetKey(KeyCode.Alpha2)) {
            selectedHotbarSlot = 1;
        } else if (Input.GetKey(KeyCode.Alpha3)) {
            selectedHotbarSlot = 2;
        } else if (Input.GetKey(KeyCode.Alpha4)) {
            selectedHotbarSlot = 3;
        }

        DrawUI();
    }

    private void DrawUI() {
        scoreTextMesh.text = $"Points: {Points}";
        waveTextMesh.text = $"Wave: {spawnManager.WaveNum}";
  
        DrawHotbar();
    }

    private Color selectedHotbarColor = new Color(0.0f, 0.0f, 0.0f, 255.0f);
    private Color unselectedHotbarColor = new Color(255.0f, 255.0f, 255.0f, 255.0f);
    private void DrawHotbar() {
        // Draw hotbar items
        for (int i = 0; i < hotbarSize; i++) {
            PlayerAbility hotbarAbility = hotbar[i];
            if (hotbarAbility == null) {
                continue;
            }

            var hotbarUISlot = hotbarUISlots[i];
            GameObject itemRawImage = hotbarUISlot.transform.Find("Item").gameObject;
            itemRawImage.GetComponent<RawImage>().texture = hotbarAbility.IconTex;
        }

        // Draw selected hotbar border
        foreach (GameObject hotbar in hotbarUISlots) {
            hotbar.GetComponent<Image>().color = unselectedHotbarColor;
        }
        hotbarUISlots[selectedHotbarSlot].GetComponent<Image>().color = selectedHotbarColor;
    }

    public bool AddNewAbilityToHotbar(PlayerAbility ability) {
        int hotbarSlotToInsertInto = GetLowestUnoccupiedHotbarSlot();
        if (hotbarSlotToInsertInto == -1) {
            // Hotbar full. Failed to insert.
            return false;
        }

        hotbar[hotbarSlotToInsertInto] = ability;
        return true;
    }

    private int GetLowestUnoccupiedHotbarSlot() {
        for (int i = 0; i < hotbarSize; i++) {
            if (hotbar[i] == null) {
                return i;
            }
        }
        return -1;
    }

    public PlayerAbility GetEquippedSlotAbility() {
        return hotbar[selectedHotbarSlot];
    }
}
