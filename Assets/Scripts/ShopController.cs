using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopController : Interactable {
    private float maxProximityToPrompt = 10.0f;
    private GameObject proximityPrompt;

    private GameObject player;
    // Start is called before the first frame update
    void Start() {
        player = GameObject.Find("Player");
        proximityPrompt = transform.Find("ProximityPrompt").gameObject;
    }

    // Update is called once per frame
    void Update() {
        if (PlayerCloseEnough()) {
            DisplayShopProximity();
        } else {
            HideShopProximity();
        }
    }

    private bool PlayerCloseEnough() {
        float dist = Vector3.Distance(transform.position, player.transform.position);
        //print($"Distance to player is: {dist}");
        return dist < maxProximityToPrompt;
    }

    private void DisplayShopProximity() {
        Mesh mesh = proximityPrompt.GetComponent<MeshFilter>().sharedMesh;
        Vector3 meshSize = mesh.bounds.size;
        //print($"Mesh Sizes: {meshSize.x}, {meshSize.y}, {meshSize.z}");
        float xScale = maxProximityToPrompt / meshSize.x * 2;
        float yScale = maxProximityToPrompt / meshSize.y * 2;
        float zScale = maxProximityToPrompt / meshSize.z * 2;
        
        proximityPrompt.transform.localScale = new Vector3(xScale, yScale, zScale);
    }

    private void HideShopProximity() {
        proximityPrompt.transform.localScale = Vector3.one;
    }

    public override void Activate() {
        OpenShopMenu();
    }

    private void OpenShopMenu() {
        print("Opening shop menu");
    }
}
