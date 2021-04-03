using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public class Struct_HotbarSlot {
    public Struct_HotbarSlot(int slot, GameObject slotUI) {
        SlotNumber = slot;
        UIGameObj = slotUI;
        UIImageComponent = slotUI.GetComponent<Image>();
        UIRawImageComponent = slotUI.transform.Find("Item").gameObject.GetComponent<RawImage>();
    }

    public bool IsOccupied {
        get { return Ability != null;  }
    }
    
    public int SlotNumber { get; }
    public GameObject UIGameObj { get; }
    public Image UIImageComponent { get; }
    public RawImage UIRawImageComponent { get; }
    public PlayerAbility Ability { get; set; }

    public override string ToString() => $"Slot {SlotNumber} - {Ability}";
}
