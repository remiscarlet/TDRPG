using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopController : Interactable {
    private float maxProximityToPrompt = 10.0f;
    private GameObject proximityPrompt;

    private RawImage currSelectedItemRawImage; 

    public GameObject magicMissilePrefab;
    public GameObject fireballPrefab;
    private List<Purchaseable> shopInventory = new List<Purchaseable>(); 

    private int selectedShopInventorySlot = 0;

    void Awake() {
        shopInventory.Add(new MagicMissile(magicMissilePrefab));
        shopInventory.Add(new Fireball(fireballPrefab));
    
        currSelectedItemRawImage = transform.Find("Menu/ItemToBuy").gameObject.GetComponent<RawImage>();
    }

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
            UpdateShopUI();
        } else {
            CloseShopMenu();
        }
    }

    private void UpdateShopUI() {
        Purchaseable selectedItem = shopInventory[selectedShopInventorySlot];
        currSelectedItemRawImage.texture = selectedItem.IconTex;
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
        if (!GameState.ShopOpen) {
            GameState.ShopOpen = true;

            DisplayShopProximity();
            shopMenu.SetActive(true);
        }
    }
    private void CloseShopMenu() {
        if (GameState.ShopOpen) {
            GameState.ShopOpen = false;

            HideShopProximity();
            shopMenu.SetActive(false);
        }
    }

    public override void Activate() {
        if (GameState.ShopOpen) {
            if (Input.GetKey(KeyCode.E)) {
                print("'Selecting next item in menu'");
                SelectNextItemInMenu();
            } else if (Input.GetKey(KeyCode.Q)) {
                print("'Selecting prev item in menu'");
                SelectPrevItemInMenu();
            } else if (Input.GetKey(KeyCode.Space)) {
                print("'Purchase selected item'");
                PurchaseSelectedItemInMenu();
            }
        }
    }

    private float keyRepeatWaitTime = 0.1f;

    private float lastSelectedNext = 0.0f;
    private void SelectNextItemInMenu() {

        print(Time.time);
        print(lastSelectedNext);
        print(Time.time - lastSelectedNext);
        print(keyRepeatWaitTime);
        if (Time.time - lastSelectedNext > keyRepeatWaitTime) {
            if (selectedShopInventorySlot == shopInventory.Count - 1) {
                selectedShopInventorySlot = 0;
            } else {
                selectedShopInventorySlot += 1;
            }
            lastSelectedNext = Time.time;
            print("Huh?");
        } else {
            print("Not yet");
        }
    }

    private float lastSelectedPrev = 0.0f;
    private void SelectPrevItemInMenu() {
        if (Time.time - lastSelectedPrev > keyRepeatWaitTime) {
            if (selectedShopInventorySlot == 0) {
                selectedShopInventorySlot = shopInventory.Count - 1;
            } else {
                selectedShopInventorySlot -= 1;
            }
            lastSelectedPrev = Time.time;
        }
    }

    private void PurchaseSelectedItemInMenu() {
    }
}
