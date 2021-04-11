using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Ehh...
/// </summary>
public class Purchaseable {

    private GameObject instancePrefab;
    public GameObject InstancePrefab {
        get => instancePrefab;
        set => instancePrefab = value;
    }

    private int price;
    public int Price {
        get => price;
        set => price = value;
    }

    public int TowerPrice {
        get => Price * 2;
    }

    private Texture2D iconTex;
    public Texture2D IconTex {
        get => iconTex;
        set => iconTex = value;
    }

    private string abilityName;
    public string AbilityName {
        get => abilityName;
        set => abilityName = value;
    }

    private string description;
    public string Description {
        get => description;
        set => description = value;
    }
}
