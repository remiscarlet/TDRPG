using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    public float sensitivity;
    private GameObject player;
    private float botRotBound = -30.0f; 
    private float topRotBound = 30.0f;
    // Start is called before the first frame update
    void Start() {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update() {
        UpdateCamera();
    }

    void UpdateCamera() {
        float rotateHorizontal = Input.GetAxis("Mouse X");
        float rotateVertical = Input.GetAxis("Mouse Y");

        player.transform.Rotate(0.0f, rotateHorizontal * sensitivity, 0.0f);
        transform.Rotate(-1 * rotateVertical * sensitivity, 0.0f, 0.0f);
        
        // This is ugly. Precompute? Uglier?
        float xRotationAngle = transform.rotation.eulerAngles.x;
        if (xRotationAngle > 180.0f) { xRotationAngle -= 360.0f;}
        print(xRotationAngle);
        if (xRotationAngle < botRotBound || xRotationAngle > topRotBound) {
            print("Reverting angle change");
            transform.Rotate(rotateVertical * sensitivity, 0.0f, 0.0f);
        }
    }
}
