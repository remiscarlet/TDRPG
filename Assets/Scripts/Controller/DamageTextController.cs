using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTextController : MonoBehaviour
{
    private float initializationTime;
    private float timeSinceInitialization;
    private float floatForce = 5.0f;
    private MeshRenderer meshRenderer;

    // Start is called before the first frame update
    void Start() {
        initializationTime = Time.timeSinceLevelLoad;
        Rigidbody damageTextRb = GetComponent<Rigidbody>();
        damageTextRb.AddForce(transform.up * floatForce, ForceMode.Impulse);
        meshRenderer = GetComponent<MeshRenderer>();
    }

    public void Initialize(float dmg) {
        GetComponent<TextMesh>().text = $"{dmg:0.0}";
    }

    private float fadeDuration = 3.0f; // Time to fade invisible
    private void Update() {
        timeSinceInitialization = Time.timeSinceLevelLoad - initializationTime;
        if (timeSinceInitialization > fadeDuration) {
            Destroy(gameObject);
        } else {
            FadeColor();
        }
    }

    private void FadeColor() {
        Material material = meshRenderer.material;
        Color color = material.color;
        color.a = (1 - (timeSinceInitialization / fadeDuration));
        material.color = color;
    }
}
