using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Foo : MonoBehaviour {

    private Rigidbody rb;
    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody>();
    }

    private float impulseForce = 1.0f;
    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 forceDir = (Quaternion.Euler(-45.0f, 10.0f, 0) * transform.forward).normalized;
        Debug.Log($"ForceDir vector: {forceDir}, trans.fwd: {transform.forward}");
        rb.AddForce(forceDir * impulseForce, ForceMode.Force);
        transform.rotation = Quaternion.LookRotation(rb.velocity);
    }
}
