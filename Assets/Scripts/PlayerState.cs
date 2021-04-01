using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerState : MonoBehaviour {

    private int points;
    public int Points {
        get { return points; }
        set { points = value; }
    }

    public void AddToPoints(int pointsToAdd) {
        Points += pointsToAdd;
    }


    private TextMeshProUGUI scoreTextMesh;
    public void Start() {
        scoreTextMesh =  GameObject.Find("Canvas/ScoreText").GetComponent<TextMeshProUGUI>();

        print("Initializing PlayerState");
        print($"MagicMissile Prefab: {magicMissilePrefab}");

        hotbar = new PlayerAbility[6];
        hotbar[0] = new MagicMissile(magicMissilePrefab);
        hotbar[1] = new Fireball(fireballPrefab);

        hotbarSize = hotbar.Length;
        selectedHotbarSlot = 0;
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
    }

    public GameObject magicMissilePrefab;
    public GameObject fireballPrefab;

    private PlayerAbility[] hotbar;

    private int hotbarSize;
    private int selectedHotbarSlot;

    public PlayerAbility GetEquippedSlotAbility() {
        return hotbar[selectedHotbarSlot];
    }
}
