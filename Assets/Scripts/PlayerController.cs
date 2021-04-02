using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random=UnityEngine.Random;

public class PlayerController : MonoBehaviour {
    public GameObject projectilePrefab;
    public float forwardForce;
    public float runSpeedup = 5.0f;
    private Rigidbody playerRb;
    private GameObject shootingTip;
    private GameObject camera;
    private float maxInteractDistance = 25.0f;

    private UIRaycasterUtil shopMenuRaycastUtil;
    private PlayerState playerState;

    // Start is called before the first frame update
    void Start() {
        shopMenuRaycastUtil = ReferenceManager.ShopMenuRaycastUtilComponent;
        playerState = ReferenceManager.PlayerStateComponent;

        playerRb = GetComponent<Rigidbody>();
        shootingTip = GameObject.Find("Shooting Tip");
        camera = GameObject.Find("Main Camera");
    }

    void FixedUpdate() {
        UpdateMovement();
        StabilizeTipping();
        Shoot();
    }

    void Update() {
        Interact();
    }

    Vector3 normalizeEulerAngle(Vector3 eulerAngle) {
        // Gross.
        Vector3 normalized = new Vector3(0, 0, 0);
        if (eulerAngle.x > 180.0f) {
            normalized.x = eulerAngle.x - 360.0f;
        } else {
            normalized.x = eulerAngle.x;
        }
        if (eulerAngle.y > 180.0f) {
            normalized.y = eulerAngle.y - 360.0f;
        } else {
            normalized.y = eulerAngle.y;
        }
        if (eulerAngle.z > 180.0f) {
            normalized.z = eulerAngle.z - 360.0f;
        } else {
            normalized.z = eulerAngle.z;
        }

        return normalized;
    }

    private float tippingEpsilon = 0.01f;
    void StabilizeTipping() {
        var eulerRot = normalizeEulerAngle(transform.rotation.eulerAngles);
        var newRot = new Vector3(0.0f, 0.0f, 0.0f);
        if (Math.Abs(eulerRot.x) > tippingEpsilon) {
            newRot.x = -1 * eulerRot.x / 2.0f;
        } else {
            newRot.x = -1 * eulerRot.x;
        }

        if (Math.Abs(eulerRot.z) > tippingEpsilon) {
            newRot.z = -1 * eulerRot.z / 2.0f;
        } else {
            newRot.z = -1 * eulerRot.z;
        }

        //print($"Correcting tipping by rotating: {newRot} - Orig rot: {eulerRot}");
        transform.Rotate(newRot);
    }

    void UpdateMovement() {
        bool isShiftPressed = Input.GetKey(KeyCode.LeftShift);
        //print($"isShiftPressed: {isShiftPressed}");
        float vInput = Input.GetAxis("Vertical");
        float hInput = Input.GetAxis("Horizontal");

        if (isShiftPressed) {
            print($"Applying multiplier of {runSpeedup} to force on rb");
            playerRb.AddForce(transform.forward * forwardForce * vInput * runSpeedup, ForceMode.Force);
            playerRb.AddForce(transform.right * forwardForce * hInput * runSpeedup, ForceMode.Force);
        } else {
            playerRb.AddForce(transform.forward * forwardForce * vInput, ForceMode.Force);
            playerRb.AddForce(transform.right * forwardForce * hInput, ForceMode.Force);
        }
    }

    private float lastShotAt;
    void Shoot() {
        var ability = playerState.GetEquippedSlotAbility();
        if (ability != null) {
            if (!GameState.ShopOpen
                && Input.GetKey(KeyCode.Space)
                && (Time.time - ability.LastShotAt) > ability.GetWaitTimeBetweenShots()) {
                ability.SpawnInstances(shootingTip.transform, camera.transform.rotation);

                ability.LastShotAt = Time.time;
            }
            if (!GameState.ShopOpen
                && Input.GetKey(KeyCode.B)) {
                // Kek.
                ability.SpawnInstances(shootingTip.transform, camera.transform.rotation);
            }
        } else {
            //print("No ability/item equipped, dummy.");
        }
    }

    Interactable GetInteractable(GameObject go) {
        Interactable interactable = go.GetComponent<Interactable>();
        if (interactable == null) {
            return GetInteractable(go.transform.parent.gameObject);
        } else {
            return interactable;
        }
    }

    void Interact() {
        GameObject interactableGameObj = null;
        if (Input.anyKey) {
            RaycastHit hit;
            // Physics raycast
            if(Physics.Raycast(camera.transform.position, camera.transform.forward, out hit, maxInteractDistance)) {
                if (hit.transform.gameObject.CompareTag("Interactable")) {
                    interactableGameObj = hit.transform.gameObject;
                }
            }

            // UI/canvas raycast
            GameObject hitGameObj;
            if (shopMenuRaycastUtil.IsPlayerFacingUIElem(out hitGameObj)) {
                interactableGameObj = hitGameObj;
            }
        }

        if (interactableGameObj != null) {
            Interactable interactable = GetInteractable(interactableGameObj);
            interactable.Activate();
        }
    }
}
