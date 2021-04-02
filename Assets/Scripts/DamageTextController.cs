using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTextController : MonoBehaviour
{
    private float initializationTime;
    private float timeSinceInitialization;
    private float floatForce = 5.0f;

    // Start is called before the first frame update
    void Start() {
        initializationTime = Time.timeSinceLevelLoad;
        Rigidbody damageTextRb = GetComponent<Rigidbody>();
        damageTextRb.AddForce(transform.up * floatForce, ForceMode.Impulse);
    }

    public void Initialize(float dmg) {
        GetComponent<TextMesh>().text = $"{dmg:0.0}";
    }

    private float fadeDuration = 3.0f; // Time to fade invisible
    private void Update() {
        timeSinceInitialization = Time.timeSinceLevelLoad - initializationTime;
        if (timeSinceInitialization > fadeDuration) {
            //print($"Alive for {timeSinceInitialization} secs - Destroying");
            Destroy(gameObject);
        } else {
            FadeColor();
        }
    }

    private void FadeColor() {
        var material = GetComponent<MeshRenderer>().material;
        Color color = material.color;
        color.a = (1 - (timeSinceInitialization / fadeDuration));
        material.color = color;
    }
}
