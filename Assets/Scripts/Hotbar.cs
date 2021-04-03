using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEngine;
using UnityEngine.UI;

public class Hotbar {
    private int _size = 4;
    public int Size {
        get { return _size; }
    }
    
    private bool _hasHotbarUpdated = true;
    public bool HasHotbarUpdated {
        get { return _hasHotbarUpdated; }
        set { _hasHotbarUpdated = value; }
    }
    
    private int _selectedSlot = 0;
    private int SelectedSlot {
        get { return _selectedSlot; }
        set { _selectedSlot = value; }
    }
    
    private Struct_HotbarSlot[] hotbar;

    public Hotbar() {
        Debug.Log("INITIALIZING HOTBAR");
        Debug.Log($"Size is {Size}");
        hotbar = new Struct_HotbarSlot[Size];
        for (int slot = 0; slot < Size; slot++) {
            // 0-index vs 1-index
            GameObject go = GameObject.Find($"Canvas/HotbarPanel/Slot{slot + 1}");
            hotbar[slot] = new Struct_HotbarSlot(slot, go);
            Debug.Log($"Inserting hotbar slot object: {hotbar[slot]}");
        }
    }

    public Struct_HotbarSlot GetCurrentSelectedSlot() {
        return hotbar[SelectedSlot];
    }
    
    private readonly Color selectedColor = new Color(0.0f, 0.0f, 0.0f, 255.0f);
    private readonly Color unselectedColor = new Color(255.0f, 255.0f, 255.0f, 255.0f);
    public void DrawIfHotbarIsUpdated() {
        if (HasHotbarUpdated) {
            // Draw hotbar items
            for (int i = 0; i < Size; i++) {
                Struct_HotbarSlot hotbarSlot = hotbar[i];
                PlayerAbility ability = hotbarSlot.Ability;
                if (ability == null) {
                    continue;
                }

                hotbarSlot.UIRawImageComponent.texture = ability.IconTex;
            }

            // Draw selected hotbar border
            foreach (Struct_HotbarSlot slot in hotbar) {
                slot.UIImageComponent.color = unselectedColor;
            }

            GetCurrentSelectedSlot().UIImageComponent.color = selectedColor;
            HasHotbarUpdated = false;
        }
    }

    public void UpdateSelectedSlot() {
        int prevSlot = SelectedSlot;
        if (Input.GetKey(KeyCode.Alpha1)) {
            SelectedSlot = 0;
        } else if (Input.GetKey(KeyCode.Alpha2)) {
            SelectedSlot = 1;
        } else if (Input.GetKey(KeyCode.Alpha3)) {
            SelectedSlot = 2;
        } else if (Input.GetKey(KeyCode.Alpha4)) {
            SelectedSlot = 3;
        }

        if (prevSlot != SelectedSlot) {
            HasHotbarUpdated = true;
        }
    }

    public bool AddNewAbility(PlayerAbility ability) {
        int slotToInsertInto = GetLowestUnoccupiedHotbarSlot();
        if (slotToInsertInto == -1) {
            // Hotbar full. Failed to insert.
            return false;
        }

        HasHotbarUpdated = true;
        hotbar[slotToInsertInto].Ability = ability;
        return true;
    }

    private int GetLowestUnoccupiedHotbarSlot() {
        for (int i = 0; i < Size; i++) {
            if (!hotbar[i].IsOccupied) {
                return i;
            }
        }
        return -1;
    }
}
