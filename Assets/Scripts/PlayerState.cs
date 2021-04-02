using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerState : MonoBehaviour {
    public GameObject magicMissilePrefab;
    public GameObject fireballPrefab;

    private int hotbarSize;
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
        print($"MagicMissile Prefab: {magicMissilePrefab}");

        hotbar = new PlayerAbility[2];
        hotbar[0] = new MagicMissile(magicMissilePrefab);
        hotbar[1] = new Fireball(fireballPrefab);
        hotbarSize = hotbar.Length;
        
        hotbarUISlots = new GameObject[hotbarSize];
        hotbarUISlots[0] = GameObject.Find("Canvas/HotbarPanel/Slot1");
        hotbarUISlots[1] = GameObject.Find("Canvas/HotbarPanel/Slot2");

        hotbarTextures = new Texture2D[hotbarSize];
        hotbarTextures[0] = Resources.Load<Texture2D>("Images/PlayerAbility/Magic_Missile");
        hotbarTextures[1] = Resources.Load<Texture2D>("Images/PlayerAbility/Fireball");

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
            print("Changing selected item to Slot0");
            selectedHotbarSlot = 0;
        } else if (Input.GetKey(KeyCode.Alpha2)) {
            print("Changing selected item to Slot1");
            selectedHotbarSlot = 1;
        }

        UpdateUI();
    }

    private void UpdateUI() {
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
            print(hotbarTextures[i]);
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
