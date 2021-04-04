using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Purchaseable {

    private GameObject instancePrefab;
    public GameObject InstancePrefab {
        get { return instancePrefab; }
        set { instancePrefab = value; }
    }

    private int price;
    public int Price {
        get { return price; }
        set { price = value; }
    }

    public int TowerPrice {
        get { return Price * 2; }
    }

    private Texture2D iconTex;
    public Texture2D IconTex {
        get { return iconTex; }
        set { iconTex = value; }
    }

    private string abilityName;
    public string AbilityName {
        get { return abilityName; }
        set { abilityName = value; }
    }

    private string description;
    public string Description {
        get { return description; }
        set { description = value; }
    }
}
