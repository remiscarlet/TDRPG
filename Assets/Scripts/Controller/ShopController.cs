using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopController : Interactable {
    private const float PurchaseCooldown = 2.0f;
    private const float KeyRepeatWaitTime = 0.2f;
    private const float MaxProximityToPrompt = 10.0f;

    private GameObject proximityPrompt;
    private Mesh proximityPromptMesh;

    private Text priceText;
    private RawImage currSelectedItemRawImage;
    private Texture2D noInventoryTexture;

    public GameObject magicMissilePrefab;
    public GameObject fireballPrefab;
    private List<Purchaseable> shopInventory = new List<Purchaseable>();

    private int selectedShopInventorySlot = 0;

    public bool IsShopStocked { get; set; } = true;

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
    private void Start() {
        player = ReferenceManager.PlayerObject;
        playerState = ReferenceManager.PlayerStateComponent;

        shopMenu = transform.Find("Menu").gameObject;
        proximityPrompt = transform.Find("ProximityPrompt").gameObject;
        proximityPromptMesh = proximityPrompt.GetComponent<MeshFilter>().sharedMesh;
    }

    // Update is called once per frame
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
        } else {
            priceText.text = "";
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
