using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random=UnityEngine.Random;

public class PlayerController : MonoBehaviour {
    public GameObject projectilePrefab;
    public float forwardForce;
    private Rigidbody playerRb;
    private GameObject shootingTip;
    private GameObject camera;
    // Start is called before the first frame update
    void Start() {
        playerRb = GetComponent<Rigidbody>();
        shootingTip = GameObject.Find("Shooting Tip");
        camera = GameObject.Find("Main Camera");
    }

    // Update is called once per frame
    void Update() {
        UpdateMovement();
        StabilizeTipping();
        Shoot();
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
        float vInput = Input.GetAxis("Vertical");
        float hInput = Input.GetAxis("Horizontal");
        playerRb.AddForce(transform.forward * forwardForce * vInput, ForceMode.Force);
        playerRb.AddForce(transform.right * forwardForce * hInput, ForceMode.Force);
    }

    void Shoot() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            //print(shootingTip.transform.rotation);
            Instantiate(projectilePrefab,
                        shootingTip.transform.position + shootingTip.transform.forward * 1.25f,
                        projectilePrefab.transform.rotation);
        }
    }
}
