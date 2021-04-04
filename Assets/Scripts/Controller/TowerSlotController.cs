using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSlotController : Interactable {
    private bool IsBeingLookedAt { get; set; } = false;
    private bool IsOccupied { get; set; } = false;
    // Start is called before the first frame update

    private GameObject silhouette;
    private PlayerState playerState;
    private GameObject camera;

    private GameObject towerPrefab;

    private void Start() {
        silhouette = transform.Find("Silhouette").gameObject;
        playerState = ReferenceManager.PlayerStateComponent;
        camera = ReferenceManager.CameraObject;
        towerPrefab = ReferenceManager.Prefabs.Tower;
        print($"towerPrefab: {towerPrefab}");
        MaxInteractDistance = 20.0f;

        foreach (Transform child in transform) {
            // Account for pre-populated towers
            if (child.CompareTag("Tower")) {
                IsOccupied = true;
            }
        }
    }

    // Update is called once per frame
    private void Update() {
        if (!IsPlayerInRange()) {
            IsBeingLookedAt = false;
        } else {
            RaycastHit hit;
            if (Physics.Raycast(
                camera.transform.position,
                camera.transform.forward,
                out hit,
                MaxInteractDistance)) {

                IsBeingLookedAt = MonoBehaviourUtils.IsChild(gameObject, hit.transform.gameObject);
            } else {
                IsBeingLookedAt = false;
            }
        }
        silhouette.SetActive(!IsOccupied && IsBeingLookedAt);
    }

    public override void Activate() {
        if (IsOccupied) {
            return;
        }

        if (Input.GetKey(KeyCode.F)) {
            PurchaseTower();
        }
    }

    private void PurchaseTower() {
        PlayerAbility equippedAbility = playerState.GetEquippedSlotAbility();
        if (equippedAbility == null) {
            return;
        }

        int towerPrice = equippedAbility.TowerPrice;
        if (!playerState.CanAffordPurchase(towerPrice)) {
            return;
        }

        playerState.DeductPoints(towerPrice);
        GameObject tower = Instantiate(towerPrefab, transform);
        tower.GetComponent<Tower1Controller>().SetAbility(equippedAbility);

        IsOccupied = true;
    }
}
