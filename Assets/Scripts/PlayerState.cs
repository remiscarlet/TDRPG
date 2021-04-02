using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerState : MonoBehaviour {
    private int hotbarSize = 2;
    private int selectedHotbarSlot;

    private PlayerAbility[] hotbar;
    private GameObject[] hotbarUISlots;
    private Texture2D[] hotbarTextures;

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
        hotbarTextures = new Texture2D[hotbarSize];
        
        hotbarUISlots = new GameObject[hotbarSize];
        hotbarUISlots[0] = GameObject.Find("Canvas/HotbarPanel/Slot1");
        hotbarUISlots[1] = GameObject.Find("Canvas/HotbarPanel/Slot2");

        selectedHotbarSlot = 0;
    }

    
    private int points;
    public int Points {
        get { return points; }
        set { points = value; }
    }

    public void AddToPoints(int pointsToAdd) {
        Points += pointsToAdd;
    }

    public void Update() {
        if (Input.GetKey(KeyCode.Alpha1)) {
            selectedHotbarSlot = 0;
        } else if (Input.GetKey(KeyCode.Alpha2)) {
            selectedHotbarSlot = 1;
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
            var hotbar = hotbarUISlots[i];
            GameObject itemRawImage = hotbar.transform.Find("Item").gameObject;
            itemRawImage.GetComponent<RawImage>().texture = hotbarTextures[i];
        }

        // Draw selected hotbar border
        foreach (GameObject hotbar in hotbarUISlots) {
            hotbar.GetComponent<Image>().color = unselectedHotbarColor;
        }
        hotbarUISlots[selectedHotbarSlot].GetComponent<Image>().color = selectedHotbarColor;
    }

    public PlayerAbility GetEquippedSlotAbility() {
        return hotbar[selectedHotbarSlot];
    }
}
