using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopController : Interactable {
    private const float PurchaseCooldown = 1.0f;
    private const float KeyRepeatWaitTime = 0.2f;
    private const float MaxProximityToPrompt = 10.0f;


    private GameObject player;
    private PlayerState playerState;
    private GameObject shopMenu;
    private GameObject proximityPrompt;
    private Mesh proximityPromptMesh;

    private Text LeftSidebarButtonText;
    private Text rightSidebarButtonText;
    private Text priceText;
    private Text descriptionText;
    private RawImage leftSidebarRawImage;
    private RawImage rightSidebarRawImage;
    private RawImage currSelectedItemRawImage;
    private Texture2D leftSidebarTexture;
    private Texture2D rightSidebarTexture;
    private Texture2D noInventoryTexture;

    private List<Purchaseable> shopInventory = new List<Purchaseable>();

    private int selectedShopInventorySlot = 0;

    public bool IsShopStocked { get; set; } = true;

    void Awake() {
        LeftSidebarButtonText = transform.Find("Menu/ToLeftItem/Button").gameObject.GetComponent<Text>();
        rightSidebarButtonText = transform.Find("Menu/ToRightItem/Button").gameObject.GetComponent<Text>();
        priceText = transform.Find("Menu/Center/Price").GetComponent<Text>();
        descriptionText = transform.Find("Menu/Center/Description").GetComponent<Text>();
        leftSidebarTexture = Resources.Load<Texture2D>("Images/Shop/SidebarArrowLeft");
        rightSidebarTexture = Resources.Load<Texture2D>("Images/Shop/SidebarArrowRight");
        noInventoryTexture = Resources.Load<Texture2D>("Images/Shop/NoInventory");

        leftSidebarRawImage = transform.Find("Menu/ToLeftItem").gameObject.GetComponent<RawImage>();
        rightSidebarRawImage = transform.Find("Menu/ToRightItem").gameObject.GetComponent<RawImage>();
        currSelectedItemRawImage = transform.Find("Menu/Center/ItemToBuy").gameObject.GetComponent<RawImage>();

        shopMenu = transform.Find("Menu").gameObject;
        proximityPrompt = transform.Find("ProximityPrompt").gameObject;
        proximityPromptMesh = proximityPrompt.GetComponent<MeshFilter>().sharedMesh;
    }


    private void Start() {
        player = ReferenceManager.PlayerObject;
        playerState = ReferenceManager.PlayerStateComponent;
        shopInventory.Add(new Spells.MagicMissile(ReferenceManager.Prefabs.MagicMissile));
        shopInventory.Add(new Spells.Fireball(ReferenceManager.Prefabs.Fireball));

        leftSidebarRawImage.texture = leftSidebarTexture;
        rightSidebarRawImage.texture = rightSidebarTexture;

        LeftSidebarButtonText.text = "E"; // TODO: Make input manager
        rightSidebarButtonText.text = "Q";
    }

    private void Update() {
        if (IsPlayerInRange()) {
            OpenShopMenu();
            UpdateShopUI();
        } else {
            CloseShopMenu();
        }
    }

    private void UpdateShopUI() {
        if (IsShopStocked) {
            Purchaseable selectedItem = shopInventory[selectedShopInventorySlot];
            currSelectedItemRawImage.texture = selectedItem.IconTex;

            priceText.text = "Price: " + selectedItem.Price;
            descriptionText.text = "Description:\n" + selectedItem.Description;
        } else {
            priceText.text = "";
            descriptionText.text = "Description:";
        }
    }

    private Purchaseable GetCurrentSelectedShopItem() {
        if (!IsShopStocked) {
            throw new System.Exception("Tried getting shop selection while out of stock!");
        }
        return shopInventory[selectedShopInventorySlot];
    }
    private void DeleteCurrentSelectedShopItem() {
        if (!IsShopStocked) {
            throw new System.Exception("Tried to delete shop inventory while out of stock!");
        }

        shopInventory.RemoveAt(selectedShopInventorySlot);

        if (shopInventory.Count == 0) {
            currSelectedItemRawImage.texture = noInventoryTexture;
            IsShopStocked = false;
        } else if (selectedShopInventorySlot == shopInventory.Count) {
            // This assumes you can only ever buy one item at a time.
            // If can buy multiple items at once, need to make this more robust.
            selectedShopInventorySlot -= 1;
        }
    }

    private void DisplayShopProximity() {
        Vector3 meshSize = proximityPromptMesh.bounds.size;
        float xScale = MaxProximityToPrompt / meshSize.x * 2;
        float yScale = MaxProximityToPrompt / meshSize.y * 2;
        float zScale = MaxProximityToPrompt / meshSize.z * 2;

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
                SelectNextItemInMenu();
            } else if (Input.GetKey(KeyCode.Q)) {
                SelectPrevItemInMenu();
            } else if (Input.GetKey(KeyCode.F)) {
                PurchaseSelectedItemInMenu();
            }
        }
    }

    private float lastSelectedNext = 0.0f;
    private void SelectNextItemInMenu() {
        if (Time.time - lastSelectedNext > KeyRepeatWaitTime) {
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
        if (Time.time - lastSelectedPrev > KeyRepeatWaitTime) {
            if (selectedShopInventorySlot == 0) {
                selectedShopInventorySlot = shopInventory.Count - 1;
            } else {
                selectedShopInventorySlot -= 1;
            }
            lastSelectedPrev = Time.time;
        }
    }

    private float lastPurchased = 0.0f;
    private void PurchaseSelectedItemInMenu() {
        if (Time.time - lastPurchased > PurchaseCooldown && IsShopStocked) {
            Spell toPurchase = GetCurrentSelectedShopItem() as Spell;

            if (playerState.CanAffordPurchase(toPurchase.Price)) {
                playerState.DeductPoints(toPurchase.Price);
                playerState.AddNewAbilityToHotbar(GetCurrentSelectedShopItem() as Spell);
                DeleteCurrentSelectedShopItem();
            } else {
                print("You can't afford that!");
            }

            lastPurchased = Time.time;
        }
    }
}
