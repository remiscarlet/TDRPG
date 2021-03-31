using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour {
    public float shootForce;
    private Rigidbody projectileRb;
    // Start is called before the first frame update
    void Start() {
        projectileRb = GetComponent<Rigidbody>();
        projectileRb.AddForce(transform.forward * shootForce, ForceMode.Impulse); 
    }

    // Update is called once per frame
    void Update() {
    }

    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Ground")) {
            Destroy(gameObject);
        }
    }   
}
