using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopController : Interactable {
    private float maxProximityToPrompt = 10.0f;
    private GameObject proximityPrompt;
    private Mesh proximityPromptMesh;

    private Text priceText;
    private RawImage currSelectedItemRawImage; 
    private Texture2D noInventoryTexture;

    public GameObject magicMissilePrefab;
    public GameObject fireballPrefab;
    private List<Purchaseable> shopInventory = new List<Purchaseable>(); 

    private int selectedShopInventorySlot = 0;

    private bool shopIsStocked = true;
    public bool ShopIsStocked {
        get { return shopIsStocked; }
        set { shopIsStocked = value; }
    }

    void Awake() {
        shopInventory.Add(new MagicMissile(magicMissilePrefab));
        shopInventory.Add(new Fireball(fireballPrefab));
    
        priceText = transform.Find("Menu/Price").GetComponent<Text>();
        noInventoryTexture = Resources.Load<Texture2D>("Images/Shop/NoInventory");

        currSelectedItemRawImage = transform.Find("Menu/ItemToBuy").gameObject.GetComponent<RawImage>();
    }

    private GameObject player;
    private PlayerState playerState;
    private GameObject shopMenu;
    // Start is called before the first frame update
    void Start() {
        player = ReferenceManager.PlayerObject;
        playerState = ReferenceManager.PlayerStateComponent;

        shopMenu = transform.Find("Menu").gameObject;
        proximityPrompt = transform.Find("ProximityPrompt").gameObject;
        proximityPromptMesh = proximityPrompt.GetComponent<MeshFilter>().sharedMesh;
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
        if (ShopIsStocked) {
            Purchaseable selectedItem = shopInventory[selectedShopInventorySlot];
            currSelectedItemRawImage.texture = selectedItem.IconTex;

            priceText.text = "Price: " + selectedItem.Price;
        } else {
            priceText.text = "";
        }
    }

    private Purchaseable GetCurrentSelectedShopItem() {
        if (!ShopIsStocked) {
            throw new System.Exception("Tried getting shop selection while out of stock!");
        }
        return shopInventory[selectedShopInventorySlot];
    }
    private void DeleteCurrentSelectedShopItem() {
        if (!ShopIsStocked) {
            throw new System.Exception("Tried to delete shop inventory while out of stock!");
        }

        shopInventory.RemoveAt(selectedShopInventorySlot);

        if (shopInventory.Count == 0) {
            currSelectedItemRawImage.texture = noInventoryTexture;
            ShopIsStocked = false;
        }
    }

    private bool PlayerCloseEnough() {
        float dist = Vector3.Distance(transform.position, player.transform.position);
        //print($"Distance to player is: {dist}");
        return dist < maxProximityToPrompt;
    }

    private void DisplayShopProximity() {
        Vector3 meshSize = proximityPromptMesh.bounds.size;
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
        if (Time.time - lastSelectedNext > keyRepeatWaitTime) {
            if (selectedShopInventorySlot == shopInventory.Count - 1) {
                selectedShopInventorySlot = 0;
            } else {
                selectedShopInventorySlot += 1;
            }
            lastSelectedNext = Time.time;
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

    private float purchaseCooldown = 2.0f;
    private float lastPurchased = 0.0f;
    private void PurchaseSelectedItemInMenu() {
        if (Time.time - lastPurchased > purchaseCooldown && ShopIsStocked) {
            PlayerAbility toPurchase = GetCurrentSelectedShopItem() as PlayerAbility;

            if (playerState.CanAffordPurchase(toPurchase.Price)) { 
                playerState.DeductPoints(toPurchase.Price);
                playerState.AddNewAbilityToHotbar(GetCurrentSelectedShopItem() as PlayerAbility);
                DeleteCurrentSelectedShopItem();
            } else {
                print("You can't afford that!");
            }
            
            lastPurchased = Time.time;
        }
    }
}
