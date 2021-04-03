using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerState : MonoBehaviour {
    private Hotbar hotbar;
    
    private int points;
    public int Points {
        get { return points; }
        set { points = value; }
    }
    
    private SpawnManager spawnManager;
    private TextMeshProUGUI scoreTextMesh;
    private TextMeshProUGUI waveTextMesh;
    public void Start() {
        spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        scoreTextMesh = GameObject.Find("Canvas/TopPanel/ScoreText").GetComponent<TextMeshProUGUI>();
        waveTextMesh = GameObject.Find("Canvas/TopPanel/WaveText").GetComponent<TextMeshProUGUI>();

        print("Initializing PlayerState");
        
        hotbar = new Hotbar();
        Points = 500;
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
        hotbar.UpdateSelectedSlot();

        DrawUI();
    }

    private void DrawUI() {
        scoreTextMesh.text = $"Points: {Points}";
        waveTextMesh.text = $"Wave: {spawnManager.WaveNum}";
  
        hotbar.DrawIfHotbarIsUpdated();
    }

    public bool AddNewAbilityToHotbar(PlayerAbility ability) {
        return hotbar.AddNewAbility(ability);
    }

    public PlayerAbility GetEquippedSlotAbility() {
        return hotbar.GetCurrentSelectedSlot().Ability;
    }
}