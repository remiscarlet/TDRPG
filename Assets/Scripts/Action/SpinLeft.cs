using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinLeft : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    private float turnSpeed = 45.0f;
    // Update is called once per frame
    void Update() {
        float yRot = turnSpeed * Time.deltaTime;
        transform.Rotate(0, yRot, 0);
    }
}
