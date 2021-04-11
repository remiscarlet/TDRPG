using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerState : MonoBehaviour {
    private Hotbar hotbar;

    private int points;

    private SpawnManager spawnManager;
    private TextMeshProUGUI scoreTextMesh;
    private TextMeshProUGUI waveTextMesh;
    public void Start() {
        spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        scoreTextMesh = GameObject.Find("Canvas/TopPanel/ScoreText").GetComponent<TextMeshProUGUI>();
        waveTextMesh = GameObject.Find("Canvas/TopPanel/WaveText").GetComponent<TextMeshProUGUI>();

        print("Initializing PlayerState");

        hotbar = new Hotbar();
        points = 1000;
    }

    public void Update() {
        hotbar.UpdateSelectedSlot();

        DrawHUDUI();
    }

    /// <summary>
    /// Add points.
    /// </summary>
    /// <param name="pointsToAdd"></param>
    public void AddPoints(int pointsToAdd) {
        points += pointsToAdd;
    }

    /// <summary>
    /// Deduct points... :thinking:
    /// </summary>
    /// <param name="pointsToDeduct"></param>
    public void DeductPoints(int pointsToDeduct) {
        points -= pointsToDeduct;
    }

    /// <summary>
    /// Returns if player can afford the purchase...
    /// </summary>
    /// <param name="costToPurchase"></param>
    /// <returns></returns>
    public bool CanAffordPurchase(int costToPurchase) {
        return points > costToPurchase;
    }

    /// <summary>
    /// Draws the player HUD UI
    /// </summary>
    private void DrawHUDUI() {
        scoreTextMesh.text = $"Points: {points}";
        waveTextMesh.text = $"Wave: {spawnManager.WaveNum}";

        hotbar.DrawIfHotbarIsUpdated();
    }

    /// <summary>
    /// Adds an additional ability to player hotbar.
    /// </summary>
    /// <param name="ability"></param>
    /// <returns></returns>
    public bool AddNewAbilityToHotbar(Spell ability) {
        return hotbar.AddNewAbility(ability);
    }

    /// <summary>
    /// Returns the ability currently equipped on the hotbar.
    /// </summary>
    /// <returns></returns>
    public Spell GetEquippedSlotAbility() {
        return hotbar.GetCurrentSelectedSlot().Ability;
    }
}
