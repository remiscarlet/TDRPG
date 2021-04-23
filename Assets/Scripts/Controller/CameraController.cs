using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraController : MonoBehaviour {
    public float sensitivity;
    private GameObject player;
    private Vector3 cameraPosOffset;
    private float botRotBound = 65.0f; // I know, these signs make me sad.
    private float topRotBound = -65.0f; // Rotating downwards on x axis is in the positive direction tho

    private void Start() {
        player = ReferenceManager.PlayerObject;
        cameraPosOffset = transform.position - player.transform.position;
    }

    private void Update() {
        UpdateRotation();
    }

    private void UpdateRotation() {
        float rotateHorizontal = Input.GetAxis("Mouse X");
        float rotateVertical = Input.GetAxis("Mouse Y");

        player.transform.Rotate(0.0f, rotateHorizontal * sensitivity, 0.0f);
        transform.Rotate(-1 * rotateVertical * sensitivity, 0.0f, 0.0f);

        // This is ugly. Precompute? Uglier?
        float xRotationAngle = transform.rotation.eulerAngles.x;
        if (xRotationAngle > 180.0f) { xRotationAngle -= 360.0f;}
        if (xRotationAngle > botRotBound || xRotationAngle < topRotBound) {
            print("Reverting angle change");
            transform.Rotate(rotateVertical * sensitivity, 0.0f, 0.0f);
        }

        float desiredAngle = player.transform.eulerAngles.y;
        Quaternion rot = Quaternion.Euler(0, desiredAngle, 0);
        transform.position = player.transform.position + (rot * cameraPosOffset);
        transform.LookAt(player.transform);
        transform.Rotate(Vector3.right, -30);
    }
}
