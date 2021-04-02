using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopController : Interactable {
    private float maxProximityToPrompt = 10.0f;
    private GameObject proximityPrompt;

    private GameObject player;
    private GameObject shopMenu;
    // Start is called before the first frame update
    void Start() {
        player = ReferenceManager.PlayerObject;

        shopMenu = transform.Find("Menu").gameObject;
        proximityPrompt = transform.Find("ProximityPrompt").gameObject;
    }

    // Update is called once per frame
    void Update() {
        if (PlayerCloseEnough()) {
            OpenShopMenu();
        } else {
            CloseShopMenu();
        }

        if (GameState.ShopOpen) {
            if (Input.GetKeyDown(KeyCode.E)) {
                print("'Selecting next item in menu'");
                // SelectNextItemInMenu();
            } else if (Input.GetKeyDown(KeyCode.Q)) {
                print("'Selecting prev item in menu'");
                // SelectPrevItemInMenu();
            } else if (Input.GetKeyDown(KeyCode.Space)) {
                print("'Purchase selected item'");
            }
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

    private void OpenShopMenu() {
        GameState.ShopOpen = true;

        DisplayShopProximity();
        shopMenu.SetActive(true);
    }
    private void CloseShopMenu() {
        GameState.ShopOpen = false
        ;
        HideShopProximity();
        shopMenu.SetActive(false);
    }
}
