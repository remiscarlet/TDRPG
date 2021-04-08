using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerState : MonoBehaviour {
    private Hotbar hotbar;

    private int points;


    private List<TowerController> towersBeingCombod;
    public bool IsComboingTowers() {
        return towersBeingCombod.Count > 0;
    }

    public void AddToTowerBeingCombod(TowerController tower) {
        towersBeingCombod.Add(tower);
    }

    public void ClearTowersBeingCombod() {
        foreach (TowerController tower in towersBeingCombod) {
            tower.IsBeingCombod = false;
        }

        towersBeingCombod.Clear();
    }

    private SpawnManager spawnManager;
    private TextMeshProUGUI scoreTextMesh;
    private TextMeshProUGUI waveTextMesh;
    public void Start() {
        towersBeingCombod = new List<TowerController>();

        spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        scoreTextMesh = GameObject.Find("Canvas/TopPanel/ScoreText").GetComponent<TextMeshProUGUI>();
        waveTextMesh = GameObject.Find("Canvas/TopPanel/WaveText").GetComponent<TextMeshProUGUI>();

        print("Initializing PlayerState");

        hotbar = new Hotbar();
        points = 1000;
    }

    public void AddPoints(int pointsToAdd) {
        points += pointsToAdd;
    }

    public void DeductPoints(int pointsToDeduct) {
        points -= pointsToDeduct;
    }

    public bool CanAffordPurchase(int costToPurchase) {
        return points > costToPurchase;
    }

    public void Update() {
        hotbar.UpdateSelectedSlot();

        DrawUI();
    }

    private void DrawUI() {
        scoreTextMesh.text = $"Points: {points}";
        waveTextMesh.text = $"Wave: {spawnManager.WaveNum}";

        hotbar.DrawIfHotbarIsUpdated();
    }

    public bool AddNewAbilityToHotbar(Spell ability) {
        return hotbar.AddNewAbility(ability);
    }

    public Spell GetEquippedSlotAbility() {
        return hotbar.GetCurrentSelectedSlot().Ability;
    }
}
